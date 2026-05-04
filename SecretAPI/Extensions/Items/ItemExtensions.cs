namespace SecretAPI.Extensions.Items;

using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for items.
/// </summary>
public static partial class ItemExtensions
{
    extension(Item item)
    {
        /// <summary>
        /// Change this <see cref="Item"/> into a different item type, useful for the other extension methods that can be used.
        /// </summary>
        /// <typeparam name="T">The item type you want to change type to.</typeparam>
        /// <returns>The changed item type.</returns>
        public T As<T>()
            where T : Item
        {
            return (T)item;
        }
    }

    /// <summary>
    /// Modifies the battery percentage of this <see cref="RadioItem"/>.
    /// </summary>
    /// <param name="item">The <see cref="RadioItem"/>.</param>
    /// <param name="battery">The percentage.</param>
    /// <returns>The modified <see cref="RadioItem"/>.</returns>
    public static RadioItem WithBatteryPercent(this RadioItem item, byte battery)
    {
        item.BatteryPercent = battery;
        return item;
    }
}