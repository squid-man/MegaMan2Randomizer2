using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using MM2Randomizer.Data;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;

namespace MM2Randomizer.Randomizers
{
    using FtXmlModule = MM2Randomizer.Data.FtModule;
    using FtXmlSong = MM2Randomizer.Data.FtSong;

    public enum ESoundEngine
    {
        C2,
        Ft,
    }

    public interface ISong
    {
        ESoundEngine Engine { get; }

        ESoundTrackUsage Usage { get; }
        ISet<String> Uses { get; }

        Int32 Bank { get; set; }
        Int32 Index { get; set; }
    }

    public class C2Song : ISong
    {
        public C2Song(String in_SongName, Int32 in_OriginalStartAddress, String in_SongBytesStr, SoundTrack? in_SoundTrack = null)
        {
            this.SoundTrack = in_SoundTrack;
            this.OriginalStartAddress = in_OriginalStartAddress;
            this.SongName = in_SongName;
            this.Uses = (in_SoundTrack is not null) ? in_SoundTrack.Uses : new();

            // Parse song bytes from hex String
            List<Byte> songBytes = new();
            while (in_SongBytesStr.Length > 0)
            {
                String twoCharByte = in_SongBytesStr.Substring(0, 2);
                songBytes.Add(Byte.Parse(twoCharByte, NumberStyles.HexNumber));
                in_SongBytesStr = in_SongBytesStr.Remove(0, 2);
            }

            // Header consists of the first 11 bytes, followed by the song data
            this.SongHeader = songBytes.GetRange(0, 11);
            this.SongData = songBytes.GetRange(11, songBytes.Count - 11);

            // Parse channel information
            Byte byteSmall = SongHeader[1];
            Byte byteLarge = SongHeader[2];
            Int32 absolutePos = byteSmall + (byteLarge * 256);
            Channel1Index = absolutePos - in_OriginalStartAddress - 11;

            byteSmall = SongHeader[3];
            byteLarge = SongHeader[4];
            absolutePos = byteSmall + (byteLarge * 256);
            Channel2Index = absolutePos - in_OriginalStartAddress - 11;

            byteSmall = SongHeader[5];
            byteLarge = SongHeader[6];
            absolutePos = byteSmall + (byteLarge * 256);
            Channel3Index = absolutePos - in_OriginalStartAddress - 11;

            byteSmall = SongHeader[7];
            byteLarge = SongHeader[8];

            if (byteSmall != 0 || byteLarge != 0) // Ignore some songs with no noise channel
            {
                absolutePos = byteSmall + (byteLarge * 256);
                Channel4Index = absolutePos - in_OriginalStartAddress - 11;
            }

            // Parse vibrato information
            byteSmall = songBytes[9];
            byteLarge = songBytes[10];
            absolutePos = byteSmall + (byteLarge * 256);
            VibratoIndex = absolutePos - in_OriginalStartAddress - 11;

            // Count length of vibrato section
            Int32 i = VibratoIndex;

            while (true)
            {
                if (i >= SongData.Count ||
                    i == Channel1Index ||
                    i == Channel2Index ||
                    i == Channel3Index ||
                    i == Channel4Index)
                {
                    VibratoLength = i - VibratoIndex;
                    break;
                }

                i++;
            }
        }

        public SoundTrack? SoundTrack { get; set; }
        public String SongName { get; set; }
        public Int32 Bank { get; set; }
        public Int32 Index { get; set; }
        public Int32 StartAddress { get; set; }
        public List<Byte> SongHeader { get; set; }
        public List<Byte> SongData { get; set; }

        public Int32 Channel1Index { get; set; }
        public Int32 Channel2Index { get; set; }
        public Int32 Channel3Index { get; set; }
        public Int32 Channel4Index { get; set; }
        public Int32 VibratoIndex { get; set; }
        public Int32 VibratoLength { get; set; }
        public Int32 OriginalStartAddress { get; set; }

        public Int32 TotalLength
        {
            get
            {
                return SongHeader.Count + SongData.Count;
            }
        }

