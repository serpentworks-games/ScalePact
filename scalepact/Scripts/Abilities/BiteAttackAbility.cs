using Godot;
using Scalepact.DamageSystem;

namespace Scalepact.Abilities.PlayerAbilities
{
    public partial class BiteAttackAbility : AbilityBase
    {
        [Export] float damageAmount;
        [Export] Damager abilityDamager;

        public override void StartAbility()
        {
            abilityTimer.Start(abilityCooldownTimer);
            abilityTimeRemaining = abilityDuration;
            abilityDamager.DealDamage(damageAmount);
        }

        public override void ResolveAbility()
        {
            abilityDamager.ClearExceptions();
            base.ResolveAbility();
        }
    }
}