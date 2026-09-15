namespace SecretAPI.Examples;

using System;
using HarmonyLib;
using LabApi.Loader.Features.Plugins;
using SecretAPI.Examples.Settings;
using SecretAPI.Extensions;
using SecretAPI.Features.UserSettings;

/// <summary>
/// Defines the entry for the plugin.
/// </summary>
public class ExampleEntry : Plugin
{
    private Harmony harmony = new("SecretAPI.Examples");

    /// <inheritdoc/>
    public override string Name => "SecretAPI.Examples";

    /// <inheritdoc/>
    public override string Description => "An example plugin";

    /// <inheritdoc/>
    public override string Author => "@obvEve";

    /// <inheritdoc/>
    public override Version Version { get; } = typeof(SecretApi).Assembly.GetName().Version;

    /// <inheritdoc/>
    public override Version RequiredApiVersion { get; } = new(LabApi.Features.LabApiProperties.CompiledVersion);

    /// <inheritdoc/>
    public override void Enable()
    {
        CustomSetting.Register(new ExampleKeybindSetting(), new ExampleDropdownSetting(), new ExampleButtonSetting(), new ExampleFakeSyncButton());
        harmony.PatchCategory(nameof(ExampleFakeSyncButton));
    }

    /// <inheritdoc/>
    public override void Disable()
    {
    }
}