using Godot;

namespace Scalepact.Enemies
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
                stateMachine.ChangeToIdleState();
                return;
            }
            if (stateMachine.IsInRange(stateMachine.AttackRange))
            {
                stateMachine.ChangeToAttackState();
                return;
            }

            stateMachine.OrientRig(stateMachine.Agent3D.GetNextPathPosition());

            Vector3 targetVelocity = Vector3.Zero;
            stateMachine.Agent3D.TargetPosition = stateMachine.Player.GlobalPosition;

            if (!stateMachine.Agent3D.IsTargetReached())
            {
                targetVelocity = stateMachine.GetLocalNavigationDirection() * stateMachine.ChaseMoveSpeed;
                stateMachine.OrientRig(stateMachine.Agent3D.GetNextPathPosition());
            }

            stateMachine.Agent3D.Velocity = targetVelocity;
        }

        public override void ExitState()
        {
            base.ExitState();
            stateMachine.StopAgent();
        }
    }
}