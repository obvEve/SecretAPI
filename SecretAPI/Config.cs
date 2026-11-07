namespace SecretAPI;

/// <summary>
/// Handles config for SecretAPI.
/// </summary>
public sealed class Config
{
    /// <summary>
    /// Gets or sets a value indicating whether PatchClassProcessor.ReportException should be edited to log instead of throwing exception.
    /// </summary>
    public bool LogHarmonyReplaceThrow { get; set; } = false;
}