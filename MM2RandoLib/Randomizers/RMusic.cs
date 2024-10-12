using FtRandoLib.Importer;
using FtRandoLib.Library;
using FtRandoLib.Utility;
using MM2Randomizer.Enums;
using MM2Randomizer.Extensions;
using MM2Randomizer.Patcher;
using MM2Randomizer.Random;
using MM2Randomizer.Settings.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using EUsage = MM2Randomizer.Enums.ESoundTrackUsage;
using Module = FtRandoLib.Importer.Module;

namespace MM2Randomizer.Randomizers;

[JsonObject]
public class C2SongInfo : MusicFileInfo
{
}

public class C2SongGroupInfo : GroupInfo<C2SongInfo>
{
}

[JsonObject]
public class C2LibraryInfo : LibraryInfo<C2SongInfo, C2SongGroupInfo>
{
}

enum EBossSongMapIndex
{
	HeatMan = 0,
	AirMan,
	WoodMan,
	BubbleMan,
	QuickMan,
	FlashMan,
	MetalMan,
	CrashMan,
	Wily1,
	Wily2,
	Wily3,
	Wily4,
	Wily5,
	Wily6,
	HeatManRefight,
	AirManRefight,
	WoodManRefight,
	BubbleManRefight,
	QuickManRefight,
	FlashManRefight,
	MetalManRefight,
	CrashManRefight,
}

public class C2Song : SongBase
{
    public C2Song(
        C2SongGroupInfo? grpInfo, 
        C2SongInfo songInfo,
        int defaultStartAddr,
        IstringSet defaultUses)
        : base(
              "native",
              grpInfo,
              songInfo,
              null,
              defaultUses)
    {
        string title = songInfo.Title;
        if (grpInfo is not null)
            title = $"{grpInfo.Title} - " + title;
        Module = new("native",
            title,
            songInfo.StartAddr ?? defaultStartAddr,
            songInfo.UncompressedData);
    }
}

public class RomAccessAdapter : IRomAccess
{
    Patch Patch;

    public RomAccessAdapter(Patch patch) => Patch = patch;

    public byte[] Rom => throw new NotImplementedException();

    public void Write(int offset, byte data, string comment = "") => Patch.Add(offset, data, comment);
    public void Write(int offset, IReadOnlyList<byte> data, string comment = "") => Patch.Add(offset, data, comment);
}

public class ShufflerAdapter : IShuffler
{
    ISeed Seed;

    public ShufflerAdapter(ISeed seed) => Seed = seed;

    public IList<T> Shuffle<T>(IReadOnlyList<T> items) => Seed.Shuffle(items);
}

public class LoggerAdapter: Logger
{
    StringBuilder sb;

    public LoggerAdapter(StringBuilder sb) => this.sb = sb;

    protected override void InternalWrite(string? message) => sb.Append(message);
    protected override void InternalWriteLine(string? message) => sb.AppendLine(message);
}

public class ImportedC2ModuleInfo : ImportedModuleInfo
{
    public ImportedC2ModuleInfo(Module mod)
        : base(mod)
    {
    }

    public override byte[] GetData(int address, int primarySquareChan)
    {
        Debug.Assert(Module.IsEngine("native"));

        byte[] buffer = Module.Data.ToArray();
        RebaseSong(buffer, address, primarySquareChan != 0);

        return buffer;
    }

    void RebaseSong(
        byte[] data,
        int address, 
        bool swapSquareChans)
    {
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
                    Log.WriteLine($"WARNING: Song {Module.Title} has a null instrument table pointer.");

                chansOffs.Add(-1);
                buff.Seek(2, System.IO.SeekOrigin.Current);

                continue;
            }

            int offs = origAddr - Module.Address;
            if (offs >= data.Length || offs < 0)
            {
                Log.WriteLine($"WARNING: Song {Module.Title} channel {chanIdx} points to an external location.");
                offs = -1;
            }

            buff.WriteLE(checked((UInt16)(offs + address)));
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
                        int relLoopOffset = origLoopOffset - Module.Address;
                        // Make new loop destination with respect to the new starting location of this song
                        int newLoopOffset = address + relLoopOffset;

                        // Put new hex bytes back into song data array
                        data[i + 2] = (byte)(newLoopOffset % 256);
                        data[i + 3] = (byte)(newLoopOffset / 256);

                        // Validation check when testing out newly ripped songs to make sure I didn't miss any loops
                        if (relLoopOffset > data.Length || relLoopOffset < 0)
                        {
                            Log.WriteLine($"WARNING: Song {Module.Title} has external loop point.");
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
}

public class Mm2Importer : Importer
{
    protected override int BankSize => 0x2000;

    static readonly BankLayout CommonBankLayout = new(0xa000, 0x2000);

    protected override List<int> FreeBanks { get; } 
        = new(InRange(0x20, 0x3d));

