namespace SecretAPI.Extensions;

using System;
using AdminToys;
using LabApi.Features.Wrappers;
using UnityEngine;
using CapybaraToy = LabApi.Features.Wrappers.CapybaraToy;
using PrimitiveObjectToy = LabApi.Features.Wrappers.PrimitiveObjectToy;
using TextToy = LabApi.Features.Wrappers.TextToy;
using WaypointToy = LabApi.Features.Wrappers.WaypointToy;

public static partial class AdminToyExtensions
{
    extension<T>(T toy)
        where T : AdminToy
    {
        public T WithPosition(Vector3 pos)
        {
            toy.Position = pos;
            return toy;
        }

        public T WithRotation(Quaternion rot)
        {
            toy.Rotation = rot;
            return toy;
        }

        public T WithScale(Vector3 scale)
        {
            toy.Scale = scale;
            return toy;
        }
    }

    extension(CameraToy toy)
    {
        public CameraToy WithLabel(string label)
        {
            toy.Label = label;
            return toy;
        }

        public CameraToy WithRoom(Room room)
        {
            toy.Room = room;
            return toy;
        }

        public CameraToy WithVerticalConstraints(Vector2 constraint)
        {
            toy.VerticalConstraints = constraint;
            return toy;
        }

        public CameraToy WithHorizontalConstraints(Vector2 constraint)
        {
            toy.HorizontalConstraint = constraint;
            return toy;
        }

        public CameraToy WithZoomConstraints(Vector2 constraint)
        {
            toy.ZoomConstraints = constraint;
            return toy;
        }
    }

    public static CapybaraToy WithCollidersEnabled(this CapybaraToy toy, bool enabled)
    {
        toy.CollidersEnabled = enabled;
        return toy;
    }

    extension(InteractableToy toy)
    {
        public InteractableToy WhenInteracted(Action<Player> action)
        {
            toy.OnInteracted += action;
            return toy;
        }

        public InteractableToy WhenSearching(Action<Player> action)
        {
            toy.OnSearching += action;
            return toy;
        }

        public InteractableToy WhenSearched(Action<Player> action)
        {
            toy.OnSearched += action;
            return toy;
        }

        public InteractableToy WhenSearchAborted(Action<Player> action)
        {
            toy.OnSearchAborted += action;
            return toy;
        }

        public InteractableToy WithShape(InvisibleInteractableToy.ColliderShape shape)
        {
            toy.Shape = shape;
            return toy;
        }

        public InteractableToy WithInteractionDuration(float duration)
        {
            toy.InteractionDuration = duration;
            return toy;
        }

        public InteractableToy IsLocked(bool locked)
        {
            toy.IsLocked = locked;
            return toy;
        }
    }

    extension(PrimitiveObjectToy toy)
    {
        public bool Collidable
        {
            get => (toy.Flags & PrimitiveFlags.Collidable) == PrimitiveFlags.Collidable;
            set => toy.Flags = value ? toy.Flags | PrimitiveFlags.Collidable : toy.Flags & ~PrimitiveFlags.Collidable;
        }

        public bool Visible
        {
            get => (toy.Flags & PrimitiveFlags.Visible) == PrimitiveFlags.Visible;
            set => toy.Flags = value ? toy.Flags | PrimitiveFlags.Visible : toy.Flags & ~PrimitiveFlags.Visible;
        }

        public PrimitiveObjectToy WithCollidable(bool collidable)
        {
            toy.Collidable = collidable;
            return toy;
        }

        public PrimitiveObjectToy WithVisible(bool visible)
        {
            toy.Visible = visible;
            return toy;
        }

        public PrimitiveObjectToy WithType(PrimitiveType type)
        {
            toy.Type = type;
            return toy;
        }

        public PrimitiveObjectToy WithColor(Color color)
        {
            toy.Color = color;
            return toy;
        }

        public PrimitiveObjectToy WithFlags(PrimitiveFlags flags)
        {
            toy.Flags = flags;
            return toy;
        }
    }

    extension(TextToy toy)
    {
        public TextToy WithText(string text)
        {
            toy.TextFormat = text;
            return toy;
        }

        public TextToy WithSize(Vector2 size)
        {
            toy.DisplaySize = size;
            return toy;
        }
    }

    extension(WaypointToy toy)
    {
        public WaypointToy WithBoundsSize(Vector3 size)
        {
            toy.BoundsSize = size;
            return toy;
        }

        public WaypointToy WithVisualiseBounds(bool visible)
        {
            toy.VisualizeBounds = visible;
            return toy;
        }

        public WaypointToy WithPriorityBias(float bias)
        {
            toy.PriorityBias = bias;
            return toy;
        }
    }
}