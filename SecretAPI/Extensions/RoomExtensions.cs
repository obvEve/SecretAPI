namespace SecretAPI.Extensions;

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using LabApi.Features.Wrappers;
using MapGeneration;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp106;
using SecretAPI.Enums;
using UnityEngine;

/// <summary>
/// Extensions related to rooms.
/// </summary>
public static class RoomExtensions
{
    private const float RaycastDistance = 2;

    /// <summary>
    /// Gets a list of <see cref="RoomName"/> that will be denied by <see cref="RoomSafetyFailReason.KnownBad"/>.
    /// </summary>
    public static List<RoomName> KnownUnsafeRooms { get; } =
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
    /// <param name="failReasons">Reasons why the safety check should fail.</param>
    /// <returns>Whether the room is safe to teleport to.</returns>
    public static bool IsSafeToTeleport(this Room room, RoomSafetyFailReason failReasons)
    {
        if (failReasons.HasFlag(RoomSafetyFailReason.Warhead) && Warhead.IsDetonated && room.Zone != FacilityZone.Surface)
            return false;

        if (failReasons.HasFlag(RoomSafetyFailReason.Decontamination) && Decontamination.IsDecontaminating && room.Zone == FacilityZone.LightContainment)
            return false;

        if (failReasons.HasFlag(RoomSafetyFailReason.Tesla) && room.Name == RoomName.HczTesla)
            return false;

        if (failReasons.HasFlag(RoomSafetyFailReason.KnownBad) && KnownUnsafeRooms.Contains(room.Name))
            return false;

        if (failReasons.HasFlag(RoomSafetyFailReason.MissingFloor) && !Physics.Raycast(room.Position, Vector3.down, out _, RaycastDistance, FpcStateProcessor.Mask))
            return false;

        return true;
    }

    /// <summary>
    /// Tries to get a location to teleport a <see cref="Player"/> to.
    /// </summary>
    /// <param name="player">The player to attempt to get a teleport position from.</param>
    /// <param name="position">The position found if any, otherwise null.</param>
    /// <param name="zone">If set to anything other than <see cref="FacilityZone.None"/> will only attempt to find in that zone.</param>
    /// <param name="defaultRadius">The default radius allowed nea  the found spot.</param>
    /// <returns>Whether a valid teleport position was correctly found.</returns>
    public static bool TryGetTeleportLocation(this Player player, [NotNullWhen(true)] out Vector3? position, FacilityZone zone = FacilityZone.None, float defaultRadius = Scp106PocketExitFinder.RaycastRange)
    {
        position = null;
        return player.RoleBase is IFpcRole fpc && TryGetTeleportLocation(fpc, out position, zone);
    }

    /// <summary>
    /// Tries to get a location to teleport a <see cref="IFpcRole"/> to.
    /// </summary>
    /// <param name="fpc">The <see cref="IFpcRole"/> to attempt to get a teleport position from.</param>
    /// <param name="position">The position found if any, otherwise null.</param>
    /// <param name="zone">If set to anything other than <see cref="FacilityZone.None"/> will only attempt to find in that zone.</param>
    /// <param name="defaultRadius">The default radius allowed nea  the found spot.</param>
    /// <returns>Whether a valid teleport position was correctly found.</returns>
    public static bool TryGetTeleportLocation(this IFpcRole fpc, [NotNullWhen(true)] out Vector3? position, FacilityZone zone = FacilityZone.None, float defaultRadius = Scp106PocketExitFinder.RaycastRange)
    {
        position = null;

        IEnumerable<Pose> poses = zone == FacilityZone.None
            ? SafeLocationFinder.GetLocations(null, null)
            : Scp106PocketExitFinder.GetPosesForZone(zone);

        if (!poses.TryGetRandomValue(out Pose pose))
            return false;

        float radius = defaultRadius;
        if (Room.TryGetRoomAtPosition(pose.position, out Room? room))
            radius = Scp106PocketExitFinder.GetRaycastRange(room.Zone);

        position = SafeLocationFinder.GetSafePosition(pose.position, pose.forward, radius, fpc.FpcModule.CharController);
        return true;
    }
}