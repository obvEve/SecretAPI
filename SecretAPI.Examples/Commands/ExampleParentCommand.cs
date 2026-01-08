namespace SecretAPI.Examples.Commands
{
    using System;
    using LabApi.Features.Console;
    using LabApi.Features.Wrappers;
    using SecretAPI.Features.Commands;
    using SecretAPI.Features.Commands.Attributes;

    /// <summary>
    /// An example of a <see cref="CustomCommand"/> that explodes a player.
    /// </summary>
    public partial class ExampleParentCommand : CustomCommand
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
        private CommandResult Run([CommandSender] Player sender)
        {
            Logger.Debug($"Example parent was run by {sender.Nickname}");
            return new CommandResult(true, "Success");
        }
    }
}