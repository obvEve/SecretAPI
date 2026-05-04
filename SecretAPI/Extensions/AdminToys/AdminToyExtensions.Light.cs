namespace SecretAPI.Extensions.AdminToys;

using LabApi.Features.Wrappers;
using UnityEngine;

/// <summary>
/// Extensions for Light Source Toys.
/// </summary>
public static partial class AdminToyExtensions
{
    extension(LightSourceToy toy)
    {
        /// <summary>
        /// Modify the intensity of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="intensity">The intensity.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithIntensity(float intensity)
        {
            toy.Intensity = intensity;
            return toy;
        }

        /// <summary>
        /// Modify the range of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithRange(float range)
        {
            toy.Range = range;
            return toy;
        }

        /// <summary>
        /// Modify the color of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithColor(Color color)
        {
            toy.Color = color;
            return toy;
        }

        /// <summary>
        /// Modify the shadow type of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="type">The shadow type.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithShadowType(LightShadows type)
        {
            toy.ShadowType = type;
            return toy;
        }

        /// <summary>
        /// Modify the shadow strength of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="strength">The strength.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithShadowStrength(float strength)
        {
            toy.ShadowStrength = strength;
            return toy;
        }

        /// <summary>
        /// Modify the type of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="lightType">The type.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithType(LightType lightType)
        {
            toy.Type = lightType;
            return toy;
        }

        /// <summary>
        /// Modify the shape of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="shape">The shape.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
#pragma warning disable CS0618 // Type or member is obsolete
        public LightSourceToy WithShape(LightShape shape)
#pragma warning restore CS0618 // Type or member is obsolete
        {
            toy.Shape = shape;
            return toy;
        }

        /// <summary>
        /// Modify the spot angle of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithSpotAngle(float angle)
        {
            toy.SpotAngle = angle;
            return toy;
        }

        /// <summary>
        /// Modify the inner spot angle of this <see cref="LightSourceToy"/>.
        /// </summary>
        /// <param name="angle">The angle.</param>
        /// <returns>The modified <see cref="LightSourceToy"/>.</returns>
        public LightSourceToy WithInnerSpotAngle(float angle)
        {
            toy.InnerSpotAngle = angle;
            return toy;
        }
    }
}