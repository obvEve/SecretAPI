namespace SecretAPI.Patches.Features.Settings;

using HarmonyLib;
using SecretAPI.Attributes;
using SecretAPI.Features.UserSettings;
using UserSettings.ServerSpecific;

/// <summary>
/// Fixes validation for <see cref="CustomSetting"/>.
/// </summary>
[HarmonyPatchCategory(nameof(CustomSetting))]
[HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.ServerPrevalidateClientResponse))]
internal static class SettingsSyncValidateFix
{
    private static void Postfix(SSSClientResponse msg, [HarmonyArgument("__result")] ref bool result)
    {
        // prevent overriding already validated settings
        if (result)
            return;

        result = CustomSetting.Get(msg.SettingType, msg.Id) != null;
    }
}