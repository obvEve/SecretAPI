/*namespace SecretAPI.Examples.Commands
{
    using LabApi.Features.Wrappers;
    using SecretAPI.Features.Commands;
    using SecretAPI.Features.Commands.Attributes;

    /// <summary>
    /// An example of a <see cref="CustomCommand"/> that explodes a player.
    /// </summary>
    public class ExampleParentCommand : CustomCommand
    {
        /// <inheritdoc/>
        public override string Command => "exampleparent";

        /// <inheritdoc/>
        public override string Description => "Example of a parent command, handling some sub commands.";

        /// <inheritdoc/>
        public override string[] Aliases { get; } = [];

        /// <inheritdoc/>
        public override CustomCommand[] SubCommands { get; } = [new ExampleExplodeCommand()];

        [ExecuteCommand]
        private void Run([CommandSender] Player sender, Player target)
        {
            TimedGrenadeProjectile.SpawnActive(target.Position, ItemType.GrenadeHE, sender);
        }
    }
}*/