        public ESoundEngine Engine { get { return ESoundEngine.C2; } }

        public ISet<String> Uses { get; set; }
        public ESoundTrackUsage Usage
        {
            get { return SoundTrackUsage.FromStrings(Uses); }
        }
    }

    public class FtSong : ISong
    {
        public ESoundEngine Engine { get { return ESoundEngine.Ft; } }

        public ISet<String> Uses { get; private set; }
        public ESoundTrackUsage Usage
        {
            get { return SoundTrackUsage.FromStrings(Uses); }
        }

        public Int32 Bank { get; set; }
        public Int32 Index { get; set; }

        public readonly FtXmlModule ModuleInfo;
        public readonly FtXmlSong? Info;

        public readonly Boolean Enabled;
        public readonly Int32 Number;
        public readonly String Title;
        public readonly Boolean SwapSquareChans;

        public String Author { get { return ModuleInfo.Author; } }
        public Byte[] TrackData { get { return ModuleInfo.TrackData; } }

        public FtSong(FtXmlModule in_Mod, Int32 in_SongIdx)
        {
            ModuleInfo = in_Mod;
            
            if (in_Mod.Songs.Count > 0)
            {
                // in_SongIdx is the index in in_Mod.Songs, NOT the index in the FTM itself
                Info = in_Mod.Songs[in_SongIdx];

                Enabled = Info.Enabled is not null ? (bool)Info.Enabled : in_Mod.Enabled;
                Number = Info.Number;
                Title = Info.Title;
                Uses = (Info.Uses.Count > 0) ? Info.Uses : in_Mod.Uses;
                SwapSquareChans = (Info.SwapSquareChans is not null) ? (bool)Info.SwapSquareChans : in_Mod.SwapSquareChans;
            }
            else
            {
                Info = null;

                Enabled = in_Mod.Enabled;
                Number = 0;
                Title = in_Mod.Title;
                Uses = in_Mod.Uses;
                SwapSquareChans = in_Mod.SwapSquareChans;
            }
        }
    }

    public class RMusic : IRandomizer
    {
        /// <summary>
        /// Map of which game songs belong to which music uses.
        /// </summary>
        private Dictionary<ESoundTrackUsage, EMusicID[]> UsesMusicIds = new Dictionary<ESoundTrackUsage, EMusicID[]>
        {
            { ESoundTrackUsage.Intro, new EMusicID[]{EMusicID.Intro} },
            { ESoundTrackUsage.Title, new EMusicID[]{EMusicID.Title} },
            { ESoundTrackUsage.StageSelect, new EMusicID[]{EMusicID.StageSelect} },
            { ESoundTrackUsage.Stage, new EMusicID[]{EMusicID.Flash, EMusicID.Wood, EMusicID.Crash, EMusicID.Heat, EMusicID.Air, EMusicID.Metal, EMusicID.Quick, EMusicID.Bubble, EMusicID.Wily1, EMusicID.Wily2, EMusicID.Wily3, EMusicID.Wily4, EMusicID.Wily6 } },
            { ESoundTrackUsage.Boss, new EMusicID[]{EMusicID.Boss} },
            { ESoundTrackUsage.Refights, new EMusicID[]{EMusicID.Wily5} },
            { ESoundTrackUsage.Ending, new EMusicID[]{EMusicID.Ending} },
            { ESoundTrackUsage.Credits, new EMusicID[]{EMusicID.Credits} },
        };

        /// <summary>
        /// Offset of the Capcom 2 song address table in the ROM.
        /// </summary>
        private const Int32 C2SongTblOffs = 0x30a60;

        /// <summary>
        /// Offset of the mm2ft song map table in the ROM.
        /// </summary>
        private const Int32 SongMapOffs = 0x30b60;

        /// <summary>
        /// Offset of the mm2ft boss song map table in the ROM.
        /// </summary>
        private const Int32 BossSongMapOffs = 0x7f2f3;

