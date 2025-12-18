/*namespace SecretAPI.Features.Commands.Parsing
{
    using System.Reflection;

    /// <summary>
    /// The result of <see cref="CustomCommandHandler.TryParse"/> to know what to do.
    /// </summary>
    internal struct CommandParseResult
    {
        /// <summary>
        /// Gets a value indicating whether parsing was successful.
        /// </summary>
        public bool CouldParse;

        /// <summary>
        /// If parsing failed, will provide the fail reason, otherwise null.
        /// </summary>
        public string FailedResponse;

        /// <summary>
        /// If parsing succeded, the method to call with <see cref="ProvidedArguments"/>.
        /// </summary>
        public MethodInfo Method;

        /// <summary>
        /// If parsing succeeded, the arguments provided.
        /// </summary>
        public object[]? ProvidedArguments;
    }
}*/