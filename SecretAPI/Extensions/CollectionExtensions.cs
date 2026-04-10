namespace SecretAPI.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Random = UnityEngine.Random;

/// <summary>
/// Extensions for collections.
/// </summary>
public static class CollectionExtensions
{
    /// <param name="collection">The collection to pull from.</param>
    /// <typeparam name="T">The Type contained by the collection.</typeparam>
    extension<T>(IEnumerable<T> collection)
    {
        /// <summary>
        /// Gets a random value from the collection.
        /// </summary>
        /// <returns>A random value, default value when empty collection.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Will occur if the collection is empty.</exception>
        /// <remarks>Use <see cref="TryGetRandomValue"/> unless you have already validated and are sure the collection is not empty.</remarks>
        public T GetRandomValue()
        {
            TryGetRandomValue(collection, out T? value);
            return value!;
        }

        /// <summary>
        /// Tries to get a random value from <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="value">The value that was found. Default if none could be found.</param>
        /// <returns>Whether a non-null value was found.</returns>
        public bool TryGetRandomValue([NotNullWhen(true)] out T? value)
        {
            IList<T> list = collection as IList<T> ?? collection.ToList();
            if (list.Count == 0)
            {
                value = default;
                return false;
            }

            value = list[Random.Range(0, list.Count)];
            return value != null;
        }
    }
}