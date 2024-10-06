using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Reflection;

namespace RandomizerHost.Settings;

/// <summary>
/// An adapter to make randomization options visible to .NET's application settings system. Synchronizes option values through two-way change notifications. It's possible a better way will be found in the future.
/// </summary>
[SettingsGroupName("RandomizationSettings")]
public sealed class RandomizationSettingsAdapter : ApplicationSettingsBase
{
    Dictionary<string, Action<object>> _propSetters = new();
    List<PropertyChangedEventHandler> _changeHdlrs = new();

    public RandomizationSettingsAdapter(RandomizationSettings settings, SettingsProvider provider)
    {
        PropertyInfo optRndProp = typeof(IOption).GetProperty(nameof(IOption.Randomize)),
            optValueProp = typeof(IOption).GetProperty(nameof(IOption.BaseValue));

        if (Providers.Count == 0)
            Providers.Add(provider);

        foreach (var opt in settings.AllOptions.Where(opt => opt.Info.SaveLoad))
        {
            var optInfo = opt.Info;
            SettingsProperty rndProp = new(optInfo.PathString + ":Randomize")
            {
                PropertyType = typeof(bool),
                Provider = Providers[nameof(LocalFileSettingsProvider)],
                IsReadOnly = false,
                DefaultValue = "False",
            };
            rndProp.Attributes.Add(typeof(UserScopedSettingAttribute),
                new UserScopedSettingAttribute());

            SettingsProperty valueProp = new(optInfo.PathString + ":Value")
            {
                PropertyType = optInfo.Type,
                Provider = Providers[nameof(LocalFileSettingsProvider)],
                IsReadOnly = false,
                DefaultValue = opt.DefaultValue.ToString(),
            };
            valueProp.Attributes.Add(typeof(UserScopedSettingAttribute),
                new UserScopedSettingAttribute());

            Properties.Add(rndProp);
            Properties.Add(valueProp);

            _propSetters[rndProp.Name] = (object value) => optRndProp.SetValue(opt, value);
            _propSetters[valueProp.Name] = (object value) => optValueProp.SetValue(opt, value);

            PropertyChangedEventHandler changeHdlr = (object sender, PropertyChangedEventArgs args) =>
            {
                if (args.PropertyName == nameof(IOption.Randomize))
                    this[rndProp.Name] = optRndProp.GetValue(sender);
                else if (args.PropertyName == nameof(IOption.BaseValue))
                    this[valueProp.Name] = optValueProp.GetValue(sender);
            };
            _changeHdlrs.Add(changeHdlr);
            opt.PropertyChanged += changeHdlr;
        }

        foreach (var value in Providers.Cast<SettingsProvider>().First()
            .GetPropertyValues(Context, Properties)
            .Cast<SettingsPropertyValue>())
        {
            this[value.Name] = value.PropertyValue;
        }

        return;
    }

    public override object this[string propName]
    {
        get => base[propName];
        set
        {
            _propSetters[propName](value);
            base[propName] = value;
        }
    }
}
