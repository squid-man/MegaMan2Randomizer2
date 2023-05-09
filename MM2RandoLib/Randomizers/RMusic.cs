using System;
using System.Collections.Generic;
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
    public class Song
    {
        public Song(String in_SongName, Int32 in_OriginalStartAddress, String in_SongBytesStr, SoundTrack? in_SoundTrack = null)
        {
            this.SoundTrack = in_SoundTrack;
            this.OriginalStartAddress = in_OriginalStartAddress;
            this.SongName = in_SongName;

            // Parse song bytes from hex String
            List<Byte> songBytes = new List<Byte>();
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
        public Int32 SongBank { get; set; }
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
    }


    public class RMusic : IRandomizer
    {
        /// <summary>
        /// Map of which game songs belong to which music uses.
        /// </summary>
        private Dictionary<SoundTrackUsage, EMusicID[]> UsesMusicIds = new Dictionary<SoundTrackUsage, EMusicID[]>
        {
            { SoundTrackUsage.Intro, new EMusicID[]{EMusicID.Intro} },
            { SoundTrackUsage.Title, new EMusicID[]{EMusicID.Title} },
            { SoundTrackUsage.StageSelect, new EMusicID[]{EMusicID.StageSelect} },
            { SoundTrackUsage.Stage, new EMusicID[]{EMusicID.Flash, EMusicID.Wood, EMusicID.Crash, EMusicID.Heat, EMusicID.Air, EMusicID.Metal, EMusicID.Quick, EMusicID.Bubble, EMusicID.Wily1, EMusicID.Wily2, EMusicID.Wily3, EMusicID.Wily4, EMusicID.Wily5, EMusicID.Wily6 } },
            { SoundTrackUsage.Boss, new EMusicID[]{EMusicID.Boss} },
            { SoundTrackUsage.Ending, new EMusicID[]{EMusicID.Ending} },
            { SoundTrackUsage.Credits, new EMusicID[]{EMusicID.Credits} },
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

        private StringBuilder debug = new StringBuilder();

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
            var songs =
                (from soundTrack in soundTrackSet
                 where soundTrack.Enabled
                 select new Song(soundTrack.Title, Int32.Parse(soundTrack.StartAddress, NumberStyles.HexNumber), soundTrack.TrackData, soundTrack)).ToList();

            debug.AppendLine($"{songs.Count} stage songs loaded.");

            // Create the usage type lists of songs
            var usesSongIdcs = new Dictionary<String, List<Int32>>();
            foreach (String name in Enum.GetNames(typeof(SoundTrackUsage)))
                usesSongIdcs[name] = new List<Int32>();

            for (Int32 songIdx = 0; songIdx < songs.Count; songIdx++)
            {
                SoundTrack? soundTrack = songs[songIdx].SoundTrack;
                if (soundTrack is null)
                    continue;

                foreach (String useStr in soundTrack.Uses)
                    usesSongIdcs[useStr].Add(songIdx);
            }

            // Pick and import the songs
            Dictionary<EMusicID, Int32>? selSongMap = null;
            while (selSongMap is null)
            {
                selSongMap = SelectSongs(in_Seed, songs, usesSongIdcs);

                var selSongIdcs = new HashSet<int>(selSongMap.Values);
                var banksSongs = PlaceSongsInBanks((from idx in selSongIdcs select songs[idx]).ToList());
                if (banksSongs is not null)
                    ImportSongs(in_Patch, in_Seed, banksSongs);
                else
                    selSongMap = null;
            }

            // For boss tracks, update the boss song table
            foreach (EMusicID musicId in selSongMap.Keys)
            {
                Int32 songIdx = selSongMap[musicId];
                Song song = songs[songIdx];
                Int32 musicIdx = (Int32)musicId;
                Int32 TblPtrOffset = C2SongTblOffs + musicIdx * 2;
                Int32 SongMapPtrOffs = SongMapOffs + musicIdx * 2;

                // Update the song address and map tables
                in_Patch.AddWord(TblPtrOffset, song.StartAddress, $"Song {musicIdx} Pointer Offset");
                debug.AppendLine($"{Enum.GetName(typeof(EMusicID), musicId)} song: {song.SongName}, {song.OriginalStartAddress}");

                in_Patch.Add(SongMapPtrOffs++, (byte)song.SongBank, $"Song {musicIdx} Bank Index");
                in_Patch.Add(SongMapPtrOffs++, (byte)musicIdx, $"Song {musicIdx} Song Index");

                if ((Int32)musicId >= FirstBossMusicId && (Int32)musicId < FirstBossMusicId + BossSongMapLen)
                {
                    Int32 bossIdx = (Int32)musicId - FirstBossMusicId;
                    in_Patch.Add(BossSongMapOffs + bossIdx, (byte)musicIdx, $"Boss {bossIdx} Song Index");
                }
            }

            Boolean testRebase = false;
            if (testRebase)
                TestRebaseSongs(songs);
        }

        private Dictionary<EMusicID, Int32> SelectSongs(ISeed in_Seed, List<Song> in_Songs, Dictionary<String, List<Int32>> in_UsesSongIdcs)
        {
            var selSongMap = new Dictionary<EMusicID, int>();

            foreach (String usageStr in in_UsesSongIdcs.Keys)
            {
                var usageSongs = in_UsesSongIdcs[usageStr];
                if (usageSongs.Count == 0)
                    continue; // Nothing to randomize

                EMusicID[]? usageMusicIds;
                if (usageStr != "Boss")
                    usageMusicIds = UsesMusicIds[Enum.Parse<SoundTrackUsage>(usageStr)];
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

        private Dictionary<int, List<Song>>? PlaceSongsInBanks(List<Song> in_Songs)
        {
            // Allocate space for all the songs in the free banks
            Int32 numTracks = in_Songs.Count;
            var banksSongs = new Dictionary<int, List<Song>>();
            Int32 songsLeft = numTracks;
            var songs = new List<Song?>(in_Songs);
            songs.Sort((a, b) => -a.TotalLength.CompareTo(b.TotalLength));

            for (Int32 bankIdx = FirstFreeBank; bankIdx <= LastFreeBank && songsLeft > 0; bankIdx++)
            {
                banksSongs[bankIdx] = new List<Song>();

                Int32 sizeLeft = BankSize;
                for (Int32 listIdx = 0; listIdx < numTracks && songsLeft > 0; listIdx++)
                {
                    Song? song = songs[listIdx];
                    if (song is not null && song.TotalLength <= sizeLeft)
                    {
                        banksSongs[bankIdx].Add(song);
                        sizeLeft -= song.TotalLength;

                        songs[listIdx] = null;
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

        private void ImportSongs(Patch in_Patch, ISeed in_Seed, Dictionary<int, List<Song>> in_BanksSongs)
        {
            // Rebase and write the songs
            foreach (var bankAndSongs in in_BanksSongs)
            {
                Int32 bankIdx = bankAndSongs.Key;
                var bankSongs = bankAndSongs.Value;
                Int32 songStartRom = bankIdx * BankSize + 0x10;

                foreach (Song song in bankSongs)
                {
                    Int32 songAddr = (songStartRom - 0x10) % BankSize + 0xa000;

                    RebaseC2Song(song, bankIdx, songAddr);

                    // Write song header and data
                    songStartRom = in_Patch.Add(songStartRom, song.SongHeader, $"Song Header for {song.SongName}");
                    songStartRom = in_Patch.Add(songStartRom, song.SongData, $"Song Data for {song.SongName}");
                }
            }
        }

        private void RebaseC2Song(Song song, Int32 songBank, Int32 songAddr)
        {
            song.SongBank = songBank;
            song.StartAddress = songAddr;

            // Header: Calculate 4 channel addresses and vibrato address
            Byte origChannel1ByteSmall = song.SongHeader[1];
            Byte origChannel1ByteLarge = song.SongHeader[2];
            Int32 origChannel1Offset = origChannel1ByteSmall + (origChannel1ByteLarge * 256);
            Int32 relChannel1Offset = origChannel1Offset - song.OriginalStartAddress;
            Int32 newChannel1Offset = songAddr + relChannel1Offset;
            song.SongHeader[1] = (Byte)(newChannel1Offset % 256);
            song.SongHeader[2] = (Byte)(newChannel1Offset / 256);

            Byte origChannel2ByteSmall = song.SongHeader[3];
            Byte origChannel2ByteLarge = song.SongHeader[4];
            Int32 origChannel2Offset = origChannel2ByteSmall + (origChannel2ByteLarge * 256);
            Int32 relChannel2Offset = origChannel2Offset - song.OriginalStartAddress;
            Int32 newChannel2Offset = songAddr + relChannel2Offset;
            song.SongHeader[3] = (Byte)(newChannel2Offset % 256);
            song.SongHeader[4] = (Byte)(newChannel2Offset / 256);

            Byte origChannel3ByteSmall = song.SongHeader[5];
            Byte origChannel3ByteLarge = song.SongHeader[6];
            Int32 origChannel3Offset = origChannel3ByteSmall + (origChannel3ByteLarge * 256);
            Int32 relChannel3Offset = origChannel3Offset - song.OriginalStartAddress;
            Int32 newChannel3Offset = songAddr + relChannel3Offset;
            song.SongHeader[5] = (Byte)(newChannel3Offset % 256);
            song.SongHeader[6] = (Byte)(newChannel3Offset / 256);

            Byte origChannel4ByteSmall = song.SongHeader[7];
            Byte origChannel4ByteLarge = song.SongHeader[8];

            if (origChannel4ByteSmall > 0 || origChannel4ByteLarge > 0)
            {
                Int32 origChannel4Offset = origChannel4ByteSmall + (origChannel4ByteLarge * 256);
                Int32 relChannel4Offset = origChannel4Offset - song.OriginalStartAddress;
                Int32 newChannel4Offset = songAddr + relChannel4Offset;
                song.SongHeader[7] = (Byte)(newChannel4Offset % 256);
                song.SongHeader[8] = (Byte)(newChannel4Offset / 256);

                if (relChannel4Offset > song.TotalLength || relChannel4Offset < 0)
                {
                    debug.AppendLine($"WARNING: Song {song.SongName} channel 4 points to a shared location.");
                }
            }

            Byte origVibratoByteSmall = song.SongHeader[9];
            Byte origVibratoByteLarge = song.SongHeader[10];
            Int32 origVibratoOffset = origVibratoByteSmall + (origVibratoByteLarge * 256);
            Int32 relVibratoOffset = origVibratoOffset - song.OriginalStartAddress;
            Int32 newVibratoOffset = songAddr + relVibratoOffset;
            song.SongHeader[9] = (Byte)(newVibratoOffset % 256);
            song.SongHeader[10] = (Byte)(newVibratoOffset / 256);

            if (relChannel1Offset > song.TotalLength || relChannel1Offset < 0)
            {
                debug.AppendLine($"WARNING: Song {song.SongName} channel 1 points to a shared location.");
            }

            if (relChannel2Offset > song.TotalLength || relChannel2Offset < 0)
            {
                debug.AppendLine($"WARNING: Song {song.SongName} channel 2 points to a shared location.");
            }

            if (relChannel3Offset > song.TotalLength || relChannel3Offset < 0)
            {
                debug.AppendLine($"WARNING: Song {song.SongName} channel 3 points to a shared location.");
            }

            if (relVibratoOffset > song.TotalLength || relVibratoOffset < 0)
            {
                debug.AppendLine($"WARNING: Song {song.SongName} vibrato points to a shared location.");
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

        private void TestRebaseSongs(List<Song> in_Songs)
        {
            foreach (Song song in in_Songs)
            {
                try 
                {
                    RebaseC2Song(song, 0, 0x8001);
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.Assert(false, $"ERROR: Rebase failed on '{song.SongName}'");
                }
            }
        }
    }
}
