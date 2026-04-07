namespace SecretAPI.Extensions;

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using SecretAPI.Patches.Features;
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="value"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static bool TryGetRandomWeighted<T>(this IEnumerable<(T Item, float Weight)> collection, out T? value)
    {
        (T Item, float Weight)[] array = collection.ToArray();
        float totalWeight = array.Sum(item => item.Weight);
        float random = Random.Range(0f, totalWeight);

        foreach ((T item, float weight) in array)
        {
            random -= weight;
            if (random <= 0f)
            {
                value = item;
                return true;
            }
        }

        value = default;
        return false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T GetRandomWeighted<T>(this IEnumerable<(T Item, float Weight)> collection)
    {
        TryGetRandomWeighted(collection, out T? value);
        return value ?? throw new InvalidOperationException("[CollectionExtensions.GetRandomWeighted] Could not find item - Was the collection perhaps empty?");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collection"></param>
    /// <param name="weightAssigner"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static IEnumerable<(T Item, float Weight)> GetWeights<T>(this IEnumerable<T> collection, Func<T, float> weightAssigner)
        => collection.Select(item => (item, weightAssigner(item)));
}