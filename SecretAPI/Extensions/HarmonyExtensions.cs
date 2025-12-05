namespace SecretAPI.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using HarmonyLib;
    using LabApi.Features.Console;
    using SecretAPI.Attribute;

    /// <summary>
    /// Handles patching.
    /// </summary>
    public static class HarmonyExtensions
    {
        /// <summary>
        /// Patches all methods with the proper <see cref="HarmonyPatchCategory"/>.
        /// </summary>
        /// <param name="harmony">The harmony to use for the patch.</param>
        /// <param name="category">The category to patch.</param>
        /// <param name="assembly">The assembly to find patches in.</param>
        public static void PatchCategory(this Harmony harmony, string category, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            assembly.GetTypes().Where(type =>
                {
                    IEnumerable<HarmonyPatchCategory> categories = type.GetCustomAttributes<HarmonyPatchCategory>();
                    return categories.Any(c => c.Category == category);
                })
                .Do(type => SafePatch(harmony, type));
        }

        /// <summary>
        /// Patches all patches that don't have a <see cref="HarmonyPatchCategory"/>.
        /// </summary>
        /// <param name="harmony">The harmony to use for the patch.</param>
        /// <param name="assembly">The assembly to look for patches.</param>
        public static void PatchAllNoCategory(this Harmony harmony, Assembly? assembly = null)
        {
            assembly ??= Assembly.GetCallingAssembly();

            assembly.GetTypes().Where(type =>
                {
                    IEnumerable<HarmonyPatchCategory> categories = type.GetCustomAttributes<HarmonyPatchCategory>();
                    return !categories.Any();
                })
                .Do(type => SafePatch(harmony, type));
        }

        /// <summary>
        /// Attempts to safely patch a <see cref="Type"/>, logging any errors.
        /// </summary>
        /// <param name="harmony">The harmony to use for the patch.</param>
        /// <param name="type">The <see cref="Type"/> to attempt to patch.</param>
        public static void SafePatch(this Harmony harmony, Type type)
        {
            try
            {
                harmony.CreateClassProcessor(type).Patch();
            }
            catch (Exception ex)
            {
                Logger.Error($"[HarmonyExtensions] failed to safely patch {harmony.Id} ({type.FullName}): {ex}");
            }
        }
    }
}