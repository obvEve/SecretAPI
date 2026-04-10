namespace SecretAPI.Features.Effects;

using PlayerRoles.FirstPersonControl;

/// <summary>
/// Effect that removes stamina usage, granting infinite stamina.
/// </summary>
public class Energized : CustomPlayerEffect, IStaminaModifier
{
    /// <inheritdoc />
    public bool StaminaModifierActive => IsEnabled;

    /// <inheritdoc />
    public float StaminaUsageMultiplier => 0;

    /// <inheritdoc />
    public override EffectClassification Classification => EffectClassification.Positive;
}