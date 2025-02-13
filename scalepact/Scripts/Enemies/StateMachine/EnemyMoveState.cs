using Godot;

namespace Scalepact.Enemies
{
    public partial class EnemyMoveState : EnemyBaseState
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

            if (stateMachine.IsPlayerInAttackRange())
            {
                stateMachine.ChangeToAttack();
                return;
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}