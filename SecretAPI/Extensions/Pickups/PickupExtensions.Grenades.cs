namespace SecretAPI.Extensions.Pickups;

using LabApi.Features.Wrappers;
using UnityEngine;

/// <summary>
/// Extensions for grenade projectiles.
/// </summary>
public static partial class PickupExtensions
{
    extension<T>(T pickup)
        where T : TimedGrenadeProjectile
    {
        /// <summary>
        /// Modifies the remaining time of this <see cref="TimedGrenadeProjectile"/>.
        /// </summary>
        /// <param name="time">The time.</param>
        /// <returns>The modified <see cref="TimedGrenadeProjectile"/>.</returns>
        public T WithRemainingTime(double time)
        {
            pickup.RemainingTime = time;
            return pickup;
        }
    }

    extension(ExplosiveGrenadeProjectile pickup)
    {
        /// <summary>
        /// Modifies the max explosion radius of this <see cref="ExplosiveGrenadeProjectile"/>.
        /// </summary>
        /// <param name="radius">The radius.</param>
        /// <returns>The modified <see cref="ExplosiveGrenadeProjectile"/>.</returns>
        public ExplosiveGrenadeProjectile WithMaxRadius(float radius)
        {
            pickup.MaxRadius = radius;
            return pickup;
        }

        /// <summary>
        /// Modifies the scp damage multiplier of this <see cref="ExplosiveGrenadeProjectile"/>.
        /// </summary>
        /// <param name="multiplier">The multiplier.</param>
        /// <returns>The modified <see cref="ExplosiveGrenadeProjectile"/>.</returns>
        public ExplosiveGrenadeProjectile WithScpDamageMultiplier(float multiplier)
        {
            pickup.ScpDamageMultiplier = multiplier;
            return pickup;
        }
    }

    /// <summary>
    /// Modifies the flash time of this <see cref="FlashbangProjectile"/>.
    /// </summary>
    /// <param name="pickup">The flashbang.</param>
    /// <param name="time">The time.</param>
    /// <returns>The modified <see cref="FlashbangProjectile"/>.</returns>
    public static FlashbangProjectile WithBlindTime(this FlashbangProjectile pickup, float time)
    {
        pickup.BaseBlindTime = time;
        return pickup;
    }

    /// <summary>
    /// Modifies the velocity of this <see cref="Scp018Projectile"/>.
    /// </summary>
    /// <param name="pickup">The SCP-018 instance.</param>
    /// <param name="velocity">The velocity.</param>
    /// <returns>The modified <see cref="Scp018Projectile"/>.</returns>
    public static Scp018Projectile WithVelocity(this Scp018Projectile pickup, Vector3 velocity)
    {
        pickup.Velocity = velocity;
        return pickup;
    }

    /// <summary>
    /// Modifies the lockdown duration of this <see cref="Scp2176Projectile"/>.
    /// </summary>
    /// <param name="pickup">The SCP-2176 instance.</param>
    /// <param name="duration">The duration.</param>
    /// <returns>The modified <see cref="Scp2176Projectile"/>.</returns>
    public static Scp2176Projectile WithLockdownDuration(this Scp2176Projectile pickup, float duration)
    {
        pickup.LockdownDuration = duration;
        return pickup;
    }
}