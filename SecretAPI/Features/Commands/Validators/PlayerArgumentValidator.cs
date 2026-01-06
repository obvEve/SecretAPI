namespace SecretAPI.Features.Commands.Validators
{
    using LabApi.Features.Wrappers;

    /// <summary>
    /// Validates command argument for <see cref="Player"/>.
    /// </summary>
    public class PlayerArgumentValidator : ICommandArgumentValidator<Player>
    {
        /// <inheritdoc />
        public CommandValidationResult<Player> Validate(string argument)
        {
            // player id
            if (int.TryParse(argument, out int value) && Player.TryGet(value, out Player? found))
                return new CommandValidationResult<Player>(found);

            // player user id
            if (Player.TryGet(argument, out found))
                return new CommandValidationResult<Player>(found);

            foreach (Player player in Player.List)
            {
                if (player.Nickname == argument || player.UserId == argument)
                    return new CommandValidationResult<Player>(player);
            }

            return new CommandValidationResult<Player>($"{argument} is not a valid player!");
        }
    }
}