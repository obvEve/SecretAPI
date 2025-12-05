namespace SecretAPI.Patches.Features
{
    using HarmonyLib;
    using LabApi.Features.Wrappers;
    using SecretAPI.Attribute;
    using SecretAPI.Features.UserSettings;
    using UserSettings.ServerSpecific;

    /// <summary>
    /// Fixes <see cref="ServerSpecificSettingsSync.SendToPlayer(ReferenceHub)"/> to resync with <see cref="CustomSetting.SendSettingsToPlayer"/>.
    /// </summary>
    [HarmonyPatchCategory(nameof(CustomSetting))]
    [HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.SendToPlayer), [typeof(ReferenceHub)])]
    internal static class SendSettingsPlayerSync
    {
        private static bool Prefix(ReferenceHub hub)
        {
            CustomSetting.SendSettingsToPlayer(Player.Get(hub));
            return false;
        }
    }
}