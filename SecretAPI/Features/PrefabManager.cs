namespace SecretAPI.Features;

using System;
using System.Linq;
using Interactables.Interobjects;
using MapGeneration;

/// <summary>
/// Manages prefabs that have variants and cannot be easily used within <see cref="PrefabStore{TPrefab}"/>.
/// </summary>
public static class PrefabManager
{
    private const string LczDoorName = "LCZ BreakableDoor";
    private const string HczDoorName = "HCZ BreakableDoor";
    private const string HczBulkDoorName = "HCZ BulkDoor";
    private const string EzDoorName = "EZ BreakableDoor";

    /// <summary>
    /// Gets the <see cref="ReferenceHub"/> prefab.
    /// </summary>
    public static ReferenceHub PlayerPrefab => PrefabStore<ReferenceHub>.Prefab;

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.LightContainment"/>.
    /// </summary>
    public static BasicDoor LczDoorPrefab => field ??= GetDoor(LczDoorName);

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.HeavyContainment"/>.
    /// </summary>
    public static BasicDoor HczDoorPrefab => field ??= GetDoor(HczDoorName);

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.HeavyContainment"/>.
    /// </summary>
    public static BasicDoor HczBulkDoorPrefab => field ??= GetDoor(HczBulkDoorName);

    /// <summary>
    /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.Entrance"/>.
    /// </summary>
    public static BasicDoor EzDoorPrefab => field ??= GetDoor(EzDoorName);

    private static BasicDoor GetDoor(string name)
        => PrefabStore<BasicDoor>.AllComponentPrefabs.FirstOrDefault(d => d.name == name)
           ?? throw new InvalidOperationException($"[PrefabManager] Failed to get door named {name} | Report this as a bug!");
}