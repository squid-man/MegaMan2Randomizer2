using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace MM2Randomizer.Settings;

/// <summary>
/// A settings preset containing values for all options affected.
/// </summary>
public class SettingsPreset
{
    /// <summary>
    /// The name displayed in the UI.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// The name that will be displayed in the title screen of the generated seed.
    /// </summary>
    public string? TournamentTitleScreenString { get; }

    /// <summary>
    /// The individual option presets.
    /// </summary>
    public IReadOnlyList<OptionPreset> Options { get; }

    public SettingsPreset(string name, IEnumerable<OptionPreset> opts)
        : this(name, null, null, opts)
    {
    }

    public SettingsPreset(
        string name,
        string tournTitleString,
        IEnumerable<OptionPreset> opts)
        : this(name, tournTitleString, null, opts)
    {
    }

    public SettingsPreset(
        string name,
        string? tournTitleString,
        SettingsPreset? basePreset,
        IEnumerable<OptionPreset> opts)
    {
        Name = name;
        TournamentTitleScreenString = tournTitleString;

        if (basePreset is not null)
        {
            Dictionary<IOption, int> optIdcs
                = new(ReferenceEqualityComparer.Instance);
            List<OptionPreset> presets = new();
            foreach (var preset in Enumerable.Concat(basePreset.Options, opts))
            {
                int optIdx;
                if (optIdcs.TryGetValue(preset.Option, out optIdx))
                    presets[optIdx] = preset;
                else
                {
                    optIdcs[preset.Option] = presets.Count;
                    presets.Add(preset);
                }
            }

            Options = presets;
        }
        else
            Options = opts.ToArray();
    }

    public override string ToString() => Name;
}

/// <summary>
/// The collection of all settings presets, plus helper functions to reduce typing when defining presets.
/// </summary>
public class SettingsPresets
{
    public IReadOnlyList<SettingsPreset> Presets => _presets;
    List<SettingsPreset> _presets = new();

    public SettingsPresets(RandomizationSettings settings)
    {
        var s = settings;
        _presets.Add(new("", Enumerable.Empty<OptionPreset>()));

        SettingsPreset tourn2024A = new(
            "2024 Tournament A", 
            "TOURNAMENT A",
            [
                new(s.GameplayOptions.RandomizeRefightTeleporters, true),
                new(s.GameplayOptions.RandomizeRobotMasterStageSelection, true),
                new(s.GameplayOptions.RandomizeSpecialWeaponReward, true),
                new(s.GameplayOptions.RandomizeSpecialWeaponBehavior, true),
                new(s.GameplayOptions.RandomizeBossWeaknesses, true),
                new(s.GameplayOptions.RandomizeRobotMasterLocations, true),
                new(s.GameplayOptions.RandomizeRobotMasterBehavior, true),
                new(s.GameplayOptions.RandomizeSpecialItemLocations, true),
                new(s.GameplayOptions.RandomizeEnemySpawns, true),
                new(s.GameplayOptions.RandomizeEnemyWeaknesses, true),
                new(s.GameplayOptions.RandomizeFalseFloors, true),
                new(s.GameplayOptions.FasterCutsceneText, true),
                new(s.GameplayOptions.BurstChaserMode, false),
                new(s.GameplayOptions.HideStageNames, true),
                new(s.GameplayOptions.MercilessMode, false),
                new(s.SpriteOptions.RandomizeBossSprites, true),
                new(s.SpriteOptions.RandomizeEnemySprites, true),
                new(s.SpriteOptions.RandomizeSpecialWeaponSprites, true),
                new(s.SpriteOptions.RandomizeItemPickupSprites, true),
                new(s.SpriteOptions.RandomizeEnvironmentSprites, true),
                NewPreset(s.ChargingSpeedOptions.HitPoints, ChargingSpeedOption.Fastest),
                NewPreset(s.ChargingSpeedOptions.WeaponEnergy, ChargingSpeedOption.Fastest),
                NewPreset(s.ChargingSpeedOptions.EnergyTank, ChargingSpeedOption.Fastest),
                NewPreset(s.ChargingSpeedOptions.RobotMasterEnergy, ChargingSpeedOption.Fastest),
                NewPreset(s.ChargingSpeedOptions.CastleBossEnergy, ChargingSpeedOption.Fastest),
                new(s.QualityOfLifeOptions.DisableFlashingEffects, true),
                new(s.QualityOfLifeOptions.EnableUnderwaterLagReduction, true),
                new(s.QualityOfLifeOptions.DisableWaterfall, true),
                new(s.QualityOfLifeOptions.DisablePauseLock, true),
                new(s.QualityOfLifeOptions.EnableLeftwardWallEjection, false),
                new(s.QualityOfLifeOptions.EnableBirdEggFix, true),
                new(s.CosmeticOptions.RandomizeColorPalettes, true),
                new(s.CosmeticOptions.RandomizeMusicTracks, true),
                new(s.CosmeticOptions.OmitUnsafeMusicTracks, true),
                new(s.CosmeticOptions.RandomizeInGameText, true),
                new(s.CosmeticOptions.RandomizeMenusAndTransitionScreens, true)
            ]);
        _presets.Add(tourn2024A);

        _presets.Add(new(
            "2024 Tournament B",
            "TOURNAMENT B",
            tourn2024A,
            [
                new(s.SpriteOptions.RandomizeBossSprites, false),
                new(s.SpriteOptions.RandomizeEnemySprites, false),
                new(s.SpriteOptions.RandomizeSpecialWeaponSprites, false),
                new(s.SpriteOptions.RandomizeItemPickupSprites, false),
                new(s.SpriteOptions.RandomizeEnvironmentSprites, false),
            ]));
    }

    /// <summary>
    /// Create a new preset for an enum option.
    /// </summary>
    public static OptionPreset NewPreset<TEnum>(
        EnumOption<TEnum> option,
        bool randomize,
        TEnum value)
        where TEnum : struct, Enum
        => new OptionPreset(option, randomize, value);

    /// <summary>
    /// Create a new preset for an enum option.
    /// </summary>
    public static OptionPreset NewPreset<TEnum>(
        EnumOption<TEnum> option,
        TEnum value)
        where TEnum : struct, Enum
        => NewPreset(option, false, value);

    /// <summary>
    /// Verify that all presets define all non-cosmetic options.
    /// </summary>
    /// <param name="allOpts">The full list of options in the randomizer</param>
    /// <exception cref="Exception"></exception>
    public void ValidatePresets(IEnumerable<IOption> allOpts)
    {
        foreach (var preset in _presets.Skip(1))
        {
            List<IOption> unset = allOpts
                .Except<IOption>(preset.Options.Select(x => x.Option))
                .Where(x => !x.Info?.IsCosmetic ?? true)
                .ToList();

            if (unset.Count != 0)
            {
                throw new Exception(
                    $"{preset.Name} has the following missing options: "
                        + string.Join(", ", unset.Select(x => x.Info?.PathString)));
            }
        }
    }
}
