#if DEBUG

namespace SecretAPI.Debugging;

using System.Collections.Generic;
using System.IO;
using System.Reflection;
using SecretAPI.Attributes;

/// <summary>
/// Ensures all patches are loaded for debug purposes.
/// </summary>
internal static class PatchDebugger
{
    /// <summary>
    /// Loads the debug.
    /// </summary>
    [CallOnLoad]
    internal static void Load()
    {
        SecretApi.Harmony.PatchAll(SecretApi.Assembly);
        IEnumerable<MethodBase> patchedMethods = SecretApi.Harmony.GetPatchedMethods();
        int patchCount = 0;
        List<string> patchedMethodNames = new();
        foreach (MethodBase patchedMethod in patchedMethods)
        {
            patchCount++;
            patchedMethodNames.Add((patchedMethod.IsStatic ? "static " : "instance ") + (patchedMethod.DeclaringType?.FullName ?? "No Type") + $" : {patchedMethod.Name}");
        }

        patchedMethodNames.Insert(0, $"SecretAPI has patched {patchCount} methods:");
        File.WriteAllLines(Path.Combine(SecretApi.ConfigDirectory.FullName, "debug_patches.txt"), patchedMethodNames);
    }
}

#endif