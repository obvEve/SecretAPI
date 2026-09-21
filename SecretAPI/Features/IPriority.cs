namespace SecretAPI.Features;

/// <summary>
/// Handles priority on certain things.
/// </summary>
public interface IPriority
{
    /// <summary>
    /// Gets the current priority.
    /// </summary>
    public int Priority { get; }
}