namespace SecretAPI.Patches.Features
{
    using HarmonyLib;
    using SecretAPI.Attribute;
    using SecretAPI.Features.UserSettings;
    using UserSettings.ServerSpecific;

    /// <summary>
    /// Fixes <see cref="ServerSpecificSettingsSync.SendToAll"/> to resync with <see cref="CustomSetting.ResyncServer"/>.
    /// </summary>
    [HarmonyPatchCategory(nameof(CustomSetting))]
    [HarmonyPatch(typeof(ServerSpecificSettingsSync), nameof(ServerSpecificSettingsSync.SendToAll))]
    internal static class SendSettingsServerSync
    {
        private static bool Prefix()
        {
            CustomSetting.ResyncServer();
            return false;
        }
    }
}