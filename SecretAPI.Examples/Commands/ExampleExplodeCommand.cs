namespace SecretAPI.Examples.Commands
{
    using LabApi.Features.Console;
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
        private CommandResult Explode([CommandSender] Player sender, Player target)
        {
            Logger.Debug($"Example explode command run by {sender.Nickname} - Target: {target.Nickname}");
            TimedGrenadeProjectile.SpawnActive(target.Position, ItemType.GrenadeHE, sender);
            return new CommandResult(true, "Success");
        }
    }
}