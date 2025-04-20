using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerBaseState : State
    {
        protected new PlayerStateMachine stateMachine;
        protected Vector3 attackDirection = Vector3.Zero;
        protected Vector3 velocity = Vector3.Zero;
        protected Vector3 direction = Vector3.Zero;

        public override void _Ready()
        {
            if (GetParent() != null && GetParent().HasMethod("ChangeState"))
                stateMachine = (PlayerStateMachine)GetParent();
            else
                GD.PushError($"Parent is not a valid StateMachine for state: {Name}");

            SetProcessesOnState(false);
        }

        public void GetVelocityAndDirection()
        {
            velocity = stateMachine.PlayerCharBody3D.Velocity;

            direction = stateMachine.GetMovementDirection();
        }

        public void GroundedCharacterMovement(Vector3 velocity, double delta)
        {
            velocity = stateMachine.ApplyGravity((float)delta, velocity);

            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();
        }

        //Separated out to allow for aerial movement
        public void AerialCharacterMovement(Vector3 velocity, double delta)
        {
            velocity = stateMachine.ApplyGravity((float)delta, velocity);

            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();
        }

    }
}