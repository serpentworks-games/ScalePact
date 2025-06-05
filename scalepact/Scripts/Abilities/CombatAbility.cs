using Godot;
using Scalepact.DamageSystem;

namespace Scalepact.Abilities
{
    public partial class CombatAbility : AbilityBase
    {
        [Export] protected float damageAmount;
        [Export] protected Damager abilityDamager;

        public override void StartAbility()
        {
            abilityDamager.DealDamage(damageAmount);
            base.StartAbility();
        }

        public override void ResolveAbility()
        {
            abilityDamager.ClearExceptions();
            base.ResolveAbility();
        }

    }
}