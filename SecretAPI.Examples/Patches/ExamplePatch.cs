namespace SecretAPI.Examples.Patches
{
    using SecretAPI.Attribute;

    /// <summary>
    /// An example harmony patch.
    /// </summary>
    [HarmonyPatchCategory(nameof(ExampleEntry))]
    /*[HarmonyPatch]*/
    public static class ExamplePatch
    {
        // gets called before the original method is called
        private static bool Prefix()
        {
            // prevent original method from running
            return false;
        }

        // gets called after the original method is called
        private static void Postfix()
        {
        }
    }
}