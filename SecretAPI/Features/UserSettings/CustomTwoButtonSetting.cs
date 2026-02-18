namespace SecretAPI.Features.UserSettings
{
    using global::UserSettings.ServerSpecific;

    /// <summary>
    /// Wrapper for <see cref="SSTwoButtonsSetting"/>.
    /// </summary>
    public abstract class CustomTwoButtonSetting : CustomSetting, ISetting<SSTwoButtonsSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTwoButtonSetting"/> class.
        /// </summary>
        /// <param name="button">The setting to wrap.</param>
        protected CustomTwoButtonSetting(SSTwoButtonsSetting button)
            : base(button)
        {
            Base = button;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomTwoButtonSetting"/> class.
        /// </summary>
        /// <param name="id">The ID of the setting.</param>
        /// <param name="label">The setting's label.</param>
        /// <param name="optionA">The first option.</param>
        /// <param name="optionB">The second option.</param>
        /// <param name="defaultIsB">Whether the second option should be default. Default: false.</param>
        /// <param name="hint">The hint to show.</param>
        /// <param name="collectionId">The <see cref="CustomSetting.CollectionId"/>.</param>
        /// <param name="isServerSetting">See <see cref="CustomSetting.IsServerSetting"/>.</param>
        protected CustomTwoButtonSetting(
            int? id,
            string label,
            string optionA,
            string optionB,
            bool defaultIsB = false,
            string? hint = null,
            byte collectionId = byte.MaxValue,
            bool isServerSetting = false)
            : this(new SSTwoButtonsSetting(id, label, optionA, optionB, defaultIsB, hint, collectionId, isServerSetting))
        {
        }

        /// <inheritdoc/>
        public new SSTwoButtonsSetting Base { get; }

        /// <summary>
        /// Gets or sets the current text for the first option.
        /// </summary>
        public string OptionA
        {
            get => Base.OptionA;
            set
            {
                Base.OptionA = value;
                SendOptionsUpdate();
            }
        }

        /// <summary>
        /// Gets or sets the current text for the second option.
        /// </summary>
        public string OptionB
        {
            get => Base.OptionB;
            set
            {
                Base.OptionB = value;
                SendOptionsUpdate();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the selected option is currently the first.
        /// </summary>
        public bool IsOptionA => Base.SyncIsA;

        /// <summary>
        /// Gets a value indicating whether the selected option is currently the second.
        /// </summary>
        public bool IsOptionB => Base.SyncIsB;

        /// <summary>
        /// Gets a value indicating whether the selected option is currently set to the default.
        /// </summary>
        public bool IsDefault => Base.DefaultIsB ? IsOptionB : IsOptionA;

        /// <summary>
        /// Sends an update to <see cref="CustomSetting.KnownOwner"/> that this has been updated on Server. Only works if <see cref="CustomSetting.IsServerSetting"/> is true.
        /// </summary>
        /// <param name="isB">Whether the setting is set to B value now.</param>
        public void SendServerUpdate(bool isB) => Base.SendValueUpdate(isB, false, IsKnownOwnerHub);

        /// <summary>
        /// Sends an update to the <see cref="CustomSetting.KnownOwner"/> that <see cref="OptionA"/> or <see cref="OptionB"/> has changed values.
        /// </summary>
        private void SendOptionsUpdate() => Base.SendTwoButtonUpdate(OptionA, OptionB, false, IsKnownOwnerHub);
    }
}