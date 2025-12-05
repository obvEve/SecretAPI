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
        protected CustomTwoButtonSetting(int? id, string label, string optionA, string optionB, bool defaultIsB = false, string? hint = null)
            : this(new SSTwoButtonsSetting(id, label, optionA, optionB, defaultIsB, hint))
        {
        }

        /// <inheritdoc/>
        public new SSTwoButtonsSetting Base { get; }

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
    }
}