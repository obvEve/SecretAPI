namespace SecretAPI.Features.Commands
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;
    using CommandSystem;

    /// <summary>
    /// Defines the base of a custom <see cref="ICommand"/>.
    /// </summary>
    public abstract partial class CustomCommand : ICommand
    {
        /// <inheritdoc />
        public abstract string Command { get; }

        /// <inheritdoc />
        public abstract string Description { get; }

        /// <inheritdoc />
        public virtual string[] Aliases { get; } = [];

        /// <summary>
        /// Gets an array of the sub commands for this command.
        /// </summary>
        public virtual CustomCommand[] SubCommands { get; } = [];

        /// <inheritdoc />
        /// <remarks>This should not be overwritten except by source generation.</remarks>
        public virtual bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            throw new NotImplementedException($"Command {Command} not implemented. Did source generation fail? - If this is not intentional, submit a bugreport!");
        }

        /// <summary>
        /// Checks whether an argument matches any <see cref="SubCommands"/>.
        /// </summary>
        /// <param name="argument">The argument to check if is reference to subcommand.</param>
        /// <param name="command">The command found, otherwise false.</param>
        /// <returns>Whether a sub command matching the argument was found.</returns>
        protected bool CheckSubCommand(string argument, [NotNullWhen(true)] out CustomCommand? command)
        {
            foreach (CustomCommand subCommand in SubCommands)
            {
                if (subCommand.Command == argument || subCommand.Aliases.Any(alias => alias == argument))
                {
                    command = subCommand;
                    return true;
                }
            }

            command = null;
            return false;
        }
    }
}