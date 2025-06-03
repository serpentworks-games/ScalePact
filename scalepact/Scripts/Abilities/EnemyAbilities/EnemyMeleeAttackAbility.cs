using Godot;
using Scalepact.DamageSystem;

namespace Scalepact.Abilities.EnemyAbilities
{
    public partial class EnemyMeleeAttackAbility : AbilityBase
    {
        [Export] float damageAmount;
        [Export] Damager abilityDamager;

        public override void _Ready()
        {
            base._Ready();
        }
        public override void StartAbility()
        {
            GD.Print("Starting!");
            abilityTimer.Start(abilityCooldownTimer);
            abilityTimeRemaining = abilityDuration;
            abilityDamager.DealDamage(damageAmount);
        }

        public override void ProcessAbility()
        {

        }

        public override void ResolveAbility()
        {
            abilityDamager.ClearExceptions();
            base.ResolveAbility();
        }
    }
}