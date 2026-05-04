namespace SecretAPI.Extensions.Pickups;

using InventorySystem.Items.Jailbird;
using JailbirdPickup = LabApi.Features.Wrappers.JailbirdPickup;

/// <summary>
/// Extensions for jailbirds.
/// </summary>
public static partial class PickupExtensions
{
    extension(JailbirdPickup pickup)
    {
        /// <summary>
        /// Modifies the total damage dealt of this <see cref="JailbirdPickup"/>.
        /// </summary>
        /// <param name="damage">The damage.</param>
        /// <returns>The modified <see cref="JailbirdPickup"/>.</returns>
        public JailbirdPickup WithTotalDamageDealt(float damage)
        {
            pickup.TotalDamageDealt += damage;
            return pickup;
        }

        /// <summary>
        /// Modifies the total charges performed of this <see cref="JailbirdPickup"/>.
        /// </summary>
        /// <param name="charges">The charges.</param>
        /// <returns>The modified <see cref="JailbirdPickup"/>.</returns>
        public JailbirdPickup WithTotalChargesPerformed(int charges)
        {
            pickup.TotalChargesPerformed += charges;
            return pickup;
        }

        /// <summary>
        /// Modifies the wear state of this <see cref="JailbirdPickup"/>.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>The modified <see cref="JailbirdPickup"/>.</returns>
        public JailbirdPickup WithWearState(JailbirdWearState state)
        {
            pickup.WearState = state;
            return pickup;
        }
    }
}