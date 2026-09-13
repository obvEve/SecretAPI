#if DEBUG

namespace SecretAPI.Debugging;

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using LabApi.Events.Handlers;
using LabApi.Loader.Features.Paths;
using Mirror;
using SecretAPI.Attributes;
using SecretAPI.Features;
using UnityEngine;
using Logger = LabApi.Features.Console.Logger;

/// <summary>
/// Debugs base-game prefabs by logging information about them.
/// </summary>
internal static class PrefabDebugger
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
        List<string> logTexts = new();
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
            // Logger.Debug(txt);
            string txt = $"Prefab Key ({pair.Key}) - Value ({pair.Value.name})";
            logTexts.Add(txt);
        }

        File.WriteAllLines(Path.Combine(SecretApi.ConfigDirectory.FullName, "debug_prefabs.txt"), logTexts);
    }
}

#endif