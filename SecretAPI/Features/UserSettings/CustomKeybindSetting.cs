namespace SecretAPI.Features.UserSettings
{
    using global::UserSettings.ServerSpecific;
    using UnityEngine;

    /// <summary>
    /// Wrapper for <see cref="SSKeybindSetting"/>.
    /// </summary>
    public abstract class CustomKeybindSetting : CustomSetting, ISetting<SSKeybindSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomKeybindSetting"/> class.
        /// </summary>
        /// <param name="setting">The setting to wrap.</param>
        protected CustomKeybindSetting(SSKeybindSetting setting)
            : base(setting)
        {
            Base = setting;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomKeybindSetting"/> class.
        /// </summary>
        /// <param name="id">The ID of the setting.</param>
        /// <param name="label">The setting's label.</param>
        /// <param name="suggestedKey">The suggested key.</param>
        /// <param name="preventInteractionOnGui">Whether to prevent interaction in a GUI.</param>
        /// <param name="allowSpectatorTrigger">Whether to allow spectators to trigger.</param>
        /// <param name="hint">The hint to show.</param>
        protected CustomKeybindSetting(
            int? id,
            string label,
            KeyCode suggestedKey = KeyCode.None,
            bool preventInteractionOnGui = true,
            bool allowSpectatorTrigger = true,
            string? hint = null)
            : this(new SSKeybindSetting(id, label, suggestedKey, preventInteractionOnGui, allowSpectatorTrigger, hint))
        {
        }

        /// <inheritdoc/>
        public new SSKeybindSetting Base { get; }

        /// <summary>
        /// Gets a value indicating whether the keybind is pressed.
        /// </summary>
        public bool IsPressed => Base.SyncIsPressed;
    }
}