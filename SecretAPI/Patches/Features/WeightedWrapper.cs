namespace SecretAPI.Patches.Features;

/// <summary>
/// Wraps a value into a struct for implementation with <see cref="IWeighted"/>.
/// </summary>
/// <typeparam name="T">The type of the value.</typeparam>
public readonly struct WeightedWrapper<T> : IWeighted
{
    /// <summary>
    /// Initializes a new instance of the <see cref="WeightedWrapper{T}"/> struct.
    /// </summary>
    /// <param name="value">The value to wrap.</param>
    /// <param name="weight">The weight of the value.</param>
    public WeightedWrapper(T value, float weight)
    {
        Value = value;
        Weight = weight;
    }

    /// <summary>
    /// Gets the value with a weight assigned.
    /// </summary>
    public T Value { get; }

    /// <inheritdoc />
    public float Weight { get; }
}