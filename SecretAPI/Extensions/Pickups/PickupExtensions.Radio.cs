namespace SecretAPI.Extensions.Pickups;

using InventorySystem.Items.Radio;
using RadioPickup = LabApi.Features.Wrappers.RadioPickup;

/// <summary>
/// Extensions for radios.
/// </summary>
public static partial class PickupExtensions
{
    extension(RadioPickup pickup)
    {
        /// <summary>
        /// Modifies the enabled state of this <see cref="RadioPickup"/>.
        /// </summary>
        /// <param name="enabled">The enabled state.</param>
        /// <returns>The modified <see cref="RadioPickup"/>.</returns>
        public RadioPickup WithEnabledState(bool enabled)
        {
            pickup.IsEnabled = enabled;
            return pickup;
        }

        /// <summary>
        /// Modifies the range level of this <see cref="RadioPickup"/>.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>The modified <see cref="RadioPickup"/>.</returns>
        public RadioPickup WithRangeLevel(RadioMessages.RadioRangeLevel range)
        {
            pickup.RangeLevel = range;
            return pickup;
        }

        /// <summary>
        /// Modifies the battery of this <see cref="RadioPickup"/>.
        /// </summary>
        /// <param name="battery">The battery.</param>
        /// <returns>The modified <see cref="RadioPickup"/>.</returns>
        public RadioPickup WithBattery(float battery)
        {
            pickup.Battery = battery;
            return pickup;
        }
    }
}