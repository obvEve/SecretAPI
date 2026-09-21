namespace SecretAPI.Features.UserSettings;

using System;
using System.Diagnostics;
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
    /// Gets the <see cref="Stopwatch"/> controlling the last press.
    /// </summary>
    public Stopwatch LastPressWatch => Base.SyncLastPress;

    /// <summary>
    /// Gets the <see cref="TimeSpan"/> of the last press.
    /// </summary>
    [Obsolete("Use TimeSinceLastPress instead - This will be removed in 4.0")]
    public TimeSpan LastPress => TimeSinceLastPress;

    /// <summary>
    /// Gets the <see cref="TimeSpan"/> of the last press.
    /// </summary>
    public TimeSpan TimeSinceLastPress => LastPressWatch.Elapsed;

    /// <summary>
    /// Gets a value indicating whether the button has ever been pressed.
    /// </summary>
    public bool EverPressed => LastPressWatch.IsRunning;

    /// <summary>
    /// Gets the amount of times the button has been pressed.
    /// </summary>
    public uint TimesPressed { get; private set; }

    /// <summary>
    /// Gets or sets the text of the button.
    /// </summary>
    public string Text
    {
        get => Base.ButtonText;
        set
        {
            Base.ButtonText = value;
            SendButtonUpdate();
        }
    }

    /// <summary>
    /// Gets or sets the amount of time to hold the button in seconds.
    /// </summary>
    public float RequiredHoldTime
    {
        get => Base.HoldTimeSeconds;
        set
        {
            Base.HoldTimeSeconds = value;
            SendButtonUpdate();
        }
    }

    /// <inheritdoc/>
    protected override void HandleBeforeSettingUpdate()
    {
        base.HandleBeforeSettingUpdate();
        TimesPressed++;
    }

    /// <summary>
    /// Sends an update to <see cref="CustomSetting.KnownOwner"/> that <see cref="Text"/> or <see cref="RequiredHoldTime"/> has updated.
    /// </summary>
    private void SendButtonUpdate()
    {
        if (!IsCurrentlyAccessible)
            return;

        Base.SendButtonUpdate(Text, RequiredHoldTime, false, IsKnownOwnerHub);
    }
}