        /// <summary>
        /// Length of the mm2ft boss song map table.
        /// </summary>
        private const Int32 BossSongMapLen = 0x16;

        /// <summary>
        /// First unused song index available for boss music. The last is 0x7f, but there isn't any situation where that will be reached.
        /// </summary>
        private const Int32 FirstBossMusicId = 0x43;

        /// <summary>
        /// The size of each ROM bank.
        /// </summary>
        private const Int32 BankSize = 0x2000;

        /// <summary>
        /// The first empty bank available for music.
        /// </summary>
        private const Int32 FirstFreeBank = 0x20;

        /// <summary>
        /// The last empty bank available for music.
        /// </summary>
        private const Int32 LastFreeBank = 0x3d;

        /// <summary>
        /// The address to place FTMs in each bank.
        /// </summary>
        private const Int32 FtmBaseAddress = 0xa000;

        /// <summary>
        /// The default uses for tracks that have no uses specified on the song or module.
        /// </summary>
        private HashSet<String> DefaultUses = new(StringComparer.InvariantCultureIgnoreCase){ "Stage", "Credits" };

        private StringBuilder debug = new();

        public RMusic()
        {
        }

        public override String ToString()
        {
            return debug.ToString();
        }

        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            debug.AppendLine();
            debug.AppendLine("Random Music Module");
            debug.AppendLine("--------------------------------------------");

            this.ImportMusic(in_Patch, in_Context.Seed);
        }


        public void ImportMusic(Patch in_Patch, ISeed in_Seed)
        {
            // Read songs from file, parse and add to list
            SoundTrackSet soundTrackSet = Properties.Resources.SoundTrackConfiguration.Deserialize<SoundTrackSet>();
            List<ISong> songs =
                (from soundTrack in soundTrackSet
                 where soundTrack.Enabled
                 select new C2Song(soundTrack.Title, Int32.Parse(soundTrack.StartAddress, NumberStyles.HexNumber), soundTrack.TrackData, soundTrack)).ToList<ISong>();

            FtModuleSet moduleSet = Properties.Resources.FtSoundTrackConfiguration.Deserialize<FtModuleSet>();
            List<FtSong> ftmSongs = new();
            foreach (FtXmlModule xmlMod in moduleSet)
            {
                Debug.Assert(xmlMod.Size <= BankSize, $"FTM {xmlMod.Title} is larger than a bank");
                
                for (int i = 0; i < Math.Max(xmlMod.Songs.Count, 1); i++)
                {
                    FtSong song = new(xmlMod, i);
                    if (song.Enabled)
                    {
                        ftmSongs.Add(song);
                        songs.Add(song);
                    }
                }
            }

            debug.AppendLine($"{songs.Count} C2 and {ftmSongs.Count} FT songs loaded.");

            // Create the usage type lists of songs
            Dictionary<String, List<Int32>> usesSongIdcs = new(StringComparer.InvariantCultureIgnoreCase);
            foreach (String name in Enum.GetNames(typeof(ESoundTrackUsage)))
                usesSongIdcs[name] = new List<Int32>();

            for (Int32 songIdx = 0; songIdx < songs.Count; songIdx++)
            {
                ISong song = songs[songIdx];
                ISet<String> uses = (song.Uses.Count > 0) ? song.Uses : DefaultUses;
                foreach (String useStr in uses)
                    usesSongIdcs[useStr].Add(songIdx);
            }

            // Pick and import the songs
            Dictionary<EMusicID, Int32>? selSongMap = null;
            while (selSongMap is null)
            {
                selSongMap = SelectSongs(in_Seed, usesSongIdcs);

                HashSet<int> selSongIdcs = new(selSongMap.Values);
                var banksSongs = PlaceSongsInBanks((from idx in selSongIdcs select songs[idx]).ToList());
                if (banksSongs is not null)
                    ImportSongs(in_Patch, in_Seed, banksSongs);
                else 
                    selSongMap = null;
            }

            // Update the sound tables
            foreach (EMusicID musicId in selSongMap.Keys)
            {
                Int32 songIdx = selSongMap[musicId];
                Int32 musicIdx = (Int32)musicId;
                Int32 tblMusicIdx = musicIdx;
                Int32 SongMapPtrOffs = SongMapOffs + musicIdx * 2;

                ISong isong = songs[songIdx];
                int bankIdx = isong.Bank;
                if (isong.Engine == ESoundEngine.C2)
                {
                    C2Song song = (C2Song)isong;
                    Int32 TblPtrOffset = C2SongTblOffs + musicIdx * 2;

                    // Update the song address table
                    in_Patch.AddWord(TblPtrOffset, song.StartAddress, $"Song {musicIdx} Pointer Offset");
                    debug.AppendLine($"{Enum.GetName(typeof(EMusicID), musicId)} song: {song.SongName}, {song.OriginalStartAddress}");
                }
                else
                {
                    FtSong song = (FtSong)isong;

                    bankIdx = ~song.Bank;
                    tblMusicIdx = song.Number;

                    debug.AppendLine($"{Enum.GetName(typeof(EMusicID), musicId)} song: {song.ModuleInfo.Title} - {song.Title}");
                }

                in_Patch.Add(SongMapPtrOffs++, (byte)bankIdx, $"Song {musicIdx} Bank Index");
                in_Patch.Add(SongMapPtrOffs++, (byte)tblMusicIdx, $"Song {musicIdx} Song Index");

                // For boss tracks, update the boss song table
                if (musicIdx >= FirstBossMusicId && musicIdx < FirstBossMusicId + BossSongMapLen)
                {
                    Int32 bossIdx = musicIdx - FirstBossMusicId;
                    in_Patch.Add(BossSongMapOffs + bossIdx, (byte)musicIdx, $"Boss {bossIdx} Song Index");
                }
            }

            Boolean testRebase = false;
            if (testRebase)
                TestRebaseSongs(songs);
        }

