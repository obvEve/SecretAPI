namespace SecretAPI.Features;

using System.Collections.Generic;

/// <summary>
/// Handles the registration of objects and ids.
/// </summary>
/// <typeparam name="T">The type of the registry.</typeparam>
public static class Registry<T>
{
    /// <summary>
    /// Gets the registered objects.
    /// </summary>
    public static List<T> Registered { get; } = [];

    /// <summary>
    /// Gets or sets the current id iteration.
    /// </summary>
    /// <remarks>You should increment this with ++CurrentId when using it.</remarks>
    public static int CurrentId { get; set; }
}