namespace SecretAPI.Features.Effects
{
    using CustomPlayerEffects;
    using UnityEngine;

    /// <summary>
    /// Custom Effect for <see cref="TickingEffectBase"/>.
    /// </summary>
    public abstract class CustomTickingPlayerEffect : CustomPlayerEffect
    {
        private float timeTillTick;

        /// <summary>
        /// Gets the time that should pass between each tick.
        /// </summary>
        protected virtual float TimePerTick => 1;

        /// <inheritdoc/>
        public override void Enabled()
        {
            timeTillTick = TimePerTick;
        }

        /// <inheritdoc/>
        public override void OnEffectUpdate()
        {
            base.OnEffectUpdate();

            if (TimePerTick == 0)
                return;

            timeTillTick -= Time.deltaTime;
            if (timeTillTick > 0)
                return;

            timeTillTick += TimePerTick;
            OnTick();
        }

        /// <summary>
        /// Called everytime <see cref="TimePerTick"/> passes.
        /// </summary>
        public virtual void OnTick()
        {
        }
    }
}