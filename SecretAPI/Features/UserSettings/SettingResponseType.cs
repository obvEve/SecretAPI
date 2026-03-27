namespace SecretAPI.Features.UserSettings
{
    /// <summary>
    /// The type of response.
    /// </summary>
    public enum SettingResponseType
    {
        /// <summary>
        /// Indicates that no response has been recorded.
        /// </summary>
        None,

        /// <summary>
        /// Indicates that this is the initial response.
        /// </summary>
        Initial,

        /// <summary>
        /// Indicates that this is an update, changing the value.
        /// </summary>
        Update,
    }
}