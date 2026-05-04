namespace SecretAPI.Extensions.Items;

using InventorySystem.Items.MicroHID.Modules;
using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for Micro H.I.D.
/// </summary>
public static partial class ItemExtensions
{
    extension(MicroHIDItem item)
    {
        /// <summary>
        /// Modify the energy of this <see cref="MicroHIDItem"/>.
        /// </summary>
        /// <param name="energy">The energy.</param>
        /// <returns>The modified <see cref="MicroHIDItem"/>.</returns>
        public MicroHIDItem WithEnergy(float energy)
        {
            item.Energy = energy;
            return item;
        }

        /// <summary>
        /// Modify the broken state of this <see cref="MicroHIDItem"/>.
        /// </summary>
        /// <param name="brokenState">Whether the item is broken or not.</param>
        /// <returns>The modified <see cref="MicroHIDItem"/>.</returns>
        public MicroHIDItem WithBrokenState(bool brokenState)
        {
            item.IsBroken = brokenState;
            return item;
        }

        /// <summary>
        /// Modify the phase of this <see cref="MicroHIDItem"/>.
        /// </summary>
        /// <param name="phase">The phase.</param>
        /// <returns>The modified <see cref="MicroHIDItem"/>.</returns>
        public MicroHIDItem WithPhase(MicroHidPhase phase)
        {
            item.Phase = phase;
            return item;
        }

        /// <summary>
        /// Modify the firing mode of this <see cref="MicroHIDItem"/>.
        /// </summary>
        /// <param name="mode">The firing mode.</param>
        /// <returns>The modified <see cref="MicroHIDItem"/>.</returns>
        public MicroHIDItem WithFiringMode(MicroHidFiringMode mode)
        {
            item.FiringMode = mode;
            return item;
        }
    }
}