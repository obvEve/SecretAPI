namespace SecretAPI.Examples.Patches;

using HarmonyLib;
using InventorySystem.Configs;
using SecretAPI.Attributes;
using SecretAPI.Examples.Settings;

/// <summary>
/// An example harmony patch.
/// </summary>
[HarmonyPatchCategory(nameof(ExampleFakeSyncButton))]
[HarmonyPatch(typeof(InventoryLimits), nameof(InventoryLimits.GetAmmoLimit), [typeof(ItemType), typeof(ReferenceHub)])]
public static class ExampleAmmoPatch
{
    // gets called before the original method is called
    private static void Prefix()
    {
        // we return void so original method is always run
    }

    // gets called after the original method is called
    // We grab the method params of ammoType and player, the names must be correct
    // ref __result will become a reference to the return value
#pragma warning disable SA1313
    private static void Postfix(ItemType ammoType, ReferenceHub player, ref ushort __result)
#pragma warning restore SA1313
    {
        // make sure we don't modify the max ammo if its not the correct ammo type or the player hasn't been fake synced
        if (ammoType != ExampleFakeSyncButton.AmmoFakeSync || !ExampleFakeSyncButton.FakeSyncs.TryGetValue(player, out ushort sync))
            return;

        __result = sync;
    }
}