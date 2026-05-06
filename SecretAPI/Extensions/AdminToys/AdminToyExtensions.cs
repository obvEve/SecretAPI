namespace SecretAPI.Extensions.AdminToys;

using LabApi.Features.Wrappers;
using UnityEngine;
using CapybaraToy = LabApi.Features.Wrappers.CapybaraToy;

/// <summary>
/// Extensions for admin toys.
/// </summary>
public static partial class AdminToyExtensions
{
    extension<T>(T toy)
        where T : AdminToy
    {
        /// <summary>
        /// Modify the position of this <see cref="AdminToy"/>.
        /// </summary>
        /// <param name="pos">The position to change to.</param>
        /// <returns>The modified <see cref="AdminToy"/>.</returns>
        public T WithPosition(Vector3 pos)
        {
            toy.Position = pos;
            return toy;
        }

        /// <summary>
        /// Modify the rotation of this <see cref="AdminToy"/>.
        /// </summary>
        /// <param name="rot">The rotation to change to.</param>
        /// <returns>The modified <see cref="AdminToy"/>.</returns>
        public T WithRotation(Quaternion rot)
        {
            toy.Rotation = rot;
            return toy;
        }

        /// <summary>
        /// Modify the scale of this <see cref="AdminToy"/>.
        /// </summary>
        /// <param name="scale">The scale to change to.</param>
        /// <returns>The modified <see cref="AdminToy"/>.</returns>
        public T WithScale(Vector3 scale)
        {
            toy.Scale = scale;
            return toy;
        }
    }

    /// <summary>
    /// Updates whether the capybara has enabled colliders.
    /// </summary>
    /// <param name="toy">The <see cref="CapybaraToy"/>.</param>
    /// <param name="enabled">Whether the colliders are enabled.</param>
    /// <returns>The modified <see cref="CapybaraToy"/>.</returns>
    public static CapybaraToy WithCollidersEnabled(this CapybaraToy toy, bool enabled)
    {
        toy.CollidersEnabled = enabled;
        return toy;
    }
}