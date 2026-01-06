namespace SecretAPI.Features.Commands.Validators
{
    /// <summary>
    /// Defines the base of a validator for <see cref="CustomCommand"/>.
    /// </summary>
    /// <typeparam name="T">The type this validator is for.</typeparam>
    public interface ICommandArgumentValidator<T>
    {
        /// <summary>
        /// Validates the specified argument.
        /// </summary>
        /// <param name="argument">The argument needed to validate.</param>
        /// <returns>The result of the validation.</returns>
        public CommandValidationResult<T> Validate(string argument);
    }
}