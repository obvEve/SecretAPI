namespace SecretAPI.Features.Effects
{
    using System;
    using System.Collections.Generic;
    using CustomPlayerEffects;
    using LabApi.Features.Wrappers;
    using SecretAPI.Attribute;
    using SecretAPI.Extensions;
    using UnityEngine;
    using UnityEngine.SceneManagement;
    using Logger = LabApi.Features.Console.Logger;

    /// <summary>
    /// Handles custom player effects.
    /// <remarks>Must register to <see cref="EffectsToRegister"/> to work.</remarks>
    /// </summary>
    public abstract class CustomPlayerEffect : StatusEffectBase
    {
        private static bool isLoaded;

        /// <summary>
        /// Gets a list of types to register (Must inherit <see cref="StatusEffectBase"/>).
        /// <remarks>Must be <see cref="Type"/>, can be gotten through <code>typeof(Scp207)</code></remarks>
        /// </summary>
        public static List<Type> EffectsToRegister { get; } = [];

        /// <summary>
        /// Gets the <see cref="Player"/> with this effect.
        /// </summary>
        public Player Owner { get; private set; } = null!;

        /// <inheritdoc/>
        public override void Start()
        {
            Owner = Player.Get(Hub);
            base.Start();
        }

        /// <inheritdoc/>
        public override string ToString() => $"{GetType().Name}: Owner ({Owner}) - Intensity ({Intensity}) - Duration {Duration}";

        /// <summary>
        /// Initializes the <see cref="CustomPlayerEffect"/> to implement <see cref="EffectsToRegister"/>.
        /// </summary>
        [CallOnLoad]
        internal static void Initialize()
        {
            SecretApi.Harmony?.PatchCategory(nameof(CustomPlayerEffect), SecretApi.Assembly);
            EffectsToRegister.Add(typeof(TemporaryDamageImmunity));
            EffectsToRegister.Add(typeof(StaminaUsageDisablerEffect));
            EffectsToRegister.Add(typeof(SprintDisablerEffect));

            SceneManager.sceneLoaded += (_, _) =>
            {
                if (isLoaded)
                    return;

                isLoaded = true;

                Transform playerEffects = PrefabStore<ReferenceHub>.Prefab.playerEffectsController.effectsGameObject.transform;
                foreach (Type type in EffectsToRegister)
                {
                    if (!typeof(StatusEffectBase).IsAssignableFrom(type))
                    {
                        Logger.Error($"[CustomPlayerEffect.Initialize] {type.FullName} is not a valid StatusEffectBase and thus could not be registered!");
                        continue;
                    }

                    // register effect into prefab
                    new GameObject(type.Name, type).transform.parent = playerEffects;
                }
            };
        }
    }
}