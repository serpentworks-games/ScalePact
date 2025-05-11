using Godot;

namespace Scalepact.Abilities.EnemyAbilities
{
    public partial class EnemyMeleeAttackAbility : AbilityBase
    {
        public override void _Ready()
        {
            base._Ready();
        }
        public override void StartAbility()
        {
            GD.Print("Starting!");
            abilityTimer.Start(abilityCooldownTimer);
            abilityTimeRemaining = abilityDuration;
        }

        public override void ProcessAbility()
        {

        }

        public override void ResolveAbility()
        {

        }
    }
}