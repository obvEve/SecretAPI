namespace SecretAPI.Attributes;

using System;
using SecretAPI.Extensions;

/// <summary>
/// Category handling for <see cref="HarmonyExtensions"/>.
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
[Obsolete("Please use HarmonyLib.HarmonyPatchCategoryAttribute - This will be removed in 4.0")]
public class HarmonyPatchCategory : HarmonyLib.HarmonyPatchCategory
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HarmonyPatchCategory"/> class.
    /// </summary>
    /// <param name="category">The category of the patch.</param>
    public HarmonyPatchCategory(string category)
        : base(category)
    {
        Category = category;
    }

    /// <summary>
    /// Gets the patch category.
    /// </summary>
    public string Category { get; }
}