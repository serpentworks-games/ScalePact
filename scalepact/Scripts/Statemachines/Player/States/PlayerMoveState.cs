using Scalepact.Core;

namespace Scalepact.StateMachines.Player
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
			GetVelocityAndDirection();

			velocity = stateMachine.ResolveMovementPhysics(
				direction, velocity, stateMachine.MoveSpeed, (float)delta);

			GroundedCharacterMovement(velocity, delta);

			stateMachine.UpdateMovementBlendValue(
				PlayerStringRefs.PlayerMoveBlendValue,
				(float)velocity.Length(),
				stateMachine.AnimBlendWeight,
				(float)delta
			);
		}

		public override void ExitState()
		{
			base.ExitState();
		}
	}
}