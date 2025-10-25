namespace SecretAPI.Extensions
{
    using System.Collections.Generic;
    using LabApi.Features.Wrappers;
    using MapGeneration;
    using UnityEngine;

    /// <summary>
    /// Extensions related to rooms.
    /// </summary>
    public static class RoomExtensions
    {
        private static readonly List<RoomName> KnownUnsafeRooms =
        [
            RoomName.HczTesla, // Instant death
            RoomName.EzEvacShelter, // Stuck permanently
            RoomName.EzCollapsedTunnel, // Stuck permanently
            RoomName.HczWaysideIncinerator, // Death
            RoomName.Hcz096, // Void
        ];

        /// <summary>
        /// Gets whether a room is safe to teleport to. Will consider decontamination, warhead, teslas and void rooms.
        /// </summary>
        /// <param name="room">The room to check.</param>
        /// <returns>Whether the room is safe to teleport to.</returns>
        public static bool IsSafeToTeleport(this Room room)
        {
            if (Warhead.IsDetonated && room.Zone != FacilityZone.Surface)
                return false;

            if (Decontamination.IsDecontaminating && room.Zone == FacilityZone.LightContainment)
                return false;

            if (KnownUnsafeRooms.Contains(room.Name))
                return false;

            return Physics.Raycast(room.Position, Vector3.down, out _, 2);
        }
    }
}