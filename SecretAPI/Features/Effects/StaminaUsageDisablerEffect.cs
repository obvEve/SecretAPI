namespace SecretAPI.Features.Effects;

using PlayerRoles.FirstPersonControl;

/// <summary>
/// Effect that disables stamina usage.
/// </summary>
public class StaminaUsageDisablerEffect : CustomPlayerEffect, IStaminaModifier
{
    /// <inheritdoc />
    public bool StaminaModifierActive => IsEnabled;

    /// <inheritdoc />
    public float StaminaUsageMultiplier => 0;

    /// <inheritdoc />
    public override EffectClassification Classification => EffectClassification.Positive;
}