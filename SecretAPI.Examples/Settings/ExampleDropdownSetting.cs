namespace SecretAPI.Examples.Settings
{
    using LabApi.Features.Console;
    using LabApi.Features.Permissions;
    using SecretAPI.Features.UserSettings;

    /// <summary>
    /// Example version of <see cref="CustomDropdownSetting"/>.
    /// </summary>
    public class ExampleDropdownSetting : CustomDropdownSetting
    {
        private static readonly string[] ExampleOptions = ["hi", "test", "yum", "fish", "nugget"];
        private static readonly string[] ExampleSupporterOptions = ["bucket", "lava", "wanted", "globe"];

        /// <summary>
        /// Initializes a new instance of the <see cref="ExampleDropdownSetting"/> class.
        /// </summary>
        public ExampleDropdownSetting()
            : base(901, "Example dropdown", ExampleOptions)
        {
        }

        /// <inheritdoc/>
        public override CustomHeader Header => CustomHeader.Examples;

        /// <inheritdoc/>
        protected override CustomSetting CreateDuplicate() => new ExampleDropdownSetting();

        /// <inheritdoc/>
        protected override void PersonalizeSetting()
        {
            if (KnownOwner == null || !KnownOwner.HasAnyPermission("example.supporter"))
                return;

            Options = ExampleSupporterOptions;
        }

        /// <inheritdoc/>
        protected override void HandleSettingUpdate()
        {
            Logger.Info($"{KnownOwner?.DisplayName ?? "null reference"} selected {SelectedOption} (Index {ValidatedSelectedIndex}/{Options.Length})");
        }
    }
}