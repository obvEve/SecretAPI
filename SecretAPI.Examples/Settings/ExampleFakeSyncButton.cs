namespace SecretAPI.Examples.Settings;

using LabApi.Features.Wrappers;
using MEC;
using Mirror;
using SecretAPI.Extensions;
using SecretAPI.Features.UserSettings;
using UnityEngine;

/// <summary>
/// Example version for fake syncing on a <see cref="CustomButtonSetting"/>.
/// </summary>
public class ExampleFakeSyncButton : CustomButtonSetting
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ExampleFakeSyncButton"/> class.
    /// </summary>
    public ExampleFakeSyncButton()
        : base(typeof(ExampleFakeSyncButton).FullName?.GetHashCode(), "Example Fake Sync Button", "Fake!")
    {
    }

    /// <inheritdoc />
    public override CustomHeader Header => CustomHeader.Examples;

    /// <inheritdoc/>
    protected override bool CanView(Player player) => player.RemoteAdminAccess;

    /// <inheritdoc />
    protected override CustomSetting CreateDuplicate() => new ExampleFakeSyncButton();

    /// <inheritdoc />
    protected override void HandleSettingUpdate()
    {
        if (KnownOwner == null)
            return;

        TextToy textToy = TextToy.Create(KnownOwner.Position, KnownOwner.Rotation);
        textToy.TextFormat = "{0}";
        textToy.Arguments.Add("Default Text!");

        CapybaraToy capybaraToy = CapybaraToy.Create(KnownOwner.Position + new Vector3(0, 1, 0), KnownOwner.Rotation);
        capybaraToy.Base.NetworkCollisionsEnabled = true;

        Timing.CallDelayed(5, () =>
        {
            // sync var example
            KnownOwner.SendFakeSyncVar(capybaraToy.Base, 1L, KnownOwner.Position + new Vector3(0, 1, 0)); // position
            KnownOwner.SendFakeSyncVar(capybaraToy.Base, 32L, false); // collisions

            // sync list example
            MirrorExtensions.SyncListChange<string> change = new()
            {
                Operation = SyncList<string>.Operation.OP_SET,
                Index = 0,
                Item = "Fake synced text!",
            };

            KnownOwner.SendFakeSyncListData(textToy.Base, 1L, change);
        });
    }
}