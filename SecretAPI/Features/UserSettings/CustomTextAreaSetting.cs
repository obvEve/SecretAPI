namespace SecretAPI.Features.UserSettings;

using global::UserSettings.ServerSpecific;
using TMPro;

/// <summary>
/// Wrapper for <see cref="SSTextArea"/>.
/// </summary>
public abstract class CustomTextAreaSetting : CustomSetting, ISetting<SSTextArea>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="CustomTextAreaSetting"/> class.
    /// </summary>
    /// <param name="setting">The setting to wrap.</param>
    protected CustomTextAreaSetting(SSTextArea setting)
        : base(setting)
    {
        Base = setting;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CustomTextAreaSetting"/> class.
    /// </summary>
    /// <param name="id">The ID of the setting.</param>
    /// <param name="content">The content of the setting.</param>
    /// <param name="foldoutMode">The foldout mode.</param>
    /// <param name="collapsedText">The collapsed text.</param>
    /// <param name="textAlignment">The align for the text.</param>
    protected CustomTextAreaSetting(
        int? id,
        string content,
        SSTextArea.FoldoutMode foldoutMode = SSTextArea.FoldoutMode.NotCollapsable,
        string? collapsedText = null,
        TextAlignmentOptions textAlignment = TextAlignmentOptions.TopLeft)
        : this(new SSTextArea(id, content, foldoutMode, collapsedText, textAlignment))
    {
    }

    /// <inheritdoc />
    public new SSTextArea Base { get; }

    /// <summary>
    /// Gets or sets the current content. This is equal to <see cref="CustomSetting.Label"/>.
    /// </summary>
    public string Content
    {
        get => Label;
        set => Label = value;
    }

    /// <summary>
    /// Gets the foldout mode.
    /// </summary>
    public SSTextArea.FoldoutMode Foldout => Base.Foldout;

    /// <summary>
    /// Gets the <see cref="TextAlignmentOptions"/> of the setting.
    /// </summary>
    public TextAlignmentOptions AlignmentOptions => Base.AlignmentOptions;
}