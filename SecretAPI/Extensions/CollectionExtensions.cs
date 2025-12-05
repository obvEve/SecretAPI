namespace SecretAPI.Extensions
{
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
        /// <summary>
        /// Gets a random value from the collection.
        /// </summary>
        /// <param name="collection">The collection to pull from.</param>
        /// <typeparam name="T">The Type contained by the collection.</typeparam>
        /// <returns>A random value, default value when empty collection.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Will occur if the collection is empty.</exception>
        public static T GetRandomValue<T>(this IEnumerable<T> collection)
        {
            TryGetRandomValue(collection, out T? value);
            return value!;
        }

        /// <summary>
        /// Tries to get a random value from <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="collection">The <see cref="IEnumerable{T}"/> to try and get a random value from.</param>
        /// <param name="value">The value that was found. Default if none could be found.</param>
        /// <typeparam name="T">The type contained within the <see cref="IEnumerable{T}"/>.</typeparam>
        /// <returns>Whether a non-null value was found.</returns>
        public static bool TryGetRandomValue<T>(this IEnumerable<T> collection, [NotNullWhen(true)] out T? value)
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