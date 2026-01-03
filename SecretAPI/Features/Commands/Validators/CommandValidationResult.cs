namespace SecretAPI.Features.Commands.Validators
{
    /// <summary>
    /// Defines the result of a <see cref="ICommandArgumentValidator"/>.
    /// </summary>
    public struct CommandValidationResult
    {
        /// <summary>
        /// Whether the validation was successful.
        /// </summary>
        public readonly bool Success;

        /// <summary>
        /// The error message, if any exists.
        /// </summary>
        public readonly string? ErrorMessage;

        /// <summary>
        /// If an argument failed, then the name. Other-wise null.
        /// </summary>
        public readonly string? ArgumentFailName;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidationResult"/> struct.
        /// </summary>
        public CommandValidationResult()
        {
            Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidationResult"/> struct.
        /// </summary>
        /// <param name="argument">The argument that failed.</param>
        /// <param name="error">The error message, including how it went wrong.</param>
        public CommandValidationResult(string argument, string error)
        {
            Success = false;
        }
    }
}