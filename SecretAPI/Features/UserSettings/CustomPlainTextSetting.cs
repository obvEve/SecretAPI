namespace SecretAPI.Features.UserSettings
{
    using System;
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
        /// Gets the input text prior to the most recent <see cref="CustomSetting.HandleSettingUpdate"/> call.
        /// </summary>
        public string LastInputText
        {
            get => field ??= string.Empty;
            private set;
        }

        /// <summary>
        /// Gets the synced input text.
        /// </summary>
        public string InputText => Base.SyncInputText;

        /// <summary>
        /// Gets or sets the content type.
        /// </summary>
        public TMP_InputField.ContentType ContentType
        {
            get => Base.ContentType;
            set
            {
                Base.ContentType = value;
                SendPlaintextUpdate();
            }
        }

        /// <summary>
        /// Gets or sets the placeholder.
        /// </summary>
        public string Placeholder
        {
            get => Base.Placeholder;
            set
            {
                Base.Placeholder = value;
                SendPlaintextUpdate();
            }
        }

        /// <summary>
        /// Gets or sets the character limit.
        /// </summary>
        public int CharacterLimit
        {
            get => Base.CharacterLimit;
            set
            {
                Base.CharacterLimit = value;
                SendPlaintextUpdate();
            }
        }

        /// <summary>
        /// Sends an update to <see cref="CustomSetting.KnownOwner"/> that this has been updated on Server. Only works if <see cref="CustomSetting.IsServerSetting"/> is true.
        /// </summary>
        /// <param name="text">The new text.</param>
        public void SendServerUpdate(string text) => Base.SendValueUpdate(text, false, IsKnownOwnerHub);

        /// <inheritdoc />
        protected internal override void HandleBeforeSettingUpdate()
        {
            base.HandleBeforeSettingUpdate();
            LastInputText = InputText;
        }

        /// <summary>
        /// Sends an update to the <see cref="CustomSetting.KnownOwner"/> that <see cref="Placeholder"/> <see cref="CharacterLimit"/> or <see cref="ContentType"/> has changed values.
        /// </summary>
        private void SendPlaintextUpdate() => Base.SendPlaintextUpdate(Placeholder, (ushort)Math.Clamp(CharacterLimit, ushort.MinValue, ushort.MaxValue), ContentType, false, IsKnownOwnerHub);
    }
}