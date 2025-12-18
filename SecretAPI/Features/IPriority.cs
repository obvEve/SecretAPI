namespace SecretAPI.Features
{
    /// <summary>
    /// Handles IPriority.
    /// </summary>
    public interface IPriority
    {
        /// <summary>
        /// Gets the current priority.
        /// </summary>
        public int Priority { get; }
    }
}