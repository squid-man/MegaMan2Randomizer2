using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2RandoLib.Settings.Options;

/// <summary>
/// A preset for a single option.
/// </summary>
public class OptionPreset
{
    public readonly string Path;
    public readonly bool Randomize;
    public readonly object Value;

    public OptionPreset(IOption option, bool randomize, object value)
    {
        Path = option.Info!.PathString;
        Randomize = randomize;
        Value = value;
    }

    public OptionPreset(BoolOption option, bool randomize, bool value)
        : this((IOption)option, randomize, value)
    {
    }

    public OptionPreset(BoolOption option, bool value)
        : this(option, false, value)
    {
    }

    public override string ToString()
    {
        return $"{Path}, {Randomize}, {Value}";
    }
}
