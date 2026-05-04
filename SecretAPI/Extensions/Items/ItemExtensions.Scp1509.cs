namespace SecretAPI.Extensions.Items;

using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for SCP-1509.
/// </summary>
public static partial class ItemExtensions
{
    extension(Scp1509Item item)
    {
        /// <summary>
        /// Modifies the shield regen rate of this <see cref="Scp1509Item"/>.
        /// </summary>
        /// <param name="rate">The rate.</param>
        /// <returns>The modified <see cref="Scp1509Item"/>.</returns>
        public Scp1509Item WithShieldRegenRate(float rate)
        {
            item.ShieldRegenRate = rate;
            return item;
        }

        /// <summary>
        /// Modifies the shield decay rate of this <see cref="Scp1509Item"/>.
        /// </summary>
        /// <param name="rate">The rate.</param>
        /// <returns>The modified <see cref="Scp1509Item"/>.</returns>
        public Scp1509Item WithShieldDecayRate(float rate)
        {
            item.ShieldDecayRate = rate;
            return item;
        }

        /// <summary>
        /// Modifies the unequip decay delay of this <see cref="Scp1509Item"/>.
        /// </summary>
        /// <param name="time">The time to start decaying.</param>
        /// <returns>The modified <see cref="Scp1509Item"/>.</returns>
        public Scp1509Item WithUnequipDecayDelay(float time)
        {
            item.UnequipDecayDelay = time;
            return item;
        }

        /// <summary>
        /// Modifies the revive cooldown of this <see cref="Scp1509Item"/>.
        /// </summary>
        /// <param name="cooldown">The cooldown.</param>
        /// <returns>The modified <see cref="Scp1509Item"/>.</returns>
        public Scp1509Item WithReviveCooldown(double cooldown)
        {
            item.ReviveCooldown = cooldown;
            return item;
        }

        /// <summary>
        /// Modifies the equipped HS of this <see cref="Scp1509Item"/>.
        /// </summary>
        /// <param name="hs">The hume shield.</param>
        /// <returns>The modified <see cref="Scp1509Item"/>.</returns>
        public Scp1509Item WithEquippedHumeShield(float hs)
        {
            item.EquippedHS = hs;
            return item;
        }
    }
}