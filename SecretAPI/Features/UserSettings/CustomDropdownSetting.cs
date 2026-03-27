namespace SecretAPI.Features.UserSettings
{
    using System;
    using global::UserSettings.ServerSpecific;

    /// <summary>
    /// Custom <see cref="SSDropdownSetting"/> wrapper.
    /// </summary>
    public abstract class CustomDropdownSetting : CustomSetting, ISetting<SSDropdownSetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDropdownSetting"/> class.
        /// </summary>
        /// <param name="setting">The base setting to create the wrapper with.</param>
        protected CustomDropdownSetting(SSDropdownSetting setting)
            : base(setting)
        {
            Base = setting;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomDropdownSetting"/> class.
        /// </summary>
        /// <param name="id">The ID of the setting.</param>
        /// <param name="label">The setting's label.</param>
        /// <param name="options">The array of string options to give.</param>
        /// <param name="defaultOptionIndex">The default option (int index).</param>
        /// <param name="entryType">The entry type.</param>
        /// <param name="hint">The hint to show.</param>
        /// <param name="collectionId">The <see cref="CustomSetting.CollectionId"/>.</param>
        /// <param name="isServerSetting">See <see cref="CustomSetting.IsServerSetting"/>.</param>
        protected CustomDropdownSetting(
            int? id,
            string label,
            string[] options,
            int defaultOptionIndex = 0,
            SSDropdownSetting.DropdownEntryType entryType = SSDropdownSetting.DropdownEntryType.Regular,
            string? hint = null,
            byte collectionId = byte.MaxValue,
            bool isServerSetting = false)
            : this(new SSDropdownSetting(id, label, options, defaultOptionIndex, entryType, hint, collectionId, isServerSetting))
        {
        }

        /// <inheritdoc/>
        public new SSDropdownSetting Base { get; }

        /// <inheritdoc />
        public override bool HasValueChanged => LastSelectedIndex != SyncedIndex;

        /// <summary>
        /// Gets the index the client is claiming to have selected.
        /// </summary>
        /// <remarks>This is not validated, if you need it validated use <see cref="ValidatedSelectedIndex"/>.</remarks>
        public int SyncedIndex => Base.SyncSelectionIndexRaw;

        /// <summary>
        /// Gets the selected index after validation.
        /// </summary>
        public int ValidatedSelectedIndex => Math.Clamp(SyncedIndex, 0, Options.Length - 1);

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        public string[] Options
        {
            get => Base.Options;
            set
            {
                Base.Options = value;
                SendDropdownUpdate();
            }
        }

        /// <summary>
        /// Gets the selected option as string.
        /// </summary>
        public string SelectedOption => Options[ValidatedSelectedIndex];

        /// <summary>
        /// Gets the <see cref="SSDropdownSetting.DropdownEntryType"/>.
        /// </summary>
        /// <remarks>Refer to https://github.com/HubertMoszka/Server-Specific-Settings-System/blob/main/SSDropdownSetting.cs#L151 for proper documentation.</remarks>
        public SSDropdownSetting.DropdownEntryType EntryType => Base.EntryType;

        /// <summary>
        /// Gets the last selected index, or -1 if none was selected previously.
        /// </summary>
        public int LastSelectedIndex { get; private set; } = -1;

        /// <summary>
        /// Gets the last selected option as a string, or null if none was selected previously.
        /// </summary>
        public string? LastSelectedOption => LastSelectedIndex < 0 || LastSelectedIndex >= Options.Length ? null : Options[LastSelectedIndex];

        /// <summary>
        /// Sends an update to <see cref="CustomSetting.KnownOwner"/> that this has been updated on Server. Only works if <see cref="CustomSetting.IsServerSetting"/> is true.
        /// </summary>
        /// <param name="selectionId">The new ID selected.</param>
        public void SendServerUpdate(int selectionId) => Base.SendValueUpdate(selectionId, false, IsKnownOwnerHub);

        /// <inheritdoc />
        protected internal override void HandleBeforeSettingUpdate()
        {
            base.HandleBeforeSettingUpdate();

            if (LastUpdateType != SettingResponseType.Initial)
                LastSelectedIndex = SyncedIndex;
        }

        /// <summary>
        /// Sends an update to <see cref="CustomSetting.KnownOwner"/> that <see cref="Options"/> has been updated.
        /// </summary>
        private void SendDropdownUpdate() => Base.SendDropdownUpdate(Options, false,  IsKnownOwnerHub);
    }
}