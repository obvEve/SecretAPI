namespace SecretAPI;

using System.ComponentModel;
using SecretAPI.Features.UserSettings;
using UserSettings.ServerSpecific;

/// <summary>
/// Handles config for SecretAPI.
/// </summary>
public sealed class Config
{
    /// <summary>
    /// Gets or sets a value indicating whether PatchClassProcessor.ReportException should be edited to log instead of throwing exception.
    /// </summary>
    [Description("When set to true, will patch PatchClassProcessor.ReportException to log instead of throw.")]
    public bool LogHarmonyReplaceThrow { get; set; } = false;

    /// <summary>
    /// Gets or sets a value indicating whether <see cref="CustomSetting"/> should validate <see cref="ServerSpecificSettingsSync.DefinedSettings"/>.
    /// </summary>
    [Description("When set to true and CustomSetting is used, will begin validating ServerSpecificSettingsSync.DefinedSettings.")]
    public bool ValidateDefinedSettings { get; set; } = true;
}