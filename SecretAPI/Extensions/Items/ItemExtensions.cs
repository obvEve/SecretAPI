namespace SecretAPI.Extensions.Items;

using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for items.
/// </summary>
public static partial class ItemExtensions
{
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