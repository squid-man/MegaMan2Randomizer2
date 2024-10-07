using MM2Randomizer.Random;
using MM2Randomizer.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace MM2RandoLib.Settings.Options;

/// <summary>
/// The base interface for all randomization options.
/// </summary>
public interface IOption : INotifyPropertyChanged
{
    /// <summary>
    /// The option's value type.
    /// </summary>
    Type Type { get; }

    /// <summary>
    /// Auto-generated information about the option to integrate it into the option system.
    /// </summary>
    OptionInfo? Info { get; set; }

    object DefaultValue { get; }

    /// <summary>
    /// The base setting for whether the option should be randomized as set by the user.
    /// </summary>
    bool Randomize { get; set; }

    /// <summary>
    /// The value of the option as set by the user, which may be overridden in multiple ways.
    /// </summary>
    object BaseValue { get; set; }

    /// <summary>
    /// Whether the user-specified value of the option is being overridden e.g. by a preset.
    /// </summary>
    bool Override { get; set; }
    bool OverrideRandomize { get; set; }
    object OverrideValue { get; set; }

    /// <summary>
    /// The "actualized" value of the option for the randomizer modules.
    /// </summary>
    object Value { get; }

    /// <summary>
    /// The user-interface version of Randomize. Shows either the user-specified value or the overridden value depending on whether override is active.
    /// </summary>
    bool UiRandomize { get; set; }

    /// <summary>
    /// The user-interface version of BaseValue. Shows either the user-specified value or the overridden value depending on whether override is active.
    /// </summary>
    object UiValue { get; set; }

    /// <summary>
    /// Parse a string to the value type of the option.
    /// </summary>
    object ParseType(string value);

    /// <summary>
    /// Determine the final randomized value for the option.
    /// </summary>
    /// <param name="seed"></param>
    void Actualize(ISeed seed);
}

/// <summary>
/// The base interface for all options based on enum values.
/// </summary>
public interface IEnumOption : IOption
{
    /// <summary>
    /// The list of valid values for the enum as returned by Enum.GetValues.
    /// </summary>
    IReadOnlyList<object> EnumValues { get; }

    /// <summary>
    /// The mapping of enum values to indices in EnumValues.
    /// </summary>
    IReadOnlyDictionary<object, int> EnumValueIdcs { get; }

    /*int BaseValueIndex { get; set; }
    int EffectiveValueIndex { get; set; }*/
}

