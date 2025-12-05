namespace SecretAPI.Examples.Settings
{
    using SecretAPI.Features.UserSettings;
    using UnityEngine;

    /// <summary>
    /// Example setting for keybinds.
    /// </summary>
    public class ExampleKeybindSetting : CustomKeybindSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleKeybindSetting"/> class.
        /// </summary>
        public ExampleKeybindSetting()
            : base(900, "Example Kill Button", KeyCode.G, allowSpectatorTrigger: false)
        {
        }

        /// <inheritdoc />
        public override CustomHeader Header => CustomHeader.Examples;

        /// <inheritdoc />
        protected override CustomSetting CreateDuplicate() => new ExampleKeybindSetting();

        /// <inheritdoc />
        protected override void HandleSettingUpdate()
        {
            if (!IsPressed)
                return;

            KnownOwner?.Kill();
        }
    }
}