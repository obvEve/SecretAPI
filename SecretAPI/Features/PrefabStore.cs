namespace SecretAPI.Features;

using System.Collections.Generic;
using System.Linq;
using Interactables.Interobjects;
using Mirror;
using NorthwoodLib.Pools;
using UnityEngine;

/// <summary>
/// Handles the storing of a prefab.
/// </summary>
/// <typeparam name="TPrefab">The prefab to use.</typeparam>
/// <remarks>For Doors use <see cref="PrefabManager"/>.</remarks>
public static class PrefabStore<TPrefab>
    where TPrefab : NetworkBehaviour
{
    /// <summary>
    /// Gets the first prefab found of the specified type.
    /// </summary>
    public static TPrefab Prefab
    {
        get
        {
            if (field)
                return field;

            if (typeof(TPrefab) == typeof(ReferenceHub))
                return field = NetworkManager.singleton.playerPrefab.GetComponent<TPrefab>();

            return field = AllComponentPrefabs.FirstOrDefault()!;
        }
    }

    /// <summary>
    /// Gets every single prefab associated with this component.
    /// </summary>
    /// <remarks>Used to find all of a base type (such as <see cref="BasicDoor"/>).</remarks>
    public static TPrefab[] AllComponentPrefabs
    {
        get
        {
            if (field != null)
                return field;

            List<TPrefab> allPrefabs = ListPool<TPrefab>.Shared.Rent();

            foreach (GameObject gameObject in NetworkClient.prefabs.Values)
            {
                if (gameObject.TryGetComponent(out TPrefab prefab))
                    allPrefabs.Add(prefab);
            }

            field = allPrefabs.ToArray();
            ListPool<TPrefab>.Shared.Return(allPrefabs);

            return field;
        }
    }
}