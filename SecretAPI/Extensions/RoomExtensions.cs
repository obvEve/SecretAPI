namespace SecretAPI.Extensions
{
    using System.Collections.Generic;
    using LabApi.Features.Wrappers;
    using MapGeneration;
    using PlayerRoles.FirstPersonControl;
    using PlayerRoles.PlayableScps.Scp106;
    using SecretAPI.Enums;
    using UnityEngine;

    /// <summary>
    /// Extensions related to rooms.
    /// </summary>
    /// TODO: Make TryGetSafeTeleport(Room)
    public static class RoomExtensions
    {
        private static readonly List<RoomName> KnownUnsafeRooms =
        [
            RoomName.EzEvacShelter, // Stuck permanently
            RoomName.EzCollapsedTunnel, // Stuck permanently
            RoomName.HczWaysideIncinerator, // Death
            RoomName.Hcz096, // Void
        ];

        /// <summary>
        /// Gets whether a room is safe to teleport to. Will consider decontamination, warhead, teslas and void rooms.
        /// </summary>
        /// <param name="room">The room to check.</param>
        /// <param name="failReasons">Reasons why .</param>
        /// <returns>Whether the room is safe to teleport to.</returns>
        public static bool IsSafeToTeleport(this Room room, RoomSafetyFailReason failReasons)
        {
            if (failReasons.HasFlag(RoomSafetyFailReason.Warhead) && Warhead.IsDetonated && room.Zone != FacilityZone.Surface)
                return false;

            if (failReasons.HasFlag(RoomSafetyFailReason.Decontamination) && Decontamination.IsDecontaminating && room.Zone == FacilityZone.LightContainment)
                return false;

            if (failReasons.HasFlag(RoomSafetyFailReason.Tesla) && room.Name == RoomName.HczTesla)
                return false;

            if (KnownUnsafeRooms.Contains(room.Name))
                return false;

            return Physics.Raycast(room.Position, Vector3.down, out _, 2);
        }

        /// <summary>
        /// Gets a safe teleport point for a <see cref="Player"/>.
        /// </summary>
        /// <param name="player">The player to get teleport point for.</param>
        /// <param name="zone">The zone to attempt to teleport to player to.</param>
        /// <param name="range">The range of which the position is allowed to vary from its "point".</param>
        /// <param name="teleportPoint">The teleport point which was found.</param>
        /// <returns>Whether a valid position was found.</returns>
        public static bool GetSafeTeleportPoint(this Player player, FacilityZone zone, float range, out Vector3 teleportPoint)
        {
            if (player.RoleBase is not IFpcRole fpcRole)
            {
                teleportPoint = Vector3.zero;
                return false;
            }

            Pose[] poses = Scp106PocketExitFinder.GetPosesForZone(zone);
            if (poses.IsEmpty())
            {
                teleportPoint = Vector3.zero;
                return false;
            }

            Pose pose = Scp106PocketExitFinder.GetRandomPose(poses);
            Vector3 position = SafeLocationFinder.GetSafePosition(pose.position, pose.rotation.eulerAngles, range, fpcRole.FpcModule.CharController);
            teleportPoint = position;
            return true;
        }
    }
}