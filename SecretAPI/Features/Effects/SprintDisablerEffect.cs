namespace SecretAPI.Features.Effects;

using PlayerRoles.FirstPersonControl;

/// <summary>
/// Effect that disables sprinting for a player. Sets stamina to 0 and disables regen.
/// </summary>
public class SprintDisablerEffect : CustomPlayerEffect, IStaminaModifier
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