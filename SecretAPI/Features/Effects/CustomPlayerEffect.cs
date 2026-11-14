namespace SecretAPI.Features.Effects;

using System;
using System.Collections.Generic;
using CustomPlayerEffects;
using LabApi.Features.Wrappers;
using SecretAPI.Attributes;
using UnityEngine;
using UnityEngine.SceneManagement;

using Logger = LabApi.Features.Console.Logger;

// ! PLEASE NW MAKE CUSTOM EFFECTS GOOD I'M GONNA CRY

/// <summary>
/// Handles custom player effects.
/// <remarks>Must register with <see cref="Register"/> in order to work.</remarks>
/// </summary>
public abstract class CustomPlayerEffect : StatusEffectBase
{
    private static HashSet<Type> registeredEffects = new();

    /// <summary>
    /// Gets a readonly collection of all the registered effects.
    /// </summary>
    public static IReadOnlyCollection<Type> RegisteredEffects => registeredEffects;

    /// <summary>
    /// Gets the <see cref="Player"/> with this effect.
    /// </summary>
    public Player Owner => field ??= Player.Get(Hub);

    /// <summary>
    /// Registers a custom effect type.
    /// </summary>
    /// <typeparam name="T">The effect type to register.</typeparam>
    public static void Register<T>()
        where T : StatusEffectBase => Register(typeof(T));

    /// <summary>
    /// Registers a custom effect type.
    /// </summary>
    /// <param name="type">The <see cref="Type"/> to register. May not be abstract and must inherit <see cref="StatusEffectBase"/>.</param>
    public static void Register(Type type)
    {
        if (type.IsAbstract)
        {
            Logger.Error($"[CustomPlayerEffect.Register] {type.FullName} is abstract and cannot be registered!");
            return;
        }

        if (!typeof(StatusEffectBase).IsAssignableFrom(type))
        {
            Logger.Error($"[CustomPlayerEffect.Register] {type.FullName} does not inherit StatusEffectBase!");
            return;
        }

        if (!registeredEffects.Add(type))
        {
            Logger.Error($"[CustomPlayerEffect.Register] {type.FullName} is already registered!");
            return;
        }
    }

    /// <inheritdoc/>
    public override string ToString() => $"{GetType().Name}: Owner ({Owner}) - Intensity ({Intensity}) - Duration {Duration}";

    /// <summary>
    /// Initializes the <see cref="CustomPlayerEffect"/> to implement <see cref="RegisteredEffects"/>.
    /// </summary>
    /// <remarks>The effects must be loaded on before <see cref="Server.Host"/> is created.</remarks>
    [CallOnLoad]
    internal static void Initialize()
    {
        Register<Energized>();
        Register<Depleted>();
        Register<BlastResistance>();

        // it would probably be better to patch PlayerEffectsController::Awake instead of using sceneLoaded
        // this would also allow registering after players join
        // although the current way might be the fastest since its just base unity handling it on the prefab
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        Transform playerEffects = PrefabStore<ReferenceHub>.Prefab.playerEffectsController.effectsGameObject.transform;
        foreach (Type type in RegisteredEffects)
        {
            if (!typeof(StatusEffectBase).IsAssignableFrom(type))
            {
                Logger.Error($"[CustomPlayerEffect.OnSceneLoaded] {type.FullName} is not a valid StatusEffectBase and thus could not be registered!");
                continue;
            }

            // register effect into prefab
            new GameObject(type.Name, type).transform.parent = playerEffects;
        }
    }
}