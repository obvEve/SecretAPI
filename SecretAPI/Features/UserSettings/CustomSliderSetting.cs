namespace SecretAPI.Features.UserSettings
{
    using global::UserSettings.ServerSpecific;

    /// <summary>
    /// Wrapper for <see cref="SSSliderSetting"/>.
    /// </summary>
    public abstract class CustomSliderSetting : CustomSetting, ISetting<SSSliderSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSliderSetting"/> class.
        /// </summary>
        /// <param name="setting">The setting to wrap.</param>
        protected CustomSliderSetting(SSSliderSetting setting)
            : base(setting)
        {
            Base = setting;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomSliderSetting"/> class.
        /// </summary>
        /// <param name="id">The ID of the setting.</param>
        /// <param name="label">The setting's label.</param>
        /// <param name="minValue">The slider's minimum value.</param>
        /// <param name="maxValue">The slider's maximum value.</param>
        /// <param name="defaultValue">The default value for the slider.</param>
        /// <param name="integer">Whether it should be an integer (false for float).</param>
        /// <param name="valueToStringFormat">Value to string format.</param>
        /// <param name="finalDisplayFormat">The final display format.</param>
        /// <param name="hint">The hint to display.</param>
        protected CustomSliderSetting(
            int? id,
            string label,
            float minValue,
            float maxValue,
            float defaultValue = 0.0f,
            bool integer = false,
            string valueToStringFormat = "0.##",
            string finalDisplayFormat = "{0}",
            string? hint = null)
            : this(new SSSliderSetting(id, label, minValue, maxValue, defaultValue, integer, valueToStringFormat, finalDisplayFormat, hint))
        {
        }

        /// <inheritdoc/>
        public new SSSliderSetting Base { get; }

        /// <summary>
        /// Gets the synced value selected as a float.
        /// </summary>
        public float SelectedValueFloat => Base.SyncFloatValue;

        /// <summary>
        /// Gets the synced value selected as an integer.
        /// </summary>
        public int SelectedValueInt => Base.SyncIntValue;

        /// <summary>
        /// Gets or sets the minimum value of the setting.
        /// </summary>
        public float MinimumValue
        {
            get => Base.MinValue;
            set => Base.MinValue = value;
        }

        /// <summary>
        /// Gets or sets the maximum value of the setting.
        /// </summary>
        public float MaximumValue
        {
            get => Base.MaxValue;
            set => Base.MaxValue = value;
        }

        /// <summary>
        /// Gets the default value of the setting.
        /// </summary>
        public float DefaultValue => Base.DefaultValue;

        /// <summary>
        /// Gets a value indicating whether to use integer. False will use float.
        /// </summary>
        public bool UseInteger => Base.Integer;
    }
}