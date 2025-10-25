namespace SecretAPI.Extensions
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using InventorySystem;
    using LabApi.Features.Extensions;
    using PlayerRoles;
    using Respawning.Objectives;
    using UnityEngine;

    /// <summary>
    /// Extensions related to <see cref="RoleTypeId"/>.
    /// </summary>
    [Obsolete("This no longer provides anything that basegame/LabAPI does not")]
    public static class RoleExtensions
    {
        /// <summary>
        /// Tries to get a role base from a <see cref="RoleTypeId"/>.
        /// </summary>
        /// <param name="roleTypeId">The <see cref="RoleTypeId"/> to get base of.</param>
        /// <param name="role">The <see cref="PlayerRoleBase"/> found.</param>
        /// <typeparam name="T">The <see cref="PlayerRoleBase"/>.</typeparam>
        /// <returns>The role base found, else null. </returns>
        [Obsolete("Use LabApi.Features.Extensions.RoleExtensions.TryGetRoleBase")]
        public static bool TryGetRoleBase<T>(this RoleTypeId roleTypeId, [NotNullWhen(true)] out T? role)
            => LabApi.Features.Extensions.RoleExtensions.TryGetRoleBase(roleTypeId, out role);

        /// <summary>
        /// Gets the color of a <see cref="RoleTypeId"/>.
        /// </summary>
        /// <param name="roleTypeId">The role to get color of.</param>
        /// <returns>The color found, if not found then white.</returns>
        [Obsolete("Use Respawning.Objectives.GetRoleColor")]
        public static Color GetColor(this RoleTypeId roleTypeId)
            => roleTypeId.GetRoleColor();

        /// <summary>
        /// Tries to get a random spawn point from a <see cref="RoleTypeId"/>.
        /// </summary>
        /// <param name="role">The role to get spawn from.</param>
        /// <param name="position">The position found.</param>
        /// <param name="horizontalRot">The rotation found.</param>
        /// <returns>Whether a spawnpoint was found.</returns>
        [Obsolete("Use LabApi.Features.Extensions.RoleExtensions.TryGetRandomSpawnPoint")]
        public static bool GetRandomSpawnPosition(this RoleTypeId role, out Vector3 position, out float horizontalRot)
            => role.TryGetRandomSpawnPoint(out position, out horizontalRot);

        /// <summary>
        /// Gets the inventory of the specified <see cref="RoleTypeId"/>.
        /// </summary>
        /// <param name="role">The <see cref="RoleTypeId"/>.</param>
        /// <returns>The <see cref="InventoryRoleInfo"/> found.</returns>
        [Obsolete("Use LabApi.Features.Extensions.RoleExtensions.GetInventory")]
        public static InventoryRoleInfo GetInventory(this RoleTypeId role)
            => LabApi.Features.Extensions.RoleExtensions.GetInventory(role);
    }
}