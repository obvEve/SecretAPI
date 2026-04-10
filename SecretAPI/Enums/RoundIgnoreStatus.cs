namespace SecretAPI.Enums;

using System;

/// <summary>
/// Defines how a player should be ignored during a round.
/// </summary>
[Flags]
public enum RoundIgnoreStatus
{
    /// <summary>
    /// Player will not be ignored.
    /// </summary>
    None = 0,

    /// <summary>
    /// Player is ignored from <see cref="RoundSummary._ProcessServerSideCode"/>.
    /// </summary>
    RoundEndingCheck = 1 << 0,

    /// <summary>
    /// Player is ignored from <see cref="RoundSummary.TargetCount"/>.
    /// </summary>
    ScpTargetCount = 1 << 1,
}