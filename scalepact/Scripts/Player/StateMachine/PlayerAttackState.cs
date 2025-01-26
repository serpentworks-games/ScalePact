using Godot;

namespace Scalepact.Player
{
    public partial class PlayerAttackState : PlayerBaseState
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

            if (!stateMachine.IsAttackActive())
            {
                stateMachine.ChangeState(PlayerStringRefs.PlayerMoveState);
                return;
            }

            Vector3 velocity = stateMachine.PlayerCharBody3D.Velocity;

            Vector3 direction = stateMachine.GetMovementDirection();

            velocity = stateMachine.ApplyAttackMovement(velocity, direction, (float)delta);

            velocity = stateMachine.ApplyGravity((float)delta, velocity);

            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();

        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}