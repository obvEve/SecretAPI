namespace SecretAPI.Features.Commands
{
    using System;
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
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
            => ExecuteGenerated(arguments, sender, out response);

        /// <inheritdoc cref="Execute(ArraySegment{string}, ICommandSender, out string)" />
        protected virtual bool ExecuteGenerated(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Command not implemented.";
            return false;
        }
    }
}