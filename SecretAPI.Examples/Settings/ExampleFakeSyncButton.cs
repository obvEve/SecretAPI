namespace SecretAPI.Examples.Settings;

using System.Collections.Generic;
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

    /// <summary>
    /// Gets a dictionary of <see cref="ReferenceHub"/> to theirfake synced ammos.
    /// </summary>
    public static Dictionary<ReferenceHub, ushort> FakeSyncs { get; } = new();

    /// <summary>
    /// Gets the <see cref="ItemType"/> who's ammo is being fake synced.
    /// </summary>
    public static ItemType AmmoFakeSync => ItemType.Ammo9x19;

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

        // fake syncing ammo limits
        // you must patch the values on server-side too, refer to ExampleAmmoPatch.cs
        FakeSyncs[KnownOwner.ReferenceHub] = ushort.MinValue;
        KnownOwner.SendFakeSyncListData<ServerConfigSynchronizer.AmmoLimit>(ServerConfigSynchronizer.Singleton, 2, new()
        {
            Operation = SyncList<ServerConfigSynchronizer.AmmoLimit>.Operation.OP_SET,
            Index = ServerConfigSynchronizer.Singleton.AmmoLimitsSync.FindIndex(limit => limit.AmmoType == AmmoFakeSync),
            Item = new ServerConfigSynchronizer.AmmoLimit()
            {
                AmmoType = AmmoFakeSync,
                Limit = ushort.MinValue,
            },
        });

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