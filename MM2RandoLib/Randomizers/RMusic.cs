using Newtonsoft.Json;
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
using MM2Randomizer.Settings.Options;

namespace MM2Randomizer.Randomizers
{
    public enum ESoundEngine
    {
        C2,
        Ft,
    }

    public class DataInfo
    {
        public ESoundEngine Engine { get; private set; }

        // Primarily for debugging
        public string Title { get; private set; }

        public int Bank { get; set; }
        public int Address { get; set; }
        public int OriginalAddress { get; private set; }

        public byte[] Data { get; private set; }
        public int Size { get { return Data.Length; } }

        public DataInfo(ESoundEngine engine, string title, int addr, byte[] data)
        {
            Engine = engine;
            Title = title;
            Address = OriginalAddress = addr;
            Data = data;
        }
    }

    public interface ISong
    {
        ESoundEngine Engine { get; }

        //int SoundTableIndex { get; set; }

        ESoundTrackUsage Usage { get; }
        ISet<string> Uses { get; }

        DataInfo DataInfo { get; }
    }

    public class C2Song : ISong
    {
        public ESoundEngine Engine { get { return ESoundEngine.C2; } }

        //public int SoundTableIndex { get; set; }

        public ISet<string> Uses { get; set; }
        public ESoundTrackUsage Usage
        {
            get { return SoundTrackUsage.FromStrings(Uses); }
        }

        public DataInfo DataInfo { get; private set; }

        public C2SongGroupInfo? GroupInfo { get; private set; }
        public C2SongInfo Info { get; private set; }

        public readonly bool Enabled;
        public readonly string Title;
        public readonly string? Author;
        public readonly bool StreamingSafe;
        public readonly bool SwapSquareChans;

        public C2Song(C2SongGroupInfo? grpInfo, C2SongInfo songInfo)
        {
            GroupInfo = grpInfo;
            Info = songInfo;

            MusicInfoBase grpFac = grpInfo is not null ? grpInfo : songInfo;

            Enabled = songInfo.Enabled ?? grpFac.Enabled ?? C2SongInfo.EnabledDefault;
            Author = songInfo.Author ?? grpFac.Author;
            StreamingSafe = songInfo.StreamingSafe ?? grpFac.StreamingSafe ?? C2SongInfo.StreamingSafeDefault;
            SwapSquareChans = songInfo.SwapSquareChans ?? grpFac.SwapSquareChans ?? C2SongInfo.SwapSquareChansDefault;
            Uses = songInfo.Uses.Count > 0 ? songInfo.Uses
                : grpFac.Uses.Count > 0 ? grpFac.Uses
                : C2SongInfo.UsesDefault;

            Title = grpInfo is not null ? $"{grpInfo.Title} - " : "";
            Title = Title + songInfo.Title;

            DataInfo = new(ESoundEngine.C2,
                songInfo.Title,
                songInfo.StartAddr ?? songInfo.StartAddrDefault,
                songInfo.UncompressedData);
        }
    }

    public class FtSong : ISong
    {
        public ESoundEngine Engine { get { return ESoundEngine.Ft; } }

        public ISet<string> Uses { get; private set; }
        public ESoundTrackUsage Usage
        {
            get { return SoundTrackUsage.FromStrings(Uses); }
        }

        //public int SoundTableIndex { get; set; }

        public DataInfo DataInfo { get; private set; }

        public readonly FtModuleInfo ModuleInfo;
        public readonly FtSongInfo? Info;

        public readonly bool Enabled;
        public readonly int Number;
        public readonly string Title;
        public readonly string? Author;
        public readonly bool StreamingSafe;
        public readonly bool SwapSquareChans;

