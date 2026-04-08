#if DEBUG

namespace SecretAPI.Debugging;

using SecretAPI.Attributes;

/// <summary>
/// Patch debugger.
/// </summary>
internal static class PatchDebugger
{
    /// <summary>
    /// Loads the debugs for patches and ensures all patches have loaded.
    /// </summary>
    [CallOnLoad]
    internal static void Load()
    {
        SecretApi.Harmony.PatchAll(SecretApi.Assembly);
    }
}

#endif