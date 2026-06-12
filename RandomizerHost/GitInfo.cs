namespace RandomizerHost;

public static partial class GitInfo
{
    public static bool IsOfficialBuild { get; } = false;
    public static string Commit { get; } = "n/a";
    public static string Branch { get; } = "unknown";
    public static string Tag { get; } = "";
    public static bool IsDirty { get; }
}
