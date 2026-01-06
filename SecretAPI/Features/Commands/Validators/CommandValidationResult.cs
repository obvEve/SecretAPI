namespace SecretAPI.Features.Commands.Validators
{
    /// <summary>
    /// Defines the result of a <see cref="ICommandArgumentValidator{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type this is validating.</typeparam>
    public struct CommandValidationResult<T>
    {
        /// <summary>
        /// Whether the validation was successful.
        /// </summary>
        public readonly bool Success;

        /// <summary>
        /// Gets the value when successful.
        /// </summary>
        public readonly T? Value;

        /// <summary>
        /// The error message, if any exists.
        /// </summary>
        public readonly string? ErrorMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidationResult{T}"/> struct.
        /// </summary>
        /// <param name="value">The value that was validated.</param>
        public CommandValidationResult(T value)
        {
            Value = value;
            Success = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidationResult{T}"/> struct.
        /// </summary>
        /// <param name="error">The error message, including how it went wrong.</param>
        public CommandValidationResult(string error)
        {
            ErrorMessage = error;
            Success = false;
        }
    }
}