        private Dictionary<EMusicID, Int32> SelectSongs(ISeed in_Seed, IReadOnlyDictionary<String, List<Int32>> in_UsesSongIdcs)
        {
            Dictionary<EMusicID, int> selSongMap = new();

            foreach (String usageStr in in_UsesSongIdcs.Keys)
            {
                var usageSongs = in_UsesSongIdcs[usageStr];
                if (usageSongs.Count == 0)
                    continue; // Nothing to randomize

                EMusicID[]? usageMusicIds;
                if (!StringComparer.InvariantCultureIgnoreCase.Equals(usageStr, "Boss"))
                    usageMusicIds = UsesMusicIds[SoundTrackUsage.Values[usageStr]];
                else
                    usageMusicIds = (from idx in Enumerable.Range(FirstBossMusicId, BossSongMapLen) select (EMusicID)idx).ToArray();

                // Duplicate the candidate song list until there's enough for all the song slots
                var songIdcs = usageSongs;
                if (songIdcs.Count < usageMusicIds.Length)
                {
                    songIdcs = usageSongs.ToList();
                    while (songIdcs.Count < usageMusicIds.Length)
                        songIdcs.AddRange(usageSongs);
                }

                // Finally, make the list
                var cndIdcs = in_Seed.Shuffle(songIdcs).ToList();
                for (Int32 i = 0; i < usageMusicIds.Length; i++)
                    selSongMap[usageMusicIds[i]] = cndIdcs[i];
            }

            return selSongMap;
        }

