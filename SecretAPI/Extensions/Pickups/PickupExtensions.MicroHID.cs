namespace SecretAPI.Extensions.Pickups;

using InventorySystem.Items.MicroHID.Modules;
using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for Micro H.I.D.
/// </summary>
public static partial class PickupExtensions
{
    extension(MicroHIDPickup pickup)
    {
        /// <summary>
        /// Modify the energy of this <see cref="MicroHIDPickup"/>.
        /// </summary>
        /// <param name="energy">The energy.</param>
        /// <returns>The modified <see cref="MicroHIDPickup"/>.</returns>
        public MicroHIDPickup WithEnergy(float energy)
        {
            pickup.Energy = energy;
            return pickup;
        }

        /// <summary>
        /// Modify the phase of this <see cref="MicroHIDPickup"/>.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified <see cref="MicroHIDPickup"/>.</returns>
        public MicroHIDPickup WithPhase(MicroHidPhase phase)
        {
            pickup.Phase = phase;
            return pickup;
        }

        /// <summary>
        /// Modify the firing mode of this <see cref="MicroHIDPickup"/>.
        /// </summary>
        /// <param name="mode">The firing mode.</param>
        /// <returns>The modified <see cref="MicroHIDPickup"/>.</returns>
        public MicroHIDPickup WithFiringMode(MicroHidFiringMode mode)
        {
            pickup.FiringMode = mode;
            return pickup;
        }
    }
}