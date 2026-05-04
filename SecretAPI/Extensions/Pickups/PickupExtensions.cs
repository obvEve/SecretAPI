namespace SecretAPI.Extensions.Pickups;

using InventorySystem.Items.Usables.Scp330;
using LabApi.Features.Wrappers;
using Scp330Pickup = LabApi.Features.Wrappers.Scp330Pickup;

/// <summary>
/// Extensions for pickups.
/// </summary>
public static partial class PickupExtensions
{
    extension<T>(T pickup)
        where T : Pickup
    {
        /// <summary>
        /// Modifies the weight of this <see cref="Pickup"/>.
        /// </summary>
        /// <param name="weight">The weight.</param>
        /// <returns>The modified <see cref="Pickup"/>.</returns>
        public T WithWeight(float weight)
        {
            pickup.Weight = weight;
            return pickup;
        }

        /// <summary>
        /// Modifies the locked state of this <see cref="Pickup"/>.
        /// </summary>
        /// <param name="locked">The lock state.</param>
        /// <returns>The modified <see cref="Pickup"/>.</returns>
        public T WithLockedState(bool locked)
        {
            pickup.IsLocked = locked;
            return pickup;
        }
    }

    /// <summary>
    /// Modifies the ammo in this <see cref="AmmoPickup"/>.
    /// </summary>
    /// <param name="pickup">The pickup.</param>
    /// <param name="ammo">The ammo.</param>
    /// <returns>The modified <see cref="AmmoPickup"/>.</returns>
    public static AmmoPickup WithAmmo(this AmmoPickup pickup, ushort ammo)
    {
        pickup.Ammo = ammo;
        return pickup;
    }

    /// <summary>
    /// Modifies the exposed candy in this <see cref="Scp330Pickup"/>.
    /// </summary>
    /// <param name="pickup">The pickup.</param>
    /// <param name="kind">The candy kind.</param>
    /// <returns>The modified <see cref="Scp330Pickup"/>.</returns>
    public static Scp330Pickup WithExposedCandy(this Scp330Pickup pickup, CandyKindID kind)
    {
        pickup.ExposedCandy = kind;
        return pickup;
    }
}