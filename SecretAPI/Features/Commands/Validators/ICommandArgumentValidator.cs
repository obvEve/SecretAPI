namespace SecretAPI.Features.Commands.Validators
{
    /// <summary>
    /// Defines the base of a validator for <see cref="CustomCommand"/>.
    /// </summary>
    public interface ICommandArgumentValidator
    {
        /// <summary>
        /// Validates the specified argument.
        /// </summary>
        /// <param name="argument">The argument needed to validate.</param>
        /// <returns>The result of the validation..</returns>
        public CommandValidationResult Validate(string argument);
    }
}