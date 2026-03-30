namespace SecretAPI.Features.Effects;

using CustomPlayerEffects;
using PlayerStatsSystem;

/// <summary>
/// Grants a player temporary damage immunity.
/// </summary>
public class TemporaryDamageImmunity : CustomPlayerEffect, IDamageModifierEffect
{
    /// <inheritdoc/>
    public bool DamageModifierActive => IsEnabled;

    /// <inheritdoc />
    public override EffectClassification Classification => EffectClassification.Technical;

    /// <inheritdoc/>
    public float GetDamageModifier(float baseDamage, DamageHandlerBase handler, HitboxType hitboxType) => 0;
}