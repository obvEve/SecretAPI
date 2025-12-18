namespace SecretAPI.Patches.Features
{
    using HarmonyLib;
    using SecretAPI.Attribute;
    using SecretAPI.Features.UserSettings;
    using UserSettings.ServerSpecific;

    /// <summary>
    /// Fixes validation for <see cref="CustomSetting"/>.
    /// </summary>
    [HarmonyPatchCategory(nameof(CustomSetting))]
    [HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.ServerPrevalidateClientResponse))]
    internal static class SettingsSyncValidateFix
    {
#pragma warning disable SA1313
        private static void Postfix(SSSClientResponse msg, ref bool __result)
#pragma warning restore SA1313
        {
            // prevent overriding already validated settings
            if (__result)
                return;

            __result = CustomSetting.Get(msg.SettingType, msg.Id) != null;
        }
    }
}