    protected override int PrimarySquareChan => 0;
    protected override IstringSet Uses { get; } 
        = new(Enum.GetNames<EUsage>());
    protected override IstringSet DefaultUses { get; } 
        = new(){ "Stage", "Credits" };
    protected override bool DefaultStreamingSafe => true;

    protected override int SongMapOffs => 0x30b60;
    protected override int SongModAddrTblOffs => 0x30c60;

    protected override HashSet<int> BuiltinSongIdcs { get; } 
        = new(from int i in Enum.GetValues<EMusicID>() where i < 0x80 select i);
    protected override List<int> FreeSongIdcs { get; } 
        = new(InRange(0x43, 0x7f));
    protected override int NumSongs => 0x80;

    protected override IReadOnlyDictionary<string, SongMapInfo> SongMapInfos { get; } 
        = new SongMapInfo[] 
        {
            new SongMapInfo(BossSongMapName, BossSongMapOffs,
                Enum.GetValues<EBossSongMapIndex>().Length),
        }.ToDictionary(info => info.Name);

    protected override int NumFtChannels => 5;
    protected override int DefaultFtStartAddr => 0;
    protected override int DefaultFtPrimarySquareChan => 0;

    const int DefaultC2StartAddr = 0x8000;

    /// <summary>
    /// Offset of the Capcom 2 song address table in the ROM.
    /// </summary>
    const int C2SongTblOffs = 0x30a60;

    /// <summary>
    /// Offset of the mm2ft boss song map table in the ROM.
    /// </summary>
    const int BossSongMapOffs = 0x7f2f3;

    public const string BossSongMapName = "BossSongMap";

    public Mm2Importer(Patch patch)
        : base(
              new IstringDictionary<BankLayout>()
                  {
                      { "native", CommonBankLayout },
                      { "ft", CommonBankLayout },
                  },
              new RomAccessAdapter(patch))
    {
    }

    IEnumerable<C2Song> LoadC2Songs(
        C2SongGroupInfo? grpInfo, 
        IEnumerable<C2SongInfo> songInfos,
        LibraryParserOptions? opts = null)
    {
        opts = opts ?? DefaultParserOptions;
        foreach (C2SongInfo songInfo in songInfos)
        {
            Debug.Assert(songInfo.Size <= BankSize, $"C2 {songInfo.Title} is larger than a bank");

            C2Song song = new(
                grpInfo, 
                songInfo,
                DefaultC2StartAddr,
                DefaultUses);
            if ((song.Enabled || !opts.EnabledOnly)
                && (song.StreamingSafe || !opts.SafeOnly))
                yield return song;
        }
    }

    public IEnumerable<C2Song> LoadC2JsonLibrarySongs(
        LibraryParserOptions? opts = null)
    {
        string jsonData = ReadUtf8Resource(
            "Resources.SoundTrackConfiguration.json5");
        return LoadJsonLibrarySongs<C2Song, C2SongInfo, C2SongGroupInfo>(jsonData, LoadC2Songs, opts);
    }

    public IEnumerable<FtSong> LoadFtJsonLibrarySongs(
        LibraryParserOptions? opts = null)
    {
        string jsonData = ReadUtf8Resource(
            "Resources.FtSoundTrackConfiguration.json5");
        return LoadFtJsonLibrarySongs(jsonData, opts);
    }

    string ReadUtf8Resource(string name)
        => Assembly.GetExecutingAssembly().LoadUtf8Resource(name);

    protected override ImportedModuleInfo CreateImportedModuleInfo(Module mod)
    {
        if (mod.IsEngine("native"))
            return new ImportedC2ModuleInfo(mod);
        else
            return base.CreateImportedModuleInfo(mod);
    }

    protected override void WritePrimarySongMap(
        IReadOnlyDictionary<int, ISong?> songs,
        Dictionary<Module, ImportedModuleInfo> modInfos)
    {
        base.WritePrimarySongMap(songs, modInfos);

        HashSet<int> doneIdcs = new();
        byte[] writeBuff = new byte[2];
        BinaryBuffer buff = new(writeBuff);
        foreach (var (songIdx, song) in songs)
        {
            if (song is null
                || song.Module is null
                || !song.Module.IsEngine("native"))
                continue;

            var modInfo = modInfos[song.Module];
            int romOffs = C2SongTblOffs + songIdx * 2;
            buff.WriteLE(checked((UInt16)modInfo.Address), false);
            RomWriter.Write(
                romOffs,
                writeBuff, 
                $"C2 Song Address Map {songIdx:x}");
        }
    }
}

public class RMusic : IRandomizer
{
    /// <summary>
    /// Map of which game songs belong to which music uses.
    /// </summary>
    private Dictionary<EUsage, EMusicID[]> UsesMusicIds = new Dictionary<EUsage, EMusicID[]>
    {
        { EUsage.Intro, new EMusicID[]{EMusicID.Intro} },
        { EUsage.Title, new EMusicID[]{EMusicID.Title} },
        { EUsage.StageSelect, new EMusicID[]{EMusicID.StageSelect} },
        { EUsage.Stage, new EMusicID[]{EMusicID.Flash, EMusicID.Wood, EMusicID.Crash, EMusicID.Heat, EMusicID.Air, EMusicID.Metal, EMusicID.Quick, EMusicID.Bubble, EMusicID.Wily1, EMusicID.Wily2, EMusicID.Wily3, EMusicID.Wily4, EMusicID.Wily6 } },
        { EUsage.Boss, new EMusicID[]{EMusicID.Boss} },
        { EUsage.Refights, new EMusicID[]{EMusicID.Wily5} },
        { EUsage.Ending, new EMusicID[]{EMusicID.Ending} },
        { EUsage.Credits, new EMusicID[]{EMusicID.Credits} },
    };

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

