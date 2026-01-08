namespace SecretAPI.Features.Commands
{
    /// <summary>
    /// Defines the result of a <see cref="CustomCommand"/>.
    /// </summary>
    public readonly struct CommandResult(bool success, string response)
    {
        /// <summary>
        /// Whether the command succeeded.
        /// </summary>
        public readonly bool Success = success;

        /// <summary>
        /// The response to give after command use.
        /// </summary>
        public readonly string Response = response;
    }
}