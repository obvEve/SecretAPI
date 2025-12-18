namespace SecretAPI.Features
{
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

        private static BasicDoor? lczDoor;
        private static BasicDoor? hczDoor;
        private static BasicDoor? hczBulkDoor;
        private static BasicDoor? ezDoor;

        /// <summary>
        /// Gets the <see cref="ReferenceHub"/> prefab.
        /// </summary>
        public static ReferenceHub PlayerPrefab => PrefabStore<ReferenceHub>.Prefab;

        /// <summary>
        /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.LightContainment"/>.
        /// </summary>
        public static BasicDoor LczDoorPrefab => lczDoor ??= GetDoor(LczDoorName);

        /// <summary>
        /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.HeavyContainment"/>.
        /// </summary>
        public static BasicDoor HczDoorPrefab => hczDoor ??= GetDoor(HczDoorName);

        /// <summary>
        /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.HeavyContainment"/>.
        /// </summary>
        public static BasicDoor HczBulkDoorPrefab => hczBulkDoor ??= GetDoor(HczBulkDoorName);

        /// <summary>
        /// Gets the <see cref="BasicDoor"/> found in <see cref="FacilityZone.Entrance"/>.
        /// </summary>
        public static BasicDoor EzDoorPrefab => ezDoor ??= GetDoor(EzDoorName);

        private static BasicDoor GetDoor(string name)
            => PrefabStore<BasicDoor>.AllComponentPrefabs.FirstOrDefault(d => d.name == name)!;
    }
}