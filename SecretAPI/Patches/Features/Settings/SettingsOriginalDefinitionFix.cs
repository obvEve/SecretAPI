namespace SecretAPI.Patches.Features.Settings;

using HarmonyLib;
using SecretAPI.Attributes;
using SecretAPI.Features.UserSettings;
using UserSettings.ServerSpecific;

/// <summary>
/// Fixes <see cref="ServerSpecificSettingBase.OriginalDefinition"/> on custom settings.
/// </summary>
[HarmonyPatchCategory(nameof(CustomSetting))]
[HarmonyPatch(typeof(ServerSpecificSettingBase), nameof(ServerSpecificSettingBase.OriginalDefinition), MethodType.Getter)]
internal static class SettingsOriginalDefinitionFix
{
    private static void Postfix([HarmonyArgument("__instance")] ServerSpecificSettingBase instance, [HarmonyArgument("__result")] ref ServerSpecificSettingBase result)
    {
        // Prevent handling non SecretAPI settings.
        if (result != null)
            return;

        result = CustomSetting.Get(instance.GetType(), instance.SettingId)?.Base ?? null!;
    }
}