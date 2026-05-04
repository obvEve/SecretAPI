namespace SecretAPI.Extensions.Items;

using InventorySystem.Items.Firearms.Modules.Scp127;
using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for SCP-127.
/// </summary>
public static partial class ItemExtensions
{
    extension(Scp127Firearm item)
    {
        /// <summary>
        /// Modifies the tier of this <see cref="Scp127Firearm"/>.
        /// </summary>
        /// <param name="tier">The tier.</param>
        /// <returns>The modified <see cref="Scp127Firearm"/>.</returns>
        public Scp127Firearm WithTier(Scp127Tier tier)
        {
            item.Tier = tier;
            return item;
        }

        /// <summary>
        /// Modifies the experience of this <see cref="Scp127Firearm"/>.
        /// </summary>
        /// <param name="experience">The experience.</param>
        /// <returns>The modified <see cref="Scp127Firearm"/>.</returns>
        public Scp127Firearm WithExperience(float experience)
        {
            item.Experience = experience;
            return item;
        }
    }
}