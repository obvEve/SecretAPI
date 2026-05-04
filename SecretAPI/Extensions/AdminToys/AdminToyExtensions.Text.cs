namespace SecretAPI.Extensions.AdminToys;

using LabApi.Features.Wrappers;
using UnityEngine;

/// <summary>
/// Extension methods for text toys.
/// </summary>
public static partial class AdminToyExtensions
{
    extension(TextToy toy)
    {
        /// <summary>
        /// Modify the text of this <see cref="TextToy"/>.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The modified <see cref="TextToy"/>.</returns>
        public TextToy WithText(string text)
        {
            toy.TextFormat = text;
            return toy;
        }

        /// <summary>
        /// Modify the size of this <see cref="TextToy"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The modified <see cref="TextToy"/>.</returns>
        public TextToy WithSize(Vector2 size)
        {
            toy.DisplaySize = size;
            return toy;
        }
    }
}