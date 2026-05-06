namespace SecretAPI.Extensions.AdminToys;

using System;
using global::AdminToys;
using LabApi.Features.Wrappers;

/// <summary>
/// Extensions for interactable toys.
/// </summary>
public static partial class AdminToyExtensions
{
    extension(InteractableToy toy)
    {
        /// <summary>
        /// Call an <see cref="Action"/> when this <see cref="InteractableToy"/> is interacted with.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to run.</param>
        /// <returns>The modified <see cref="InteractableToy"/>.</returns>
        public InteractableToy WhenInteracted(Action<Player> action)
        {
            toy.OnInteracted += action;
            return toy;
        }

        /// <summary>
        /// Call an <see cref="Action"/> when this <see cref="InteractableToy"/> is being searched.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to run.</param>
        /// <returns>The modified <see cref="InteractableToy"/>.</returns>
        public InteractableToy WhenSearching(Action<Player> action)
        {
            toy.OnSearching += action;
            return toy;
        }

        /// <summary>
        /// Call an <see cref="Action"/> when this <see cref="InteractableToy"/> has been searched.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to run.</param>
        /// <returns>The modified <see cref="InteractableToy"/>.</returns>
        public InteractableToy WhenSearched(Action<Player> action)
        {
            toy.OnSearched += action;
            return toy;
        }

        /// <summary>
        /// Call an <see cref="Action"/> when this <see cref="InteractableToy"/> has had an aborted search.
        /// </summary>
        /// <param name="action">The <see cref="Action"/> to run.</param>
        /// <returns>The modified <see cref="InteractableToy"/>.</returns>
        public InteractableToy WhenSearchAborted(Action<Player> action)
        {
            toy.OnSearchAborted += action;
            return toy;
        }

        /// <summary>
        /// Modify the shape of this <see cref="InteractableToy"/>.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <returns>The modified <see cref="InteractableToy"/>.</returns>
        public InteractableToy WithShape(InvisibleInteractableToy.ColliderShape shape)
        {
            toy.Shape = shape;
            return toy;
        }

        /// <summary>
        /// Modify how long a user should interact with before triggering the <see cref="InteractableToy.OnSearched"/> event.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <returns>The modified <see cref="InteractableToy"/>.</returns>
        public InteractableToy WithInteractionDuration(float duration)
        {
            toy.InteractionDuration = duration;
            return toy;
        }

        /// <summary>
        /// Modify whether this <see cref="InteractableToy"/> can be interacted with.
        /// </summary>
        /// <param name="locked">The lock state.</param>
        /// <returns>The modified <see cref="InteractableToy"/>.</returns>
        public InteractableToy IsLocked(bool locked)
        {
            toy.IsLocked = locked;
            return toy;
        }
    }
}