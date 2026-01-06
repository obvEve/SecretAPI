namespace SecretAPI.Features.Commands.Validators
{
    using System;

    /// <summary>
    /// Validator for <see cref="Enum"/>.
    /// </summary>
    /// <typeparam name="TEnum">The <see cref="Enum"/> to validate.</typeparam>
    public sealed class EnumArgumentValidator<TEnum> : ICommandArgumentValidator<TEnum>
        where TEnum : struct, Enum
    {
        /// <inheritdoc />
        public CommandValidationResult<TEnum> Validate(string argument)
        {
            return Enum.TryParse(argument, true, out TEnum value)
                ? new CommandValidationResult<TEnum>(value)
                : new CommandValidationResult<TEnum>($"Argument provided was not a valid {typeof(TEnum).Name}");
        }
    }
}