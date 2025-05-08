using Godot;
using Scalepact.Enemies;

namespace Scalepact.Abilities.EnemyAbilities
{
    public partial class EnemyMeleeAttackAbility : AbilityBase
    {
        public override void _Ready()
        {
            if (stateMachine is EnemyStateMachine)
            {
                EnemyStateMachine enemy = (EnemyStateMachine)stateMachine;
                enemy.OnAttackTriggered += StartAbility;
            }
            base._Ready();
        }
        public override void StartAbility()
        {
            SetIsInAbility(true);
            GD.Print("Starting!");
            abilityTimer.Start(cooldownTimer);
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