public abstract class Option<T> : IOption
    where T : notnull
{
    public Type Type => typeof(T);

    public OptionInfo? Info
    {
        get => _info;
        set => SetPropertyValue(ref _info, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public readonly T DefaultValue;
    object IOption.DefaultValue => DefaultValue;

    public bool Randomize 
    {
        get => _randomize;
        set => SetPropertyValue(ref _randomize, value);
    }
    public T BaseValue 
    {
        get => _baseValue;
        set => SetPropertyValue(ref _baseValue, value);
    }
    object IOption.BaseValue 
    { 
        get => BaseValue; 
        set => BaseValue = (T)value; 
    }

    public bool Override 
    {
        get => _override;
        set => SetPropertyValue(ref _override, value);
    }
    public bool OverrideRandomize 
    {
        get => _overrideRandomize;
        set => SetPropertyValue(ref _overrideRandomize, value);
    }
    public T OverrideValue 
    {
        get => _overrideValue;
        set => SetPropertyValue(ref _overrideValue, value);
    }
    object IOption.OverrideValue
    {
        get => OverrideValue;
        set => OverrideValue = (T)value;
    }

    public bool UiRandomize
    {
        get => _uiRandomize;
        set
        {
            if (!_override)
                Randomize = value;
        }
    }
    public T UiValue
    {
        get => _uiValue;
        set
        {
            if (!_override)
                BaseValue = value;
        }
    }
    object IOption.UiValue { 
        get => UiValue; 
        set => UiValue = (T)value; 
    }

    public T Value
    {
        get => _value;
        private set => SetPropertyValue(ref _value, value);
    }
    object IOption.Value => Value;

    protected OptionInfo? _info = null;

    protected bool _randomize;
    protected T _baseValue;

    protected bool _override;
    protected bool _overrideRandomize;
    protected T _overrideValue;

    protected bool _uiRandomize;
    protected T _uiValue;

    protected T _value;

    public Option(T defaultValue)
    {
        DefaultValue = _baseValue = _overrideValue = _uiValue = _value = defaultValue;
        _randomize = _override = _overrideRandomize = _uiRandomize = false;
    }

    public override string ToString()
    {
        string ovrStr = _override ? $", Ovr = {_overrideRandomize},{_overrideValue}" : "";
        return $"{_value}, Def = {DefaultValue}, Rnd = {_randomize}, Val = {_baseValue}{ovrStr}, ERnd = {_uiRandomize}, EVal = {_uiValue}";
    }

    public abstract T ParseType(string value);
    object IOption.ParseType(string value) => ParseType(value);

    public void Actualize(ISeed seed)
    {
        Value = (_override ? _overrideRandomize : _randomize)
            ? GetRandomizedOption(seed)
            : (_override ? _overrideValue : _baseValue);
    }

    protected abstract T GetRandomizedOption(ISeed seed);

    protected void UpdateUi()
    {
        bool rnd = _override ? _overrideRandomize : _randomize;
        if (rnd != _uiRandomize)
        {
            _uiRandomize = rnd;
            NotifyPropertyChanged(nameof(UiRandomize));
        }

        T value = _override ? _overrideValue : _baseValue;
        if (!value.Equals(_uiValue))
        {
            _uiValue = value;
            NotifyPropertyChanged(nameof(UiValue));
        }
    }

    protected void NotifyPropertyChanged(
        [CallerMemberName] string propName = "")
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
    }

    protected void SetPropertyValue<TProp>(
        ref TProp field,
        TProp value,
        [CallerMemberName] string propName = "")
    {
        Debug.Assert(propName != nameof(UiRandomize)
            && propName != nameof(UiValue));

        if ((value is not null && !value.Equals(field))
            || (value is null) != (field is null))
        {
            field = value;

            NotifyPropertyChanged(propName);
            UpdateUi();
        }
    }
}

/// <summary>
/// A bool-values option.
/// </summary>
public class BoolOption : Option<bool>
{
    public BoolOption(bool value)
        : base(value)
    {
    }

    public override bool ParseType(string value) => bool.Parse(value);

    protected override bool GetRandomizedOption(ISeed seed)
        => seed.NextBoolean();
}

/// <summary>
/// An enum-valued option.
/// </summary>
/// <typeparam name="TEnum">The specific enum type</typeparam>
public class EnumOption<TEnum> : Option<TEnum>, IEnumOption
    where TEnum : struct, Enum
{
    public static readonly IReadOnlyList<TEnum> Values = Enum.GetValues<TEnum>();
    static readonly IReadOnlyList<object> ObjEnumValues = Values.Cast<object>().ToArray();
    IReadOnlyList<object> IEnumOption.EnumValues => ObjEnumValues;
    public IReadOnlyList<TEnum> EnumValues => Values;

    public static readonly IReadOnlyDictionary<TEnum, int> EnumValueIdcs = Values.Zip(Enumerable.Range(0, Values.Count)).ToDictionary();
    static readonly IReadOnlyDictionary<object, int> ObjEnumValueIdcs = EnumValueIdcs.ToDictionary(kv => (object)kv.Key, kv => kv.Value);
    IReadOnlyDictionary<object, int> IEnumOption.EnumValueIdcs => ObjEnumValueIdcs;

    /*public int BaseValueIndex
    {
        get => EnumValueIdcs[_baseValue];
    }*/


    public EnumOption(TEnum value)
        : base(value)
    {
    }

    public override TEnum ParseType(string value) => Enum.Parse<TEnum>(value);

    protected override TEnum GetRandomizedOption(ISeed seed) 
        => seed.NextEnum<TEnum>();
}
