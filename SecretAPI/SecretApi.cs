namespace SecretAPI;

using System;
using System.IO;
using System.Reflection;
using HarmonyLib;
using LabApi.Features;
using LabApi.Loader.Features.Paths;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using SecretAPI.Patches.Features;
using SecretAPI.Patches.Features.Settings;

/// <summary>
/// Main class handling loading API.
/// </summary>
public class SecretApi : Plugin<Config>
{
    /// <summary>
    /// Whether conditional harmony patches should be forced. This is to simply things such as <see cref="ReportDefinedSettingPatch"/> and <see cref="HarmonyLogger"/>.
    /// </summary>
    internal const bool ForceHarmonyConditionTrue =
#if DEBUG
        true;
#else
        false;
#endif

    /// <inheritdoc/>
    public override string Name => "SecretAPI";

    /// <inheritdoc/>
    public override string Description => "API for SCP:SL";

    /// <inheritdoc/>
    public override string Author => "@obvEve";

    /// <inheritdoc/>
    public override LoadPriority Priority => LoadPriority.Highest;

    /// <inheritdoc/>
    public override Version Version { get; } = Assembly.GetName().Version;

    /// <inheritdoc/>
    public override Version RequiredApiVersion => LabApiProperties.CurrentVersion;

    /// <inheritdoc />
    /// <remarks>We use transparent here because this is an API and should not interfere by itself with game logic.</remarks>
    public override bool IsTransparent => true;

    /// <summary>
    /// Gets the harmony to use for the API.
    /// </summary>
    internal static Harmony Harmony { get; } = new("SecretAPI" + DateTime.Now);

    /// <summary>
    /// Gets the Assembly of the API.
    /// </summary>
    internal static Assembly Assembly { get; } = typeof(SecretApi).Assembly;

    /// <summary>
    /// Gets the current <see cref="Config"/> loaded.
    /// </summary>
    /// <remarks>This is null before <see cref="Enable"/> is called.</remarks>
    internal static new Config Config { get; private set; } = null!;

    /// <summary>
    /// Gets the config directory within "SCP Secret Laboratory/LabAPI/configs/SecretAPI" for internal use.
    /// </summary>
    internal static DirectoryInfo ConfigDirectory => field ??= PathManager.Configs.CreateSubdirectory("SecretAPI");

    /// <inheritdoc/>
    public override void Enable()
    {
        Config = base.Config;
        Harmony.PatchCategory(nameof(SecretAPI));

        SecretApiGenerated.OnLoad();
    }

    /// <inheritdoc/>
    public override void Disable()
    {
        Harmony.UnpatchAll(Harmony.Id);
    }
}
