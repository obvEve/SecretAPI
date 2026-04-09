namespace SecretAPI;

using System;
using System.Reflection;
using HarmonyLib;
using LabApi.Events.Handlers;
using LabApi.Features;
using LabApi.Features.Console;
using LabApi.Loader.Features.Plugins;
using LabApi.Loader.Features.Plugins.Enums;
using SecretAPI.Attributes;
using SecretAPI.Enums;
using SecretAPI.Features;

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
        CallOnLoadAttribute.Load(Assembly);

        PlayerEvents.Spawned += ev =>
        {
            if (!ev.Player.IsDummy)
                return;

            if (PlayerRoundIgnore.PlayerToStatus.ContainsKey(ev.Player))
                return;

            Logger.Info("Dummy been set to ignore status!");
            ev.Player.RoundIgnoreStatus = RoundIgnoreStatus.ScpTargetCount | RoundIgnoreStatus.RoundEndingCheck;
            Logger.Info($"Ignore Status is {ev.Player.RoundIgnoreStatus}");
        };

        ServerEvents.RoundEnding += ev =>
        {
            Logger.Info($"Is Round Ending?: {ev.IsAllowed}");
        };
    }

    /// <inheritdoc/>
    public override void Disable()
    {
        Harmony.UnpatchAll(Harmony.Id);
    }
}