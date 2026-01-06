namespace SecretAPI.Features.Commands.Validators;

/// <summary>
/// Handles singleton-ing <see cref="ICommandArgumentValidator"/>.
/// </summary>
/// <typeparam name="T">The <see cref="ICommandArgumentValidator"/> type.</typeparam>
public static class ValidatorSingleton<T>
    where T : class, ICommandArgumentValidator, new()
{
    /// <summary>
    /// The current <see cref="T"/> instance.
    /// </summary>
    public static readonly T Instance = new();
}