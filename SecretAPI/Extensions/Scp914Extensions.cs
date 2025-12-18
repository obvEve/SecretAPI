namespace SecretAPI.Extensions
{
    using System.Linq;
    using LabApi.Features.Interfaces;
    using LabApi.Features.Wrappers;
    using Scp914;

    /// <summary>
    /// Extensions related to SCP-914.
    /// </summary>
    public static class Scp914Extensions
    {
        /// <summary>
        /// Process Player like 914 Without Being in 914.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="heldOnly">If it should upgrade only the held item.</param>
        /// <param name="setting">The knob setting.</param>
        public static void Process914Player(this Player player, bool heldOnly, Scp914KnobSetting setting)
        {
            if (heldOnly)
            {
                Process914Item(player.CurrentItem, setting);
                return;
            }

            foreach (Item item in player.Items.ToList())
                Process914Item(item, setting);
        }

        /// <summary>
        /// Processes an item in 914.
        /// </summary>
        /// <param name="item">The item to process.</param>
        /// <param name="setting">The setting to process the item on.</param>
        public static void Process914Item(this Item? item, Scp914KnobSetting setting)
        {
            if (item == null)
                return;

            IScp914ItemProcessor? processor = Scp914.GetItemProcessor(item.Type);
            processor?.UpgradeItem(setting, item);
        }
    }
}