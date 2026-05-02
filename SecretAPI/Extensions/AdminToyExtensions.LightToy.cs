namespace SecretAPI.Extensions;

using LabApi.Features.Wrappers;
using UnityEngine;

public static partial class AdminToyExtensions
{
    extension(LightSourceToy toy)
    {
        public LightSourceToy WithIntensity(float intensity)
        {
            toy.Intensity = intensity;
            return toy;
        }

        public LightSourceToy WithRange(float range)
        {
            toy.Range = range;
            return toy;
        }

        public LightSourceToy WithColor(Color color)
        {
            toy.Color = color;
            return toy;
        }

        public LightSourceToy WithShadowType(LightShadows type)
        {
            toy.ShadowType = type;
            return toy;
        }

        public LightSourceToy WithShadowStrength(float strength)
        {
            toy.ShadowStrength = strength;
            return toy;
        }

        public LightSourceToy WithType(LightType lightType)
        {
            toy.Type = lightType;
            return toy;
        }

#pragma warning disable CS0618 // Type or member is obsolete
        public LightSourceToy WithShape(LightShape shape)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            toy.Shape = shape;
            return toy;
        }

        public LightSourceToy WithSpotAngle(float angle)
        {
            toy.SpotAngle = angle;
            return toy;
        }

        public LightSourceToy WithInnerSpotAngle(float angle)
        {
            toy.InnerSpotAngle = angle;
            return toy;
        }
    }
}