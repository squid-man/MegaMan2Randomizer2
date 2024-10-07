using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MM2RandoLib.Settings.Options;

/// <summary>
/// When applied to an OptionGroup class, specifies that it contains cosmetic options.
/// </summary>
[AttributeUsage(AttributeTargets.Class)]
public sealed class CosmeticOptionGroupAttribute : Attribute
{
}

/// <summary>
/// Base class for a group of options that automatically detects and compiles lists of its options and subgroups.
/// </summary>
public abstract class OptionGroup
{
    /// <summary>
    /// The options declared in this group.
    /// </summary>
    public IReadOnlyList<IOption> Options { get; }

    /// <summary>
    /// The options declared in this group that should have controls auto-created.
    /// </summary>
    public IReadOnlyList<IOption> AutoCreateOptions { get; }

    /// <summary>
    /// Subgroups declared in this group.
    /// </summary>
    public IReadOnlyList<OptionGroup> Subgroups { get; }

    /// <summary>
    /// Member metadata for options and subgroups declared in this group.
    /// </summary>
    public IReadOnlyDictionary<object, MemberInfo> MemberInfos { get; }

    public OptionGroup()
    {
        List<IOption> opts = new(),
            autoCreateOpts = new();
        List<OptionGroup> grps = new();
        Dictionary<object, MemberInfo> mbrInfos = new(
            ReferenceEqualityComparer.Instance);

        foreach (var mbrInfo in GetType().GetMembers())
        {
            TypeInfo? mbrType = null;
            object? mbrObj = null;
            if (mbrInfo is FieldInfo fieldInfo)
            {
                mbrType = fieldInfo.FieldType.GetTypeInfo();
                mbrObj = fieldInfo.GetValue(this);
            }
            else if (mbrInfo is PropertyInfo propInfo)
            {
                mbrType = propInfo.PropertyType.GetTypeInfo();
                mbrObj = propInfo.GetValue(this);
            }
            else
                continue;

            if (mbrObj is IOption opt)
            {
                mbrInfos[mbrObj] = mbrInfo;
                opts.Add(opt);
                if (mbrInfo.GetCustomAttribute<CreateControlAttribute>()?.CreateControl ?? true)
                    autoCreateOpts.Add(opt);
            }
            else
            {
                var grp = mbrObj as OptionGroup;
                if (grp is null)
                    continue;

                mbrInfos[grp] = mbrInfo;
                grps.Add(grp);
            }
        }

        Options = opts;
        AutoCreateOptions = autoCreateOpts;
        Subgroups = grps;
        MemberInfos = mbrInfos;
    }
}