        private Dictionary<int, List<ISong>>? PlaceSongsInBanks(IReadOnlyList<ISong> in_Songs)
        {
            // Separate FT and C2 songs, as the former must be at the start of banks
            Dictionary<FtXmlModule, List<FtSong>> ftmsSongs = new(ReferenceEqualityComparer.Instance);
            List<FtSong> ftSongs = new();
            List<C2Song?> c2Songs = new();
            foreach (ISong isong in in_Songs)
            {
                if (isong.Engine == ESoundEngine.Ft)
                {
                    FtSong song = (FtSong)isong;
                    FtXmlModule mod = song.ModuleInfo;

                    List<FtSong>? ftmSongs = null;
                    if (!ftmsSongs.TryGetValue(mod, out ftmSongs))
                    {
                        ftmSongs = new List<FtSong>();
                        ftmsSongs[mod] = ftmSongs;
                    }
                    
                    ftmSongs.Add(song);
                    ftSongs.Add(song);
                }
                else
                    c2Songs.Add((C2Song)isong);
            }

            if (ftmsSongs.Count > LastFreeBank - FirstFreeBank + 1)
                // Not enough banks for all the FTMs
                return null;

            List<FtXmlModule> ftmsBySize = new(ftmsSongs.Keys);
            ftmsBySize.Sort((a, b) => ((a is not null && b is not null) ? -a.Size.CompareTo(b.Size) : 0));

            c2Songs.Sort((a, b) => ((a is not null && b is not null) ? -a.TotalLength.CompareTo(b.TotalLength) : 0));

            // Place the songs in each bank
            Dictionary<int, List<ISong>> banksSongs = new();
            int ftModIdx = 0;
            int numTracks = c2Songs.Count + ftmsBySize.Count;
            int songsLeft = numTracks;
            for (Int32 bankIdx = FirstFreeBank; bankIdx <= LastFreeBank && songsLeft > 0; bankIdx++)
            {
                List<ISong> bankSongs = banksSongs[bankIdx] = new();

                // Place an FTM (if any) at the start of the bank
                Int32 sizeLeft = BankSize;
                if (ftModIdx < ftmsBySize.Count)
                {
                    FtXmlModule mod = ftmsBySize[ftModIdx++];

                    bankSongs.AddRange(ftmsSongs[mod]);

                    songsLeft--;
                    sizeLeft -= mod.Size;
                }

                // Then fill it in with C2 songs
                for (Int32 listIdx = 0; listIdx < c2Songs.Count && songsLeft > 0; listIdx++)
                {
                    C2Song? song = c2Songs[listIdx];
                    if (song is not null && song.TotalLength <= sizeLeft)
                    {
                        banksSongs[bankIdx].Add(song);
                        sizeLeft -= song.TotalLength;

                        c2Songs[listIdx] = null;
                        songsLeft--;
                    }
                }
            }

            // DEBUG DEBUG
            if (songsLeft > 0)
            {
                debug.AppendLine($"{numTracks} songs selected. insufficient space.");

                return null;
            }

            debug.AppendLine($"{numTracks} songs selected.");

            return banksSongs;
        }

        private void ImportSongs(Patch in_Patch, ISeed in_Seed, Dictionary<int, List<ISong>> in_BanksSongs)
        {
            // Rebase and write the songs
            foreach (var bankAndSongs in in_BanksSongs)
            {
                int bankIdx = bankAndSongs.Key;
                var bankSongs = bankAndSongs.Value;
                int songStartRom = bankIdx * BankSize + 0x10;
                int slotIdx = 0;

                // Import the FTM if any
                FtXmlModule? modInfo = null;
                FtmBinary? ftm = null;
                for (; slotIdx < bankSongs.Count; slotIdx++)
                {
                    ISong isong = bankSongs[slotIdx];
                    if (isong.Engine != ESoundEngine.Ft)
                        break;

                    FtSong song = (FtSong)isong;

                    if (slotIdx == 0)
                    {
                        modInfo = song.ModuleInfo;
                        ftm = new FtmBinary(song.TrackData, modInfo.StartAddress);
                    }

                    if (song.SwapSquareChans)
                    {
                        Debug.Assert(ftm is not null);
                        ftm.SwapSquareChans(Math.Max(song.Number, 0));
                    }

                    song.Bank = bankIdx;
                }

                if (modInfo is not null)
                {
                    // Import the module
                    Debug.Assert(ftm is not null);

                    int songAddr = (songStartRom - 0x10) % BankSize + FtmBaseAddress;

                    ftm.Rebase(songAddr);

                    in_Patch.Add(songStartRom, modInfo.TrackData, $"FTM {modInfo.Title}");

                    modInfo.StartAddress = songAddr;

                    songStartRom += modInfo.Size;
                }

                // Import the C2 songs if any
                for (; slotIdx < bankSongs.Count; slotIdx++)
                {
                    C2Song song = (C2Song)bankSongs[slotIdx];
                    int songAddr = (songStartRom - 0x10) % BankSize + FtmBaseAddress;

                    RebaseC2Song(song, bankIdx, songAddr);

                    // Write song header and data
                    songStartRom = in_Patch.Add(songStartRom, song.SongHeader, $"Song Header for {song.SongName}");
                    songStartRom = in_Patch.Add(songStartRom, song.SongData, $"Song Data for {song.SongName}");
                }
            }
        }

