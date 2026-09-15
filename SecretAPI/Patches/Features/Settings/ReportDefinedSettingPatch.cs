namespace SecretAPI.Patches.Features.Settings;

using System.Collections.Generic;
using HarmonyLib;
using NorthwoodLib.Pools;
using SecretAPI.Attributes;
using SecretAPI.Features.UserSettings;
using UserSettings.ServerSpecific;

/// <summary>
/// Logs issues with <see cref="ServerSpecificSettingsSync.DefinedSettings"/>.
/// </summary>
[HarmonyPatchCategory(nameof(CustomSetting))]
[HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.DefinedSettings), MethodType.Setter)]
internal static class ReportDefinedSettingPatch
{
    private static void Postfix(ref ServerSpecificSettingBase[] value)
    {
        // OLD : value.ForEach(setting => CustomSetting.ValidateSettingInternal(null, setting));
        List<ServerSpecificSettingBase> settings = ListPool<ServerSpecificSettingBase>.Shared.Rent();
        foreach (ServerSpecificSettingBase setting in value)
        {
            if (CustomSetting.ValidateSettingInternal(null, setting))
                settings.Add(setting);
        }

        value = settings.ToArray();
    }
}