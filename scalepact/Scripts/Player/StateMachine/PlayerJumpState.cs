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

            Vector3 velocity = stateMachine.PlayerCharBody3D.Velocity;

            Vector3 direction = stateMachine.GetMovementDirection();

            velocity = stateMachine.ApplyGravity((float)delta, velocity);

            velocity = stateMachine.ApplyJumpVelocity(velocity);

            velocity = stateMachine.ApplyJumpMovement(velocity, direction, (float)delta);

            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();

            // if (!stateMachine.PlayerCharBody3D.IsOnFloor() && Input.IsActionPressed("jump"))
            // {
            //     //switch to gliding
            //     //return;
            //     GD.Print("Activing gliding!");
            // }

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