        private void RebaseC2Song(C2Song song, Int32 songBank, Int32 songAddr)
        {
            song.Bank = songBank;
            song.StartAddress = songAddr;

            // Header: Calculate 4 channel addresses and instrument table address
            BinaryBuffer hdrBuff = new(song.SongHeader);
            hdrBuff.Seek(1);

            for (Int32 chanIdx = 0; chanIdx < 5; chanIdx++)
            {
                Int32 origAddr = hdrBuff.ReadUInt16LE(false);
                if (origAddr == 0 || origAddr == 0xffff)
                {
                    if (chanIdx == 4)
                        debug.AppendLine($"WARNING: Song {song.SongName} has a null instrument table pointer.");

                    hdrBuff.Seek(2, System.IO.SeekOrigin.Current);

                    continue;
                }

                Int32 offs = origAddr - song.OriginalStartAddress;
                if (offs >= song.TotalLength || offs < 0)
                    debug.AppendLine($"WARNING: Song {song.SongName} channel {chanIdx} points to an external location.");

                hdrBuff.WriteLE(checked((UInt16)(offs + songAddr)));
            }

            // Song Data: Traverse stream and change loop pointers
            for (Int32 i = 0; i < song.SongData.Count; i++)
            {
                // Do not parse loop pointers for vibrato
                // TODO: Check the length of the vibrato String, or even better, use separate lists for each channel!
                if (i >= song.VibratoIndex && i < song.VibratoIndex + song.VibratoLength)
                {
                    continue;
                }

                Byte b0 = song.SongData[i];

                // Bisqwit is awesome.
                // http://www.romhacking.net/forum/index.php?topic=16383.0
                // http://bisqwit.iki.fi/jutut/megamansource/mm2music.txt
                switch (b0)
                {
                    // Two-Byte encoding $00 n.  Song speed is set as n
                    // frames per tick.
                    case 0x00:
                    {
                        i += 1;
                        break;
                    }

                    // Two-Byte encoding $01 n. Adjusts vibrato parameters
                    // by n. Affects all following notes.
                    case 0x01:
                    {
                        i += 1;
                        break;
                    }

                    // Two-Byte encoding $02 n. Selects duty cycle settings.
                    // Valid values for n: $00,$40,$80,$C0. Only applicable
                    // for squarewave channels.
                    case 0x02:
                    {
                        i += 1;
                        break;
                    }

                    // Two-Byte encoding $03 n. Selects volume and envelope
                    // settings. Value n is passed directly to the soundchip;
                    // Affects all following notes.
                    case 0x03:
                    {
                        i += 1;
                        break;
                    }

                    // Four-Byte encoding $04 n w. Ends a loop. If n=0, loop is
                    // infinite. Otherwise the marked section plays for n+1 times.
                    // w is a 16-bit pointer to the beginning of the loop.
                    // Finite loops cannot be nested.
                    case 0x04:
                    {
                        Byte origLoopPtrSmall = song.SongData[i + 2];
                        Byte origLoopPtrLarge = song.SongData[i + 3];

                        // Get the loop destination pointer by converting the two bytes to a 16-bit Int32
                        Int32 origLoopOffset = origLoopPtrSmall + (origLoopPtrLarge * 256);
                        // Find index of destination of the loop with respect to the start of the song
                        Int32 relLoopOffset = origLoopOffset - song.OriginalStartAddress;
                        // Make new loop destination with respect to the new starting location of this song
                        Int32 newLoopOffset = songAddr + relLoopOffset;

                        // Put new hex bytes back into song data array
                        song.SongData[i + 2] = (Byte)(newLoopOffset % 256);
                        song.SongData[i + 3] = (Byte)(newLoopOffset / 256);

                        // Validation check when testing out newly ripped songs to make sure I didn't miss any loops
                        if (relLoopOffset > song.TotalLength || relLoopOffset < 0)
                        {
                            debug.AppendLine($"WARNING: Song {song.SongName} has external loop point.");
                        }

                        i += 3;

                        break;
                    }

                    // Two-Byte encoding $05 n. Sets note base to n. Value
                    // n is added to the note index for any notes
                    // (excluding pauses) played on this channel from now.
                    case 0x05:
                    {
                        i += 1;
                        break;
                    }

                    // One-Byte encoding $06. Dotted note: The next note will
                    // be played 50% longer than otherwise, i.e. 3/2 of its
                    // stated duration.
                    case 0x06:
                    {
                        break;
                    }

                    // Three-Byte encoding $07 n m. Sets volume curve settings.
                    // Byte n controls the attack, and Byte m controls the decay.
                    // Affects all following notes.
                    case 0x07:
                    {
                        i += 2;
                        break;
                    }

                    // Two-Byte encoding $08 n. Select vibrato entry n from the
                    // vibrato table referred to by the song header. Affects all
                    // following notes.
                    case 0x08:
                    {
                        i += 1;
                        break;
                    }

                    // One-Byte encoding $09. Ends the track. Can be omitted if
                    // the track ends in an infinite loop instead.
                    case 0x09:
                    {
                        break;
                    }

                    // One - Byte encoding $20 + n.Note delay(n = 0 - 7):
                    //      Delays the next note by n ticks, without affecting
                    //      its overall timing. (I.e.plays silence for the
                    //      first n ticks of the note.)
                    //
                    // One - Byte encoding $30.Triplet:
                    //      The next note will be played at 2 / 3 of its
                    //      stated duration.
                    //
                    // One - Byte encoding:
                    //      m * 0x20 + n.Play note(m = 2..7).If n = 0, plays pause.
                    //      Otherwise, plays note n(note base is added to n). The
                    //      lowest note that can be played is C - 0(n + base = 0).
                    //      Note or pause length is 2m−1 ticks, possibly altered by
                    //      the triplet / dotted modifiers.The next event will be
                    //      read only after this note/pause is done playing.
                    default:
                    {
                        break;
                    }
                }
            }
        }

