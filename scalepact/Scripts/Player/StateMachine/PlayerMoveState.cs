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
			if (Input.IsActionPressed("jump"))
			{
				stateMachine.ChangeToJump();
				return;
			}

			GetVelocityAndDirection();

			velocity = stateMachine.ResolveMovementPhysics(
				direction, velocity, stateMachine.MoveSpeed, (float)delta);

			GroundedCharacterMovement(delta, velocity);

			stateMachine.UpdateMovementAnimTree(velocity.Length(), (float)delta);
		}

		public override void ExitState()
		{
			base.ExitState();
		}
	}
}