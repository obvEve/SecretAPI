namespace SecretAPI.Patches.Features
{
    using HarmonyLib;
    using SecretAPI.Attribute;
    using SecretAPI.Features.UserSettings;
    using UserSettings.ServerSpecific;

    /// <summary>
    /// Fixes <see cref="ServerSpecificSettingBase.OriginalDefinition"/> on custom settings.
    /// </summary>
    [HarmonyPatchCategory(nameof(CustomSetting))]
    [HarmonyPatch(typeof(ServerSpecificSettingBase), nameof(ServerSpecificSettingBase.OriginalDefinition), MethodType.Getter)]
    internal static class SettingsOriginalDefinitionFix
    {
#pragma warning disable SA1313
        private static void Postfix(ServerSpecificSettingBase __instance, ref ServerSpecificSettingBase __result)
#pragma warning restore SA1313
        {
            // Prevent handling non SecretAPI settings.
            if (__result != null)
                return;

            __result = CustomSetting.Get(__instance.GetType(), __instance.SettingId)?.Base ?? null!;
        }
    }
}