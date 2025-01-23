using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerBaseState : State
    {
        protected new PlayerStateMachine stateMachine;

        //Local refs from the statemachine to avoid overly long names
        CharacterBody3D playerBody3D;
        Node3D horizontalPivot;

        public override void _Ready()
        {
            if (GetParent() != null && GetParent().HasMethod("ChangeState"))
                stateMachine = (PlayerStateMachine)GetParent();
            else
                GD.PushError($"Parent is not a valid StateMachine for state: {Name}");

            SetProcessesOnState(false);
        }

        public override void EnterState()
        {
            playerBody3D = stateMachine.PlayerCharBody3D;
            horizontalPivot = stateMachine.CameraController.HorizontalPivot;

            base.EnterState();
        }

        /// <summary>
        /// Handles calculating movement based on the Godot template.
        /// Includes grounded movement and basic jumping.
        /// Glide is handled in a separate state and process.
        /// </summary>
        public virtual void CalculateVelocity(float delta)
        {
            Vector3 velocity = playerBody3D.Velocity;

            //If not on the floor, apply gravity
            if (!playerBody3D.IsOnFloor())
                velocity += playerBody3D.GetGravity() * delta;

            if (Input.IsActionJustPressed("jump") && playerBody3D.IsOnFloor())
                velocity.Y = stateMachine.JumpVelocity;

            Vector3 direction = GetMovementDirection();

            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * stateMachine.MoveSpeed;
                velocity.Z = direction.Z * stateMachine.MoveSpeed;

                LookTowardDirection(direction, delta);
            }
            else
            {
                velocity.X = Mathf.MoveToward(playerBody3D.Velocity.X, 0, stateMachine.MoveSpeed);
                velocity.Z = Mathf.MoveToward(playerBody3D.Velocity.Z, 0, stateMachine.MoveSpeed);
            }

            playerBody3D.Velocity = velocity;

            stateMachine.UpdateMovementAnimTree(direction, velocity.Length(), delta);
        }

        public Vector3 GetMovementDirection()
        {
            Vector2 inputDir = Input.GetVector(
                "move_left", "move_right", "move_forward", "move_backward");

            Vector3 inputVector = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();

            return horizontalPivot.GlobalTransform.Basis * inputVector;
        }

        public void LookTowardDirection(Vector3 direction, float delta)
        {
            Transform3D targetTransform = stateMachine.RigPivot.GlobalTransform.LookingAt(
                stateMachine.RigPivot.GlobalPosition + direction, Vector3.Up, true);

            stateMachine.RigPivot.GlobalTransform = stateMachine.RigPivot.GlobalTransform.InterpolateWith(
                targetTransform, 1.0f - Mathf.Exp(-stateMachine.AnimInterpolationDecay * delta)
            );
        }
    }
}