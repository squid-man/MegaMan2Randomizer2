using System;
using System.Collections.Generic;
using System.Linq;

namespace MM2RandoLib.Settings.Options;

/// <summary>
/// A preset for a single option.
/// </summary>
public class OptionPreset
{
    public readonly IOption Option;
    public readonly bool Randomize;
    public readonly object Value;

    public OptionPreset(IOption option, bool randomize, object value)
    {
        Option = option;
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
        return $"{Option.Info?.Name}, {Randomize}, {Value}";
    }
}
