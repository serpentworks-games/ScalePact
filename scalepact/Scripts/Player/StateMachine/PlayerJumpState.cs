using Godot;

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
            if (!stateMachine.PlayerCharBody3D.IsOnFloor() && Input.IsActionPressed("jump"))
            {
                //switch to gliding
                //return;
                GD.Print("Activing gliding!");
            }

            if (stateMachine.PlayerCharBody3D.IsOnFloor())
            {
                stateMachine.ChangeToGroundMovement();
                return;
            }

            GetVelocityAndDirection();

            velocity = stateMachine.ResolveMovementPhysics(
                velocity, direction, stateMachine.JumpMoveSpeed, (float)delta);

            AerialCharacterMovement(delta, velocity);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}