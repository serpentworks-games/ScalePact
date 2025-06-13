using Godot;
using Scalepact.DamageSystem;

namespace Scalepact.Abilities
{
    public partial class CombatAbility : AbilityBase
    {
        [Export] protected int damageAmount;
        [Export] protected Damager abilityDamager;

        public override void _Ready()
        {
            base._Ready();
            abilityDamager.SetupDamager(damageAmount, true);
        }

        public override void StartAbility()
        {
            abilityDamager.ToggleCollider(true);
            base.StartAbility();
        }

        public override void ResolveAbility()
        {
            abilityDamager.ToggleCollider(false);
            base.ResolveAbility();
        }

    }
}