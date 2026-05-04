namespace SecretAPI;

using System;
using System.Reflection;
using HarmonyLib;
using LabApi.Features;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using SecretAPI.Attributes;

/// <summary>
/// Main class handling loading API.
/// </summary>
public class SecretApi : Plugin
{
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

    /// <inheritdoc/>
    public override void Enable()
    {
        SecretApiGenerated.OnLoad();
    }

    /// <inheritdoc/>
    public override void Disable()
    {
        Harmony.UnpatchAll(Harmony.Id);
    }
}
