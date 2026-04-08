namespace SecretAPI.Extensions;

using System;
using SecretAPI.Enums;

/// <summary>
/// Faster flag checking for <see cref="Enum"/>.
/// </summary>
public static class FastEnums
{
    /// <inheritdoc cref="Enum.HasFlag"/>
    public static bool HasFlagFast(this DoorPermissionCheck @enum, DoorPermissionCheck flag) => (@enum & flag) == flag;

    /// <inheritdoc cref="Enum.HasFlag"/>
    public static bool HasFlagFast(this RoomSafetyFailReason @enum, RoomSafetyFailReason flag) => (@enum & flag) == flag;

    /// <inheritdoc cref="Enum.HasFlag"/>
    public static bool HasFlagFast(this RoundIgnoreStatus @enum, RoundIgnoreStatus flag) => (@enum & flag) == flag;
}