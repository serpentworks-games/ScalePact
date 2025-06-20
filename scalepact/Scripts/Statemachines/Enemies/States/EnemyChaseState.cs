namespace Scalepact.StateMachines.Enemies
{
    public partial class EnemyChaseState : EnemyBaseState
    {
        public override void EnterState()
        {
            base.EnterState();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            if (!stateMachine.IsInRange(stateMachine.ChaseRange))
            {
                stateMachine.ChangeToSuspicionState();
                return;
            }
            if (stateMachine.IsInRange(stateMachine.AttackRange))
            {
                if (stateMachine.MeleeAttackAbility.IsAbilityAvailable())
                {
                    stateMachine.ChangeToAttackState();
                    return;
                }
                else
                {
                    stateMachine.ChangeToIdleState();
                    return;
                }
            }

            MoveNavigationAgent(stateMachine.Player.GlobalPosition, stateMachine.ChaseMoveSpeed);
        }



        public override void ExitState()
        {
            base.ExitState();
            stateMachine.StopAgent();
        }
    }
}