namespace SecretAPI.Features;

using System.Collections.Generic;
using LabApi.Events.Arguments.PlayerEvents;
using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using SecretAPI.Enums;
using SecretAPI.Extensions;

/// <summary>
/// Handles allowing easier ignoring of players for <see cref="Round"/>.
/// </summary>
public static class PlayerRoundIgnore
{
    extension(Player player)
    {
        /// <summary>
        /// Gets or sets the players current <see cref="RoundIgnoreStatus"/>.
        /// </summary>
        public RoundIgnoreStatus RoundIgnoreStatus
        {
            get => PlayerToStatus.GetValueOrDefault(player, RoundIgnoreStatus.None);
            set => PlayerToStatus[player] = value;
        }
    }

    static PlayerRoundIgnore()
    {
        SecretApi.Harmony.PatchCategory(nameof(PlayerRoundIgnore));
        PlayerEvents.Left += OnPlayerLeft;
    }

    /// <summary>
    /// Gets a collection of <see cref="Player"/> to their current <see cref="RoundIgnoreStatus"/>.
    /// </summary>
    public static Dictionary<Player, RoundIgnoreStatus> PlayerToStatus { get; } = new();

    private static void OnPlayerLeft(PlayerLeftEventArgs ev) => PlayerToStatus.Remove(ev.Player);
}