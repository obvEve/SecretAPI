namespace SecretAPI.Enums
{
    using System;
    using Interactables.Interobjects.DoorUtils;
    using LabApi.Features.Wrappers;
    using PlayerRoles;
    using PlayerStatsSystem;
    using SecretAPI.Extensions;

    /// <summary>
    /// Flags to use for <see cref="PlayerExtensions.HasDoorPermission(Player,IDoorPermissionRequester,DoorPermissionCheck)"/>.
    /// </summary>
    [Flags]
    public enum DoorPermissionCheck
    {
        /// <summary>
        /// None. Will not check anything.
        /// </summary>
        None = 0,

        /// <summary>
        /// Used to consider <see cref="AdminFlags.BypassMode"/>.
        /// </summary>
        Bypass = 1,

        /// <summary>
        /// Used to consider the player's <see cref="PlayerRoleBase"/>.
        /// </summary>
        Role = 2,

        /// <summary>
        /// Used to consider the player's <see cref="Player.CurrentItem"/>.
        /// </summary>
        CurrentItem = 4,

        /// <summary>
        /// Used to consider the player's inventory, not including the item they are holding.
        /// </summary>
        InventoryExcludingCurrent = 8,

        /// <summary>
        /// Used to consider the player's ENTIRE inventory.
        /// </summary>
        FullInventory = CurrentItem | InventoryExcludingCurrent,

        /// <summary>
        /// Used to consider all.
        /// </summary>
        All = -1,

        /// <summary>
        /// Used to mirror default base-game checks (bypass mode, role and held item).
        /// </summary>
        Default = Bypass | Role | CurrentItem,
    }
}