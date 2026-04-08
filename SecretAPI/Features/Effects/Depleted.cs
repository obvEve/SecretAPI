namespace SecretAPI.Features.Effects;

using PlayerRoles.FirstPersonControl;

/// <summary>
/// Effect that disables sprinting for a player and disables regeneration of it.
/// </summary>
public class Depleted : CustomPlayerEffect, IStaminaModifier
{
    /// <inheritdoc />
    public bool StaminaModifierActive => IsEnabled;

    /// <inheritdoc />
    public float StaminaRegenMultiplier => 0;

    /// <inheritdoc />
    public override EffectClassification Classification => EffectClassification.Negative;

    /// <inheritdoc />
    public override void Enabled()
    {
        Owner.StaminaRemaining = 0;
    }

    /// <inheritdoc />
    public override void Disabled()
    {
    }
}