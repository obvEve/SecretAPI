namespace SecretAPI.Enums
{
    using System;
    using MapGeneration;
    using SecretAPI.Extensions;
    using UnityEngine;

    /// <summary>
    /// Reasons why <see cref="RoomExtensions.IsSafeToTeleport"/> should fail.
    /// </summary>
    [Flags]
    public enum RoomSafetyFailReason
    {
        /// <summary>
        /// No fail.
        /// </summary>
        None = 0,

        /// <summary>
        /// Room safety check will fail if warhead has gone off and room is not part of <see cref="FacilityZone.Surface"/>.
        /// </summary>
        Warhead = 1 << 0,

        /// <summary>
        /// Room safety check will fail if decontamination has gone off and room is part of <see cref="FacilityZone.LightContainment"/>.
        /// </summary>
        Decontamination = 1 << 1,

        /// <summary>
        /// Room safety check will fail if room has <see cref="TeslaGate"/>.
        /// </summary>
        Tesla = 1 << 2,

        /// <summary>
        /// Room safety check will fail if the listed room fails a <see cref="Physics.Raycast(Vector3, Vector3, out RaycastHit, float, int)"/> check.
        /// </summary>
        MissingFloor = 1 << 3,

        /// <summary>
        /// Room safety check will fail if the listed room is part of <see cref="RoomExtensions.KnownUnsafeRooms"/>.
        /// </summary>
        KnownBad = 1 << 4,
    }
}