#if DEBUG

namespace SecretAPI.Debugging;

using System;
using System.Collections.Generic;
using System.Reflection;
using LabApi.Events.Handlers;
using Mirror;
using SecretAPI.Attributes;
using SecretAPI.Features;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

/// <summary>
/// Debugs basegame prefabs by logging information about them.
/// </summary>
internal static class PrefabDebugging
{
    /// <summary>
    /// Loads the prefab debugging.
    /// </summary>
    [CallOnLoad]
    internal static void Load()
    {
        ServerEvents.WaitingForPlayers += OnWaiting;
    }

    private static void OnWaiting()
    {
        foreach (PropertyInfo properties in typeof(PrefabManager).GetProperties())
        {
            try
            {
                if (properties.GetValue(null) == null)
                    Logger.Error($"[PrefabDebugging] {properties.Name} returned a null value!");
            }
            catch (Exception ex)
            {
                Logger.Error($"[PrefabDebugging] {properties.Name} ran into an exception: {ex}");
            }
        }

        foreach (KeyValuePair<uint, GameObject> pair in NetworkClient.prefabs)
        {
            Logger.Debug($"[PrefabDebugging] Key ({pair.Key}) - Value ({pair.Value.name})");
        }
    }
}

#endif