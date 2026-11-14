namespace SecretAPI.Patches.Features.Settings;

using System.Collections.Generic;
using HarmonyLib;
using NorthwoodLib.Pools;
using SecretAPI.Attributes;
using SecretAPI.Features.UserSettings;
using UserSettings.ServerSpecific;

/// <summary>
/// Logs issues with <see cref="ServerSpecificSettingsSync.DefinedSettings"/> and prevents them being registered.
/// </summary>
[HarmonyPatchCategory(nameof(CustomSetting))]
[HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.DefinedSettings), MethodType.Setter)]
internal static class ReportDefinedSettingPatch
{
    private static void Prefix(ref ServerSpecificSettingBase[] value)
    {
        List<ServerSpecificSettingBase> settings = ListPool<ServerSpecificSettingBase>.Shared.Rent();
        foreach (ServerSpecificSettingBase setting in value)
        {
            if (CustomSetting.ValidateSettingInternal(null, setting, value))
                settings.Add(setting);
        }

        value = settings.ToArray();
        ListPool<ServerSpecificSettingBase>.Shared.Return(settings);
    }
}