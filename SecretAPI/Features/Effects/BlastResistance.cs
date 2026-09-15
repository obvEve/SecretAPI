namespace SecretAPI.Features.Effects;

using CustomPlayerEffects;
using PlayerStatsSystem;

/// <summary>
/// Grants the player a percentage immunity towards explosion damage.
/// </summary>
/// <remarks>This is 0.5% resistance per intensity level and is capped at 200.</remarks>
public class BlastResistance : CustomPlayerEffect, IDamageModifierEffect
{
    /// <inheritdoc />
    public override byte MaxIntensity => 200;

    /// <inheritdoc />
    public bool DamageModifierActive => IsEnabled;

    /// <inheritdoc />
    public float GetDamageModifier(float baseDamage, DamageHandlerBase handler, HitboxType hitboxType)
    {
        if (handler is not ExplosionDamageHandler)
            return 1;

        float modifier = 1 - ((float)Intensity / 200);
        return modifier;
    }
}