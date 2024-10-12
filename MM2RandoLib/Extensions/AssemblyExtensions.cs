using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace MM2Randomizer;

public static class AssemblyExtensions
{
    /// <summary>
    /// Load an embedded binary resource.
    /// </summary>
    /// <param name="asm">The assembly containing the resource.</param>
    /// <param name="path">The relative path to the resource.</param>
    /// <param name="asmPre">If true, prefix the assembly's name. If false, this must be done manually.</param>
    public static byte[] LoadResource(
        this Assembly asm, 
        string path, 
        bool asmPre = true)
    {
        using (var resStream = asm.GetResourceStream(path, asmPre))
        {
            var data = new byte[resStream.Length];
            resStream.ReadExactly(data, 0, data.Length);

            return data;
        }
    }

    /// <summary>
    /// Load an embedded text resource.
    /// </summary>
    /// <param name="asm">The assembly containing the resource.</param>
    /// <param name="path">The relative path to the resource.</param>
    /// <param name="enc">The text encoding of the resource.</param>
    /// <param name="asmPre">If true, prefix the assembly's name. If false, this must be done manually.</param>
    public static string LoadResource(
        this Assembly asm, 
        string path, 
        Encoding enc,
        bool asmPre = true)
    {
        using (var resStream = asm.GetResourceStream(path, asmPre))
        {
            using (var reader = new StreamReader(resStream, enc))
                return reader.ReadToEnd();
        }
    }

    /// <summary>
    /// Load an embedded resource in UTF-8 format
    /// </summary>
    /// <param name="asm">The assembly containing the resource.</param>
    /// <param name="path">The relative path to the resource.</param>
    /// <param name="asmPre">If true, prefix the assembly's name. If false, this must be done manually.</param>
    public static string LoadUtf8Resource(
        this Assembly asm, 
        string path,
        bool asmPre = true)
        => LoadResource(asm, path, Encoding.UTF8, asmPre);

    public static Stream GetResourceStream(
        this Assembly asm, 
        string path,
        bool asmPre = true)
    {
        if (asmPre)
            path = asm.GetResourcePrefix(true) + path
                ;
        var stream = asm.GetManifestResourceStream(path);
        if (stream is null)
            throw new FileNotFoundException("Resource not found", path);

        return stream;
    }

    public static string? GetResourcePrefix(
        this Assembly asm, 
        bool asmPrefix = true, 
        string? prefix = null,
        bool appendDot = true)
    {
        string dot = appendDot ? "." : "";
        if (string.IsNullOrEmpty(prefix))
        {
            return asmPrefix
                ? prefix = asm.GetName().Name + dot
                : null;
        }
        else if (!asmPrefix)
            return prefix + dot;
        else
            return $"{asm.GetName().Name}.{prefix}{dot}";

    }

    /// <summary>
    /// Enumerate the resources embedded in an assembly.
    /// </summary>
    /// <param name="asm">The assembly containing the resource.</param>
    /// <param name="asmPre">If true, prefix the assembly's name. If false, this must be done manually.</param>
    /// <param name="prefix">An optional prefix to filter paths by.</param>
    public static IEnumerable<string> GetResourceNames(
        this Assembly asm, 
        bool asmPrefix = true, 
        string? prefix = null)
    {
        prefix = asm.GetResourcePrefix(
            asmPrefix, 
            prefix,
            appendDot: prefix is null || !prefix.EndsWith("."));
        if (prefix is null)
            return asm.GetManifestResourceNames();
        else
        {
            return asm.GetManifestResourceNames()
                .Where(x => x.StartsWith(prefix))
                .Select(x => x.Substring(prefix.Length));
        }
    }
}