        public FtSong(FtModuleGroupInfo? grpInfo, FtModuleInfo modInfo, int songIdx, DataInfo dataInfo)
        {
            ModuleInfo = modInfo;
            Info = modInfo.Songs.Count > 0 ? modInfo.Songs[songIdx] : null;

            MusicInfoBase grpFac = grpInfo is not null ? grpInfo : modInfo,
                songFac = Info is not null ? Info : modInfo;

            Enabled = songFac.Enabled
                ?? modInfo.Enabled
                ?? grpFac.Enabled
                ?? FtSongInfo.EnabledDefault;
            Author = songFac.Author ?? modInfo.Author ?? grpFac.Author;
            StreamingSafe = songFac.StreamingSafe
                ?? modInfo.StreamingSafe
                ?? grpFac.StreamingSafe
                ?? FtSongInfo.StreamingSafeDefault;
            SwapSquareChans = songFac.SwapSquareChans
                ?? modInfo.SwapSquareChans
                ?? grpFac.SwapSquareChans
                ?? FtSongInfo.SwapSquareChansDefault;

            Uses = songFac.Uses.Count > 0 ? songFac.Uses
                : modInfo.Uses.Count > 0 ? modInfo.Uses
                : FtSongInfo.UsesDefault;

            DataInfo = dataInfo;

            Title = grpInfo is not null ? $"{grpInfo.Title} - " : "";
            Title = Title + modInfo.Title;

            Number = 0;

            if (Info is not null)
            {
                Number = Info.Number;
                Title = Title + $" - {Info.Title}";
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
        private const int C2SongTblOffs = 0x30a60;

        /// <summary>
        /// Offset of the mm2ft song map table in the ROM.
        /// </summary>
        private const int SongMapOffs = 0x30b60;

        /// <summary>
        /// Offset of the mm2ft song module address table in the ROM.
        /// </summary>
        private const int SongModAddrTblOffs = 0x30c60;

        /// <summary>
        /// Offset of the mm2ft boss song map table in the ROM.
        /// </summary>
        private const int BossSongMapOffs = 0x7f2f3;

        /// <summary>
        /// Length of the mm2ft boss song map table.
        /// </summary>
        private const int BossSongMapLen = 0x16;

        /// <summary>
        /// First unused song index available for boss music. The last is 0x7f, but there isn't any situation where that will be reached.
        /// </summary>
        private const int FirstBossMusicId = 0x43;

        /// <summary>
        /// The size of each ROM bank.
        /// </summary>
        private const int BankSize = 0x2000;

        /// <summary>
        /// The first address of banks for songs
        /// </summary>
        private const int BankBaseAddr = 0xa000;

        /// <summary>
        /// The first empty bank available for music.
        /// </summary>
        private const int FirstFreeBank = 0x20;

        /// <summary>
        /// The last empty bank available for music.
        /// </summary>
        private const int LastFreeBank = 0x3d;

        /// <summary>
        /// The default uses for tracks that have no uses specified on the song or module.
        /// </summary>
        private static HashSet<string> DefaultUses = new(StringComparer.InvariantCultureIgnoreCase){ "Stage", "Credits" };

        private StringBuilder debug = new();

        public RMusic()
        {
        }

        public override string ToString()
        {
            return debug.ToString();
        }

        public void Randomize(Patch in_Patch, RandomizationContext in_Context)
        {
            debug.AppendLine();
            debug.AppendLine("Random Music Module");
            debug.AppendLine("--------------------------------------------");

            Debug.Assert(in_Context.ActualizedCosmeticSettings is not null);

            this.ImportMusic(in_Patch, in_Context.Seed, in_Context.ActualizedCosmeticSettings.CosmeticOption.OmitUnsafeMusicTracks == BooleanOption.True);
        }

        public void ParseC2SongList(C2SongGroupInfo? grpInfo, IEnumerable<C2SongInfo> songInfos, bool safeOnly, List<C2Song> songs)
        {
            foreach (C2SongInfo songInfo in songInfos)
            {
                Debug.Assert(songInfo.Size <= BankSize, $"C2 {songInfo.Title} is larger than a bank");

                C2Song song = new(grpInfo, songInfo);
                if (song.Enabled && (song.StreamingSafe || !safeOnly))
                    songs.Add(song);
            }
        }

        public void ParseFtModuleList(FtModuleGroupInfo? grpInfo, IEnumerable<FtModuleInfo> modInfos, bool safeOnly, List<FtSong> songs)
        {
            foreach (FtModuleInfo modInfo in modInfos)
            {
                Debug.Assert(modInfo.Size <= BankSize, $"FTM {modInfo.Title} is larger than a bank");

                DataInfo dataInfo = new(
                    ESoundEngine.Ft, 
                    modInfo.Title,
                    modInfo.StartAddr ?? modInfo.StartAddrDefault, 
                    modInfo.UncompressedData);
                for (int i = 0; i < Math.Max(modInfo.Songs.Count, 1); i++)
                {
                    FtSong song = new(grpInfo, modInfo, i, dataInfo);
                    if (song.Enabled && (song.StreamingSafe || !safeOnly))
                        songs.Add(song);
                }
            }
        }

        public void ImportMusic(Patch patch, ISeed seed, bool safeOnly = false)
        {
            // Load C2 songs
            var c2Lib = JsonConvert.DeserializeObject<C2LibraryInfo>(
                Encoding.UTF8.GetString(Properties.Resources.SoundTrackConfiguration));
            Debug.Assert(c2Lib is not null);

            List<C2Song> c2Songs = new();
            ParseC2SongList(null, c2Lib.Single, safeOnly, c2Songs);
            foreach (var grpInfo in c2Lib.Groups)
                ParseC2SongList(grpInfo, grpInfo.Songs, safeOnly, c2Songs);

            // Load FT songs
            var ftLib = JsonConvert.DeserializeObject<FtLibraryInfo>(
                Encoding.UTF8.GetString(Properties.Resources.FtSoundTrackConfiguration));
            Debug.Assert(ftLib is not null);

            List<FtSong> ftSongs = new();
            ParseFtModuleList(null, ftLib.Single, safeOnly, ftSongs);
            foreach (var grpInfo in ftLib.Groups)
                ParseFtModuleList(grpInfo, grpInfo.Modules, safeOnly, ftSongs);

            debug.AppendLine($"{c2Songs.Count} C2 and {ftSongs.Count} FT songs loaded.");

            List<ISong> songs = new(c2Songs);
            songs.AddRange(ftSongs);

            // Create the usage type lists of songs
            Dictionary<string, List<int>> usesSongIdcs = new(StringComparer.InvariantCultureIgnoreCase);
            foreach (string name in Enum.GetNames(typeof(ESoundTrackUsage)))
                usesSongIdcs[name] = new List<int>();

            for (int songIdx = 0; songIdx < songs.Count; songIdx++)
            {
                ISong song = songs[songIdx];
                foreach (string useStr in song.Uses)
                    usesSongIdcs[useStr].Add(songIdx);
            }

            // Pick and import the songs
            Dictionary<EMusicID, int>? selSongMap = null;
            while (selSongMap is null)
            {
                selSongMap = SelectSongs(seed, usesSongIdcs);

                HashSet<int> selSongIdcs = new(selSongMap.Values);
                var banksSongs = PlaceSongsInBanks((from idx in selSongIdcs select songs[idx]).ToList());
                if (banksSongs is not null)
                    ImportSongs(patch, seed, banksSongs);
                else 
                    selSongMap = null;
            }

            // Update the sound tables
            foreach (EMusicID musicId in selSongMap.Keys)
            {
                int songIdx = selSongMap[musicId];
                int musicIdx = (int)musicId;
                int SongMapPtrOffs = SongMapOffs + musicIdx * 2;

                ISong isong = songs[songIdx];
                DataInfo dataInfo = isong.DataInfo;
                int tblBankIdx = dataInfo.Bank;
                int addrTblOffs, tblSongIdx;
                if (isong.Engine == ESoundEngine.C2)
                {
                    addrTblOffs = C2SongTblOffs;
                    tblSongIdx = musicIdx;

                    var song = (C2Song)isong;
                    debug.AppendLine($"{Enum.GetName(typeof(EMusicID), musicId)} song: {song.Title}, {dataInfo.OriginalAddress}");

                }
                else
                {
                    addrTblOffs = SongModAddrTblOffs;

                    FtSong song = (FtSong)isong;
                    tblBankIdx = ~dataInfo.Bank;
                    tblSongIdx = song.Number;

                    debug.AppendLine($"{Enum.GetName(typeof(EMusicID), musicId)} song: {song.Title}");
                }

                // Update the song address table
                patch.AddWord(addrTblOffs + musicIdx * 2, dataInfo.Address, $"Song {musicIdx} Pointer Offset");

                // And the track table
                patch.Add(SongMapPtrOffs++, (byte)tblBankIdx, $"Song {musicIdx} Bank Index");
                patch.Add(SongMapPtrOffs++, (byte)tblSongIdx, $"Song {musicIdx} Song Index");

                // For boss tracks, update the boss song table
                if (musicIdx >= FirstBossMusicId && musicIdx < FirstBossMusicId + BossSongMapLen)
                {
                    int bossIdx = musicIdx - FirstBossMusicId;
                    patch.Add(BossSongMapOffs + bossIdx, (byte)musicIdx, $"Boss {bossIdx} Song Index");
                }
            }

            bool testRebase = false;
            if (testRebase)
                TestRebaseSongs(songs);
        }

        private Dictionary<EMusicID, int> SelectSongs(ISeed seed, IReadOnlyDictionary<string, List<int>> usesSongIdcs)
        {
            EMusicID[] bossUsageMusicIds = (from idx in Enumerable.Range(FirstBossMusicId, BossSongMapLen) select (EMusicID)idx).ToArray();

            Dictionary<EMusicID, int> selSongMap = new();
            foreach (string usageStr in usesSongIdcs.Keys)
            {
                var usageSongs = usesSongIdcs[usageStr];
                if (usageSongs.Count == 0)
                    continue; // Nothing to randomize

                EMusicID[] usageMusicIds;
                if (!StringComparer.InvariantCultureIgnoreCase.Equals(usageStr, "Boss"))
                    usageMusicIds = UsesMusicIds[SoundTrackUsage.Values[usageStr]];
                else
                    usageMusicIds = bossUsageMusicIds;

                // Duplicate the candidate song list until there's enough for all the song slots
                var songIdcs = usageSongs;
                if (songIdcs.Count < usageMusicIds.Length)
                {
                    songIdcs = usageSongs.ToList();
                    while (songIdcs.Count < usageMusicIds.Length)
                        songIdcs.AddRange(usageSongs);
                }

                // Finally, make the list
                var cndIdcs = seed.Shuffle(songIdcs).ToList();
                for (int i = 0; i < usageMusicIds.Length; i++)
                    selSongMap[usageMusicIds[i]] = cndIdcs[i];
            }

            return selSongMap;
        }

        private Dictionary<int, List<ISong>>? PlaceSongsInBanks(IReadOnlyList<ISong> songs)
        {
            Dictionary<DataInfo, List<ISong>> datasSongs = new(ReferenceEqualityComparer.Instance);
            int numTracks = 0;
            foreach (ISong isong in songs)
            {
                var dataInfo = isong.DataInfo;
                List<ISong>? dataSongs;
                if (!datasSongs.TryGetValue(dataInfo, out dataSongs))
                    dataSongs = datasSongs[dataInfo] = new();

                dataSongs.Add(isong);

                numTracks++;
            }

            List<DataInfo> dataInfos = new(datasSongs.Keys);
            dataInfos.Sort((a, b) => -a.Size.CompareTo(b.Size));

            // Place the song data in each bank
            Dictionary<int, List<ISong>> banksSongs = new();
            int numData = 0;
            for (int bankIdx = FirstFreeBank; bankIdx <= LastFreeBank && dataInfos.Count > 0; bankIdx++)
            {
                List<ISong> bankSongs = banksSongs[bankIdx] = new();
                int bankOffs = 0;
                int listIdx = 0;
                while (listIdx < dataInfos.Count)
                {
                    DataInfo data = dataInfos[listIdx];
                    if (bankOffs + data.Size > BankSize)
                    {
                        listIdx++;
                        continue;
                    }

                    data.Bank = bankIdx;
                    data.Address = BankBaseAddr + bankOffs;

                    bankSongs.AddRange(datasSongs[data]);

                    dataInfos.RemoveAt(listIdx);
                    numData++;

                    bankOffs += data.Size;
                }
            }

            // DEBUG DEBUG
            if (dataInfos.Count > 0)
            {
                debug.AppendLine($"{numData} songs blocks selected. insufficient space.");

                return null;
            }

            debug.AppendLine($"{numTracks} songs selected.");

            return banksSongs;
        }

        private void ImportSongs(Patch patch, ISeed seed, Dictionary<int, List<ISong>> banksSongs)
        {
            // First pass: Build list of data to store and swap channels
            Dictionary<DataInfo, FtmBinary?> dataBins = new(ReferenceEqualityComparer.Instance);
            foreach (var bankSongs in banksSongs.Values)
            {
                foreach (var isong in bankSongs)
                {
                    DataInfo dataInfo = isong.DataInfo;
                    FtmBinary? bin = null;
                    if (!dataBins.TryGetValue(dataInfo, out bin))
                    {
                        if (dataInfo.Engine == ESoundEngine.Ft)
                            bin = new FtmBinary(dataInfo.Data, dataInfo.OriginalAddress);

                        dataBins[dataInfo] = bin;
                    }

                    if (bin is not null)
                    {
                        var song = (FtSong)isong;
                        if (song.SwapSquareChans)
                            bin.SwapSquareChans(song.Number);
                    }
                }
            }

            // Second pass: Import it
            foreach (var dataInfoAndBin in dataBins)
            {
                DataInfo dataInfo = dataInfoAndBin.Key;
                FtmBinary? ftmBin = dataInfoAndBin.Value;

                if (ftmBin is not null)
                    ftmBin.Rebase(dataInfo.Address);
                else
                    RebaseC2Song(dataInfo);

                int romOffs = dataInfo.Bank * BankSize + (dataInfo.Address % BankSize) + 0x10;

                string songType = (Enum.GetName(dataInfo.Engine) ?? "ERROR").ToUpper();
                patch.Add(romOffs, dataInfo.Data, $"{songType} {dataInfo.Title}");
            }
        }

        private void RebaseC2Song(DataInfo dataInfo)
        {
            var data = dataInfo.Data;
            BinaryBuffer buff = new(data);

            // mm2ft will break if the priority for music is not $f
            buff.Write((byte)0xf);

            // Header: Calculate 4 channel addresses and instrument table address
            List<int> chansOffs = new();
            for (int chanIdx = 0; chanIdx < 5; chanIdx++)
            {
                int origAddr = buff.ReadUInt16LE(false);
                if (origAddr == 0 || origAddr == 0xffff)
                {
                    if (chanIdx == 4)
                        debug.AppendLine($"WARNING: Song {dataInfo.Title} has a null instrument table pointer.");

                    chansOffs.Add(-1);
                    buff.Seek(2, System.IO.SeekOrigin.Current);

                    continue;
                }

                int offs = origAddr - dataInfo.OriginalAddress;
                if (offs >= dataInfo.Size || offs < 0)
                {
                    debug.AppendLine($"WARNING: Song {dataInfo.Title} channel {chanIdx} points to an external location.");
                    offs = -1;
                }

                buff.WriteLE(checked((UInt16)(offs + dataInfo.Address)));
                chansOffs.Add(offs);
            }

            // Song Data: Traverse stream and change loop pointers
            int vibStart = chansOffs[4];
            int vibEnd = -1;
            if (vibStart >= 0)
            {
                vibEnd = data.Length;
                for (int i = 0; i < 4; i++)
                {
                    int chanOffs = chansOffs[i];
                    if (chanOffs >= vibStart)
                        vibEnd = Math.Min(chanOffs, vibEnd);
                }
            }

            for (int i = buff.Position; i < data.Length; i++)
            {
                // Do not parse loop pointers for vibrato
                // TODO: Check the length of the vibrato string, or even better, use separate lists for each channel!
                if (vibStart >= 0 && i >= vibStart && i < vibEnd)
                {
                    continue;
                }

                byte b0 = data[i];

                // Bisqwit is awesome.
                // http://www.romhacking.net/forum/index.php?topic=16383.0
                // http://bisqwit.iki.fi/jutut/megamansource/mm2music.txt
                switch (b0)
                {
                    // Two-byte encoding $00 n.  Song speed is set as n
                    // frames per tick.
                    case 0x00:
                    {
                        i += 1;
                        break;
                    }

                    // Two-byte encoding $01 n. Adjusts vibrato parameters
                    // by n. Affects all following notes.
                    case 0x01:
                    {
                        i += 1;
                        break;
                    }

                    // Two-byte encoding $02 n. Selects duty cycle settings.
                    // Valid values for n: $00,$40,$80,$C0. Only applicable
                    // for squarewave channels.
                    case 0x02:
                    {
                        i += 1;
                        break;
                    }

                    // Two-byte encoding $03 n. Selects volume and envelope
                    // settings. Value n is passed directly to the soundchip;
                    // Affects all following notes.
                    case 0x03:
                    {
                        i += 1;
                        break;
                    }

                    // Four-byte encoding $04 n w. Ends a loop. If n=0, loop is
                    // infinite. Otherwise the marked section plays for n+1 times.
                    // w is a 16-bit pointer to the beginning of the loop.
                    // Finite loops cannot be nested.
                    case 0x04:
                    {
                        byte origLoopPtrSmall = data[i + 2];
                        byte origLoopPtrLarge = data[i + 3];

                        // Get the loop destination pointer by converting the two bytes to a 16-bit int
                        int origLoopOffset = origLoopPtrSmall + (origLoopPtrLarge * 256);
                        // Find index of destination of the loop with respect to the start of the song
                        int relLoopOffset = origLoopOffset - dataInfo.OriginalAddress;
                        // Make new loop destination with respect to the new starting location of this song
                        int newLoopOffset = dataInfo.Address + relLoopOffset;

                        // Put new hex bytes back into song data array
                        data[i + 2] = (byte)(newLoopOffset % 256);
                        data[i + 3] = (byte)(newLoopOffset / 256);

                        // Validation check when testing out newly ripped songs to make sure I didn't miss any loops
                        if (relLoopOffset > data.Length || relLoopOffset < 0)
                        {
                            debug.AppendLine($"WARNING: Song {dataInfo.Title} has external loop point.");
                        }

                        i += 3;

                        break;
                    }

                    // Two-byte encoding $05 n. Sets note base to n. Value
                    // n is added to the note index for any notes
                    // (excluding pauses) played on this channel from now.
                    case 0x05:
                    {
                        i += 1;
                        break;
                    }

                    // One-byte encoding $06. Dotted note: The next note will
                    // be played 50% longer than otherwise, i.e. 3/2 of its
                    // stated duration.
                    case 0x06:
                    {
                        break;
                    }

                    // Three-byte encoding $07 n m. Sets volume curve settings.
                    // byte n controls the attack, and byte m controls the decay.
                    // Affects all following notes.
                    case 0x07:
                    {
                        i += 2;
                        break;
                    }

                    // Two-byte encoding $08 n. Select vibrato entry n from the
                    // vibrato table referred to by the song header. Affects all
                    // following notes.
                    case 0x08:
                    {
                        i += 1;
                        break;
                    }

                    // One-byte encoding $09. Ends the track. Can be omitted if
                    // the track ends in an infinite loop instead.
                    case 0x09:
                    {
                        break;
                    }

                    // One - byte encoding $20 + n.Note delay(n = 0 - 7):
                    //      Delays the next note by n ticks, without affecting
                    //      its overall timing. (I.e.plays silence for the
                    //      first n ticks of the note.)
                    //
                    // One - byte encoding $30.Triplet:
                    //      The next note will be played at 2 / 3 of its
                    //      stated duration.
                    //
                    // One - byte encoding:
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
            HashSet<FtModuleInfo> ftmsDone = new(ReferenceEqualityComparer.Instance);

            foreach (ISong isong in in_Songs)
            {
                DataInfo dataInfo = isong.DataInfo;
                if (dataInfo.Address != dataInfo.OriginalAddress)
                    // It was used. No need to rebase it again.
                    continue;

                try
                {
                    if (isong.Engine == ESoundEngine.Ft)
                    {
                        var song = (FtSong)isong;
                        FtModuleInfo modInfo = song.ModuleInfo;
                        if (!ftmsDone.Add(song.ModuleInfo))
                            continue;

                        FtmBinary mod = new(modInfo.UncompressedData.ToArray(), isong.DataInfo.OriginalAddress);
                        mod.Rebase(BankBaseAddr + 1);
                    }
                    else
                    {
                        dataInfo.Address = 0x8001;
                        RebaseC2Song(dataInfo);
                    }
                }
                catch
                {
                    Debug.Assert(false, $"ERROR: Rebase failed on '{dataInfo.Title}'");
                }
            }
        }
    }
}
