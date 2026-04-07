namespace SecretAPI.Patches.Features;

/// <summary>
/// Interface for handling weighted items.
/// </summary>
public interface IWeighted
{
    /// <summary>
    /// Gets the weight of the object.
    /// </summary>
    public float Weight { get; }
}