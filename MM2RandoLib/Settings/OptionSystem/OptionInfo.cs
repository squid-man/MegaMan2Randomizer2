using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace MM2RandoLib.Settings.Options;

/// <summary>
/// Information about an option used to connect it to the options system.
/// </summary>
public class OptionInfo
{
    /// <summary>
    /// The path through the options class hierarchy.
    /// </summary>
    public readonly MemberInfo[] Path;
    public readonly string PathString;
    public readonly string GroupPathString;

    public readonly IOption Value;

    public string? Description { get; }
    public string? Tooltip { get; }
    public bool IsCosmetic { get; }
    public bool CreateControl { get; }
    public bool SaveLoad { get; }

    public string Name => Path[^1].Name;
    public Type Type => Value.Type;

    public OptionInfo(IEnumerable<MemberInfo> path, IOption value, bool isCosmetic)
    {
        Path = path.ToArray();
        GroupPathString = string.Join(".", Path.Take(Path.Length - 1).Select(x => x.Name));
        PathString = string.Join(".", GroupPathString, Path[^1].Name);
        Value = value;
        IsCosmetic = isCosmetic;

        var fieldInfo = Path[^1];
        Description = fieldInfo
            .GetCustomAttribute<DescriptionAttribute>()?.Description;
        Tooltip = fieldInfo
            .GetCustomAttribute<TooltipAttribute>()?.Tooltip;
        CreateControl = fieldInfo
            .GetCustomAttribute<CreateControlAttribute>()?.CreateControl 
                ?? CreateControlAttribute.CreateControlDefault;
        SaveLoad = fieldInfo.GetCustomAttribute<SaveOptionAttribute>()?.Save 
            ?? SaveOptionAttribute.SaveDefault;
    }

    public override string ToString() => PathString;
}
