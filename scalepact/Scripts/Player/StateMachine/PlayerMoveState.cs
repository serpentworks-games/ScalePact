using Godot;

namespace Scalepact.Player
{
	public partial class PlayerMoveState : PlayerBaseState
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
			if (Input.IsActionPressed("attack"))
			{
				stateMachine.ChangeToAttack();
				return;
			}
			if (Input.IsActionPressed("jump"))
			{
				stateMachine.ChangeToJump();
				return;
			}

			Vector3 velocity = stateMachine.PlayerCharBody3D.Velocity;

			Vector3 direction = stateMachine.GetMovementDirection();

			velocity = stateMachine.ApplyGroundMovementVelocity(direction, velocity, (float)delta);

			velocity = stateMachine.ApplyGravity((float)delta, velocity);

			stateMachine.PlayerCharBody3D.Velocity = velocity;

			stateMachine.UpdateMovementAnimTree(velocity.Length(), (float)delta);

			stateMachine.PlayerCharBody3D.MoveAndSlide();
		}

		public override void ExitState()
		{
			base.ExitState();
		}
	}
}