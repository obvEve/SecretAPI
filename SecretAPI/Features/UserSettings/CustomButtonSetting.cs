namespace SecretAPI.Features.UserSettings
{
    using System;
    using global::UserSettings.ServerSpecific;

    /// <summary>
    /// Wraps <see cref="SSButton"/>.
    /// </summary>
    public abstract class CustomButtonSetting : CustomSetting, ISetting<SSButton>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomButtonSetting"/> class.
        /// </summary>
        /// <param name="button">The button base.</param>
        protected CustomButtonSetting(SSButton button)
            : base(button)
        {
            Base = button;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomButtonSetting"/> class.
        /// </summary>
        /// <param name="id">The ID of the button.</param>
        /// <param name="label">The setting's label.</param>
        /// <param name="buttonText">The button text.</param>
        /// <param name="holdTimeSeconds">The time to hold.</param>
        /// <param name="hint">The hint to show.</param>
        protected CustomButtonSetting(int? id, string label, string buttonText, float? holdTimeSeconds = null, string? hint = null)
            : this(new SSButton(id, label, buttonText, holdTimeSeconds, hint))
        {
        }

        /// <inheritdoc/>
        public new SSButton Base { get; }

        /// <summary>
        /// Gets the <see cref="TimeSpan"/> of the last press.
        /// </summary>
        public TimeSpan LastPress => Base.SyncLastPress.Elapsed;

        /// <summary>
        /// Gets the text of the button.
        /// </summary>
        public string Text => Base.ButtonText;

        /// <summary>
        /// Gets the amount of time to hold the button in seconds.
        /// </summary>
        public float HoldTime => Base.HoldTimeSeconds;
    }
}