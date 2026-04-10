namespace SecretAPI.Features;

using System;
using System.Linq;
using AdminToys;
using Interactables.Interobjects;
using MapGeneration;
using Mirror;
using UnityEngine;

/// <summary>
/// Manages prefabs that have variants and cannot be easily used within <see cref="PrefabStore{TPrefab}"/>.
/// </summary>
public static class PrefabManager
{
    private const string LczDoorName = "LCZ BreakableDoor";
    private const string HczDoorName = "HCZ BreakableDoor";
    private const string HczBulkDoorName = "HCZ BulkDoor";
    private const string EzDoorName = "EZ BreakableDoor";

    private const string EzArmCameraToyName = "EzArmCameraToy";
    private const string EzCameraToyName = "EzCameraToy";
    private const string LczCameraToyName = "LczCameraToy";
    private const string HczCameraToyName = "HczCameraToy";
    private const string SzCameraToyName = "SzCameraToy";

    /// <summary>
    /// Gets the <see cref="ReferenceHub"/> prefab.
    /// </summary>
    public static ReferenceHub PlayerPrefab => PrefabStore<ReferenceHub>.Prefab;

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.LightContainment"/>.
    /// </summary>
    public static BasicDoor LczDoorPrefab => field ??= GetOrThrow<BasicDoor>(LczDoorName);

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.HeavyContainment"/>.
    /// </summary>
    public static BasicDoor HczDoorPrefab => field ??= GetOrThrow<BasicDoor>(HczDoorName);

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.HeavyContainment"/>.
    /// </summary>
    public static BasicDoor HczBulkDoorPrefab => field ??= GetOrThrow<BasicDoor>(HczBulkDoorName);

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.Entrance"/>.
    /// </summary>
    public static BasicDoor EzDoorPrefab => field ??= GetOrThrow<BasicDoor>(EzDoorName);

    /// <summary>
    /// Gets the <see cref="Scp079CameraToy"/> found in <see cref="FacilityZone.Entrance"/>, with an arm extension.
    /// </summary>
    public static Scp079CameraToy EzArmCameraToyPrefab => field ??= GetOrThrow<Scp079CameraToy>(EzArmCameraToyName);

    /// <summary>
    /// Gets the <see cref="Scp079CameraToy"/> found in <see cref="FacilityZone.Entrance"/>.
    /// </summary>
    public static Scp079CameraToy EzCameraToyPrefab => field ??= GetOrThrow<Scp079CameraToy>(EzCameraToyName);

    /// <summary>
    /// Gets the <see cref="Scp079CameraToy"/> found in <see cref="FacilityZone.LightContainment"/>.
    /// </summary>
    public static Scp079CameraToy LczCameraToyPrefab => field ??= GetOrThrow<Scp079CameraToy>(LczCameraToyName);

    /// <summary>
    /// Gets the <see cref="Scp079CameraToy"/> found in <see cref="FacilityZone.HeavyContainment"/>.
    /// </summary>
    public static Scp079CameraToy HczCameraToyPrefab => field ??= GetOrThrow<Scp079CameraToy>(HczCameraToyName);

    /// <summary>
    /// Gets the <see cref="Scp079CameraToy"/> found in <see cref="FacilityZone.Surface"/>.
    /// </summary>
    public static Scp079CameraToy SzCameraToyPrefab => field ??= GetOrThrow<Scp079CameraToy>(SzCameraToyName);

    private static T GetOrThrow<T>(string name)
        where T : NetworkBehaviour => PrefabStore<T>.AllComponentPrefabs.FirstOrDefault(c => c.name == name)
                                      ?? throw new InvalidOperationException($"[PrefabManager] Failed to get component ({typeof(T).Name}) by name {name} | Report this as a bug");
}