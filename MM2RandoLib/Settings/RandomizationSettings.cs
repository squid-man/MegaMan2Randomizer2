using System;
using System.Collections.Generic;
using MM2Randomizer.Random;
using MM2RandoLib.Settings.Options;
using MM2Randomizer.Settings.OptionGroups;
using System.Diagnostics;
using System.Reflection;
using System.Linq;

namespace MM2Randomizer.Settings
{
    public class RandomizationSettings : OptionGroup
    {
        //
        // Constructor
        //

        public RandomizationSettings()
        {
            BuildOptionsMetadata();
        }


        //
        // Variable Properties
        //

        public String? SeedString { get; set; }

        public String RomSourcePath { get; set; } = String.Empty;

        public SettingsPreset? SettingsPreset 
        {
            get => _preset; 
            set
            {
                HashSet<IOption> optsSet = new(ReferenceEqualityComparer.Instance);
                if (value is not null)
                {
                    foreach (var opt in value.Options)
                    {
                        opt.Option.OverrideRandomize = opt.Randomize;
                        opt.Option.OverrideValue = opt.Value;
                        opt.Option.Override = true;

                        optsSet.Add(opt.Option);
                    }
                }

                foreach (var opt in AllOptions.Except<IOption>(
                    optsSet, ReferenceEqualityComparer.Instance))
                    opt.Override = false;

                _preset = value;
            }
        }
        SettingsPreset? _preset = null;

        public Boolean CreateLogFile { get; set; }


        public bool IsTournament => !string.IsNullOrEmpty(
            _preset?.TournamentTitleScreenString);

        //
        // Option Categories
        //

        public GameplayOptions GameplayOptions { get; } = new();
        public SpriteOptions SpriteOptions { get; } = new();
        public ChargingSpeedOptions ChargingSpeedOptions { get; } = new();
        public QualityOfLifeOptions QualityOfLifeOptions { get; } = new();
        public CosmeticOptions CosmeticOptions { get; } = new();

        public IReadOnlyList<IOption> AllOptions => opts;
        public IReadOnlyDictionary<string, IOption> OptionsByPath => optsByPath;
        public IReadOnlyDictionary<string, OptionGroup> GroupsByPath => grpsByPath;


        readonly List<IOption> opts = new();
        readonly Dictionary<string, IOption> optsByPath = new();
        readonly Dictionary<string, OptionGroup> grpsByPath = new();

        //
        // Public Methods
        //

        public void ActualizeBehaviorSettings(ISeed in_Seed)
        {
            foreach (var opt in AllOptions.Where(x => x.Info is not null && !x.Info.IsCosmetic))
                opt.Actualize(in_Seed);
        }

        public void ActualizeCosmeticSettings(ISeed in_Seed)
        {
            foreach (var opt in AllOptions.Where(x => x.Info is not null && x.Info.IsCosmetic))
                opt.Actualize(in_Seed);
        }

        public void SetFromFlagString(String in_OptionsFlagString, String in_CosmeticFlagString)
        {
        }

        public String GetBehaviorFlagsString()
        {
            return GetFlagsString(
                new RandomizationFlags(28),
                AllOptions.Where(x => x.Info is not null && !x.Info.IsCosmetic));
        }


        public String GetCosmeticFlagsString()
        {
            return GetFlagsString(
                new RandomizationFlags(14),
                AllOptions.Where(x => x.Info is not null && x.Info.IsCosmetic));
        }

        void BuildOptionsMetadata()
        {
            Stack<MemberInfo> optPath = new();
            HashSet<object> checkedObjs = new(ReferenceEqualityComparer.Instance);

            BuildOptionsMetadata(optPath, this, checkedObjs);

            return;
        }

        void BuildOptionsMetadata(
            Stack<MemberInfo> optPath, 
            OptionGroup grp, 
            HashSet<object> checkedObjs)
        {
            Debug.Assert(!checkedObjs.Contains(grp));
            checkedObjs.Add(grp);

            string grpPath = string.Join(".", optPath.Reverse().Select(x => x.Name));
            grpsByPath[grpPath] = grp;

            var grpType = grp.GetType();
            bool isCosmetic = grpType.GetCustomAttribute<CosmeticOptionGroupAttribute>() is not null;

            foreach (var opt in grp.Options)
                AddOptionMetadata(optPath, grp.MemberInfos[opt], opt, isCosmetic);

            foreach (var subgrp in grp.Subgroups)
            {
                var mbrInfo = grp.MemberInfos[subgrp];

                optPath.Push(mbrInfo);
                try
                {
                    BuildOptionsMetadata(optPath, subgrp, checkedObjs);
                }
                finally
                {
                    optPath.Pop();
                }
            }

            return;
        }

        void AddOptionMetadata(
            Stack<MemberInfo> optPath,
            MemberInfo mbrInfo,
            IOption opt,
            bool isCosmetic)
        {
            opt.Info = new(optPath.Reverse().Append(mbrInfo), opt, isCosmetic);

            opts.Add(opt);
            optsByPath[opt.Info.PathString] = opt;
        }

        String GetFlagsString(
            RandomizationFlags flags, 
            IEnumerable<IOption> opts)
        {
            foreach (var iopt in opts)
            {
                flags.PushValue(iopt.UiRandomize);
                if (iopt is BoolOption opt)
                    flags.PushValue(opt.UiValue);
                else if (iopt is IEnumOption ienumOpt)
                    flags.PushValue(
                        ienumOpt.EnumValueIdcs[iopt.UiValue],
                        Enum.GetValues(iopt.Type).Length);
                else
                    Debug.Assert(false);
            }

            return flags.ToFlagString('.');
        }
    }
}
