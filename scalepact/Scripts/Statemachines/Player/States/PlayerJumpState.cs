namespace Scalepact.StateMachines.Player
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

            velocity = stateMachine.AddGravity(velocity, (float)delta);

            velocity = stateMachine.HandleJump(velocity);

            velocity = stateMachine.ApplyMovement(direction, velocity, stateMachine.MoveSpeed, (float)delta);

            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();

            if (stateMachine.WasOnFloor)
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