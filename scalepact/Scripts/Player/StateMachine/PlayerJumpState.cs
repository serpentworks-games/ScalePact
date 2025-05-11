namespace Scalepact.Player
{
    public partial class PlayerJumpState : PlayerBaseState
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
            GetVelocityAndDirection();

            velocity = stateMachine.ApplyJumpVelocity(velocity);

            velocity = stateMachine.ResolveMovementPhysics(
                            direction, velocity, stateMachine.JumpMoveSpeed, (float)delta);

            AerialCharacterMovement(velocity, delta);

            if (stateMachine.PlayerCharBody3D.IsOnFloor())
            {
                stateMachine.ChangeToGroundMovement();
                return;
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}