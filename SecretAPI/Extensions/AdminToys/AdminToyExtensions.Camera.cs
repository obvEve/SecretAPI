namespace SecretAPI.Extensions.AdminToys;

using LabApi.Features.Wrappers;
using UnityEngine;

/// <summary>
/// Extensions for camera toys.
/// </summary>
public static partial class AdminToyExtensions
{
    extension(CameraToy toy)
    {
        /// <summary>
        /// Modify the label of this <see cref="CameraToy"/>.
        /// </summary>
        /// <param name="label">The label.</param>
        /// <returns>The modified <see cref="CameraToy"/>.</returns>
        public CameraToy WithLabel(string label)
        {
            toy.Label = label;
            return toy;
        }

        /// <summary>
        /// Modify the room of this <see cref="CameraToy"/>.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <returns>The modified <see cref="CameraToy"/>.</returns>
        public CameraToy WithRoom(Room room)
        {
            toy.Room = room;
            return toy;
        }

        /// <summary>
        /// Modify the vertical constrains of this <see cref="CameraToy"/>.
        /// </summary>
        /// <param name="constraint">The constraints.</param>
        /// <returns>The modified <see cref="CameraToy"/>.</returns>
        public CameraToy WithVerticalConstraints(Vector2 constraint)
        {
            toy.VerticalConstraints = constraint;
            return toy;
        }

        /// <summary>
        /// Modify the horizontal constrains of this <see cref="CameraToy"/>.
        /// </summary>
        /// <param name="constraint">The constraints.</param>
        /// <returns>The modified <see cref="CameraToy"/>.</returns>
        public CameraToy WithHorizontalConstraints(Vector2 constraint)
        {
            toy.HorizontalConstraint = constraint;
            return toy;
        }

        /// <summary>
        /// Modify the zoom constrains of this <see cref="CameraToy"/>.
        /// </summary>
        /// <param name="constraint">The constraints.</param>
        /// <returns>The modified <see cref="CameraToy"/>.</returns>
        public CameraToy WithZoomConstraints(Vector2 constraint)
        {
            toy.ZoomConstraints = constraint;
            return toy;
        }
    }
}