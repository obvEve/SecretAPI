namespace SecretAPI.Extensions.Items;

using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for firearms.
/// </summary>
public static partial class ItemExtensions
{
    extension<T>(T item)
        where T : FirearmItem
    {
        /// <summary>
        /// Modifies the stored ammo in this <see cref="FirearmItem"/>.
        /// </summary>
        /// <param name="ammo">The ammo.</param>
        /// <returns>The modified <see cref="FirearmItem"/>.</returns>
        public T WithStoredAmmo(int ammo)
        {
            item.StoredAmmo = ammo;
            return item;
        }

        /// <summary>
        /// Modifies whether the magazine is inserted in this <see cref="FirearmItem"/>.
        /// </summary>
        /// <param name="magazineInserted">Whether the magazine is inserted.</param>
        /// <returns>The modified <see cref="FirearmItem"/>.</returns>
        public T WithMagazineInserted(bool magazineInserted)
        {
            item.MagazineInserted = magazineInserted;
            return item;
        }

        /// <summary>
        /// Modifies whether the firearm is cocked in this <see cref="FirearmItem"/>.
        /// </summary>
        /// <param name="cocked">Whether the firearm is cocked.</param>
        /// <returns>The modified <see cref="FirearmItem"/>.</returns>
        public T WithCocked(bool cocked)
        {
            item.Cocked = cocked;
            return item;
        }

        /// <summary>
        /// Modifies the chambered ammo in this <see cref="FirearmItem"/>.
        /// </summary>
        /// <param name="ammo">The ammo.</param>
        /// <returns>The modified <see cref="FirearmItem"/>.</returns>
        public T WithChamberedAmmo(int ammo)
        {
            item.ChamberedAmmo = ammo;
            return item;
        }
    }
}