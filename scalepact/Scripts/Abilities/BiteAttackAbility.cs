using Godot;
using Scalepact.DamageSystem;

namespace Scalepact.Abilities.PlayerAbilities
{
    public partial class BiteAttackAbility : AbilityBase
    {
        [Export] Damager abilityDamager;
        public override void StartAbility()
        {
            abilityTimer.Start(abilityCooldownTimer);
            abilityTimeRemaining = abilityDuration;
        }
    }
}