        private void TestRebaseSongs(IEnumerable<ISong> in_Songs)
        {
            HashSet<FtXmlModule> ftmsDone = new(ReferenceEqualityComparer.Instance);

            foreach (ISong isong in in_Songs)
            {
                if (isong.Engine == ESoundEngine.Ft)
                {
                    FtSong song = (FtSong)isong;
                    FtXmlModule modInfo = song.ModuleInfo;

                    if (!ftmsDone.Add(modInfo))
                        continue;

                    try
                    {
                        FtmBinary mod = new(modInfo.TrackData, modInfo.StartAddress);
                        mod.Rebase(FtmBaseAddress);
                    }
                    catch
                    {
                        System.Diagnostics.Debug.Assert(false, $"ERROR: Rebase failed on FTM '{modInfo.Title}'");
                    }
                }
                else
                {
                    C2Song song = (C2Song)isong;
                    try
                    {
                        // Don't rebase it if it's already been rebased cause it was used in the seed
                        if (song.StartAddress == 0 || song.StartAddress == song.OriginalStartAddress)
                            RebaseC2Song(song, 0, 0x8001);
                    }
                    catch
                    {
                        Debug.Assert(false, $"ERROR: Rebase failed on '{song.SongName}'");
                    }
                }
            }
        }
    }
}
