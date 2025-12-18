namespace SecretAPI.Features.UserSettings
{
    using global::UserSettings.ServerSpecific;
    using TMPro;

    /// <summary>
    /// Wrapper for <see cref="SSPlaintextSetting" />.
    /// </summary>
    public abstract class CustomPlainTextSetting : CustomSetting, ISetting<SSPlaintextSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPlainTextSetting"/> class.
        /// </summary>
        /// <param name="setting">The setting to create wrapper from.</param>
        protected CustomPlainTextSetting(SSPlaintextSetting setting)
            : base(setting)
        {
            Base = setting;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomPlainTextSetting"/> class.
        /// </summary>
        /// <param name="id">The ID of the setting.</param>
        /// <param name="label">The setting's label.</param>
        /// <param name="placeholder">The placeholder to use for the setting.</param>
        /// <param name="characterLimit">The max allowed characters.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="hint">The hint to display for the setting.</param>
        protected CustomPlainTextSetting(
            int? id,
            string label,
            string placeholder = "...",
            int characterLimit = 64,
            TMP_InputField.ContentType contentType = TMP_InputField.ContentType.Standard,
            string? hint = null)
            : this(new SSPlaintextSetting(id, label, placeholder, characterLimit, contentType, hint))
        {
        }

        /// <inheritdoc />
        public new SSPlaintextSetting Base { get; }

        /// <summary>
        /// Gets the synced input text.
        /// </summary>
        public string InputText => Base.SyncInputText;

        /// <summary>
        /// Gets the content type.
        /// </summary>
        public TMP_InputField.ContentType ContentType => Base.ContentType;

        /// <summary>
        /// Gets the placeholder.
        /// </summary>
        public string Placeholder => Base.Placeholder;

        /// <summary>
        /// Gets the character limit.
        /// </summary>
        public int CharacterLimit => Base.CharacterLimit;
    }
}