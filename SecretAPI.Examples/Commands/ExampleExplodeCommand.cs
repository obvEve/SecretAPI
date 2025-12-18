namespace SecretAPI.Examples.Commands
{
    using LabApi.Features.Wrappers;
    using SecretAPI.Features.Commands;
    using SecretAPI.Features.Commands.Attributes;

    /// <summary>
    /// An example subcommand for <see cref="ExampleParentCommand"/>.
    /// </summary>
    public partial class ExampleExplodeCommand : CustomCommand
    {
        /// <inheritdoc/>
        public override string Command => "explode";

        /// <inheritdoc/>
        public override string Description => "Explodes a player!";

        [ExecuteCommand]
        private void Explode(Player sender, Player target)
            => TimedGrenadeProjectile.SpawnActive(target.Position, ItemType.GrenadeHE, sender);
    }
}