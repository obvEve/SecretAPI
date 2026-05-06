namespace SecretAPI.Extensions.AdminToys;

using global::AdminToys;
using UnityEngine;
using PrimitiveObjectToy = LabApi.Features.Wrappers.PrimitiveObjectToy;

/// <summary>
/// Extensions for primitive object toys.
/// </summary>
public static partial class AdminToyExtensions
{
    extension(PrimitiveObjectToy toy)
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PrimitiveObjectToy"/> is collidable.
        /// </summary>
        public bool Collidable
        {
            get => (toy.Flags & PrimitiveFlags.Collidable) == PrimitiveFlags.Collidable;
            set => toy.Flags = value ? toy.Flags | PrimitiveFlags.Collidable : toy.Flags & ~PrimitiveFlags.Collidable;
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="PrimitiveObjectToy"/> is visible.
        /// </summary>
        public bool Visible
        {
            get => (toy.Flags & PrimitiveFlags.Visible) == PrimitiveFlags.Visible;
            set => toy.Flags = value ? toy.Flags | PrimitiveFlags.Visible : toy.Flags & ~PrimitiveFlags.Visible;
        }

        /// <summary>
        /// Modify whether this <see cref="PrimitiveObjectToy"/> is collidable.
        /// </summary>
        /// <param name="collidable">The collidable state.</param>
        /// <returns>The modified <see cref="PrimitiveObjectToy"/>.</returns>
        public PrimitiveObjectToy WithCollidable(bool collidable)
        {
            toy.Collidable = collidable;
            return toy;
        }

        /// <summary>
        /// Modify whether this <see cref="PrimitiveObjectToy"/> is visible.
        /// </summary>
        /// <param name="visible">The visibility state.</param>
        /// <returns>The modified <see cref="PrimitiveObjectToy"/>.</returns>
        public PrimitiveObjectToy WithVisible(bool visible)
        {
            toy.Visible = visible;
            return toy;
        }

        /// <summary>
        /// Modify the type of this <see cref="PrimitiveObjectToy"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The modified <see cref="PrimitiveObjectToy"/>.</returns>
        public PrimitiveObjectToy WithType(PrimitiveType type)
        {
            toy.Type = type;
            return toy;
        }

        /// <summary>
        /// Modify the color of this <see cref="PrimitiveObjectToy"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The modified <see cref="PrimitiveObjectToy"/>.</returns>
        public PrimitiveObjectToy WithColor(Color color)
        {
            toy.Color = color;
            return toy;
        }

        /// <summary>
        /// Modify the flags of this <see cref="PrimitiveObjectToy"/>.
        /// </summary>
        /// <param name="flags">The flags.</param>
        /// <returns>The modified <see cref="PrimitiveObjectToy"/>.</returns>
        public PrimitiveObjectToy WithFlags(PrimitiveFlags flags)
        {
            toy.Flags = flags;
            return toy;
        }
    }
}