        LoggerAdapter loggerAdapter = new(debug);
        Log.Push(loggerAdapter);
        try
        {
            this.ImportMusic(in_Patch, in_Context.Seed, in_Context.Settings.CosmeticOptions.OmitUnsafeMusicTracks.Value);
        }
        finally
        {
            Log.Loggers.Remove(loggerAdapter);
        }
    }

    public void ImportMusic(
        Patch patch, 
        ISeed seed, 
        bool safeOnly = false)
    {
        Mm2Importer imptr = new(patch);
        ShufflerAdapter shuffler = new(seed);
        imptr.DefaultParserOptions.SafeOnly = safeOnly;

        List<C2Song> c2Songs = new(imptr.LoadC2JsonLibrarySongs());
        List<FtSong> ftSongs = new(imptr.LoadFtJsonLibrarySongs());

        Log.WriteLine($"{c2Songs.Count} C2 and {ftSongs.Count} FT songs loaded.");
        Log.WriteLine();

        List<ISong> songs = new(Enumerable.Concat<ISong>(c2Songs, ftSongs));
        var usesSongs = imptr.SplitSongsByUsage<EUsage>(songs);

        // Pick and import the songs
        Dictionary<EMusicID, ISong?> songMap = new();
        Dictionary<EBossSongMapIndex, ISong?> bossSongMap = new();
        int triesLeft = 8;
        while (triesLeft > 0)
        {
            Log.WriteLine("Importing songs...");

            SelectSongs(
                imptr,
                shuffler, 
                usesSongs,
                out songMap,
                out bossSongMap);

            HashSet<int> freeBanks;
            try
            {
                imptr.Import(
                    songMap.ToDictionary(idSong => (int)idSong.Key, idSong => idSong.Value),
                    new IstringDictionary<IReadOnlyDictionary<int, ISong?>>() { { Mm2Importer.BossSongMapName, bossSongMap.ToDictionary(kvp => (int)kvp.Key, kvp => kvp.Value) } },
                    out freeBanks);

                break;
            }
            catch (RomFullException)
            {
                Log.WriteLine("WARNING: Attempt to select songs overflowed ROM space");

                triesLeft--;
            }

            Log.WriteLine();
        }

        Debug.Assert(triesLeft > 0); ////

        Log.WriteLine($"{songMap.Count + bossSongMap.Count} songs imported in total");
        Log.WriteLine();

#if DEBUG
        LibraryParserOptions opts = new() { EnabledOnly = false, SafeOnly = false };
        songs.Clear();
        songs.AddRange(imptr.LoadC2JsonLibrarySongs(opts));
        songs.AddRange(imptr.LoadFtJsonLibrarySongs(opts));

        imptr.TestRebase(songs);
#endif

        return;
    }

    private void SelectSongs(
        Mm2Importer imptr,
        IShuffler shuffler,
        IReadOnlyDictionary<EUsage, List<ISong>> usesSongs,
        out Dictionary<EMusicID, ISong?> songMap,
        out Dictionary<EBossSongMapIndex, ISong?> bossSongMap)
    {
        songMap = new();
        bossSongMap = new();

        Dictionary<EUsage, int> numUsageSongs = UsesMusicIds.ToDictionary(
            kv => kv.Key, kv => kv.Value.Length);
        numUsageSongs[EUsage.Boss] = Enum.GetValues<EBossSongMapIndex>().Length;

        var selUsesSongs = imptr.SelectUsesSongs<EUsage>(
            usesSongs, numUsageSongs, shuffler);

        foreach (var (usage, usageSongs) in selUsesSongs)
        {
            if (usage == EUsage.Boss)
                continue;

            foreach (var (musicId, song) in Enumerable.Zip(
                UsesMusicIds[usage], usageSongs))
            {
                songMap[musicId] = song;

                Log.WriteLine($"{musicId.ToString()}: {song.Title}");
            }
        }

        foreach (var (mapIdx, song) in Enumerable.Zip(
            Enum.GetValues<EBossSongMapIndex>(),
            selUsesSongs[EUsage.Boss]))
        {
            bossSongMap[mapIdx] = song;

            Log.WriteLine($"{mapIdx.ToString()} Boss: {song.Title}");
        }

        return;
    }
}
