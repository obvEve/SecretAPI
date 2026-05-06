namespace SecretAPI.Extensions.AdminToys;

using LabApi.Features.Wrappers;
using UnityEngine;

/// <summary>
/// Extension methods for waypoint toys.
/// </summary>
public static partial class AdminToyExtensions
{
    extension(WaypointToy toy)
    {
        /// <summary>
        /// Modify the bounds size of this <see cref="WaypointToy"/>.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <returns>The modified <see cref="WaypointToy"/>.</returns>
        public WaypointToy WithBoundsSize(Vector3 size)
        {
            toy.BoundsSize = size;
            return toy;
        }

        /// <summary>
        /// Modify whether this <see cref="WaypointToy"/> has visible bounds.
        /// </summary>
        /// <param name="visible">The visibility state.</param>
        /// <returns>The modified <see cref="WaypointToy"/>.</returns>
        public WaypointToy WithVisualiseBounds(bool visible)
        {
            toy.VisualizeBounds = visible;
            return toy;
        }

        /// <summary>
        /// Modify the priority bias of this <see cref="WaypointToy"/>.
        /// </summary>
        /// <param name="bias">The bias.</param>
        /// <returns>The modified <see cref="WaypointToy"/>.</returns>
        public WaypointToy WithPriorityBias(float bias)
        {
            toy.PriorityBias = bias;
            return toy;
        }
    }
}