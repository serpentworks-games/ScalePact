using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerBaseState : State
    {
        protected new PlayerStateMachine stateMachine;

        //Local ref to the CharacterBody3D from the statemachine to avoid overly long names
        CharacterBody3D playerBody3D;

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

            Vector2 inputDir = Input.GetVector(
                "move_left", "move_right", "move_forward", "move_backward");
            Vector3 direction =
                (playerBody3D.Transform.Basis *
                    new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * stateMachine.MoveSpeed;
                velocity.Z = direction.Z * stateMachine.MoveSpeed;
            }
            else
            {
                velocity.X = Mathf.MoveToward(playerBody3D.Velocity.X, 0, stateMachine.MoveSpeed);
                velocity.Z = Mathf.MoveToward(playerBody3D.Velocity.Z, 0, stateMachine.MoveSpeed);
            }

            playerBody3D.Velocity = velocity;
        }
    }
}