using Godot;
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

			velocity = stateMachine.AddGravity(velocity, (float)delta);

			velocity = stateMachine.ApplyMovement(direction, velocity, stateMachine.MoveSpeed, (float)delta);

			stateMachine.PlayerCharBody3D.Velocity = velocity;

			stateMachine.PlayerCharBody3D.MoveAndSlide();

			if (stateMachine.PlayerCharBody3D.Velocity.X == 0 && stateMachine.PlayerCharBody3D.Velocity.Z == 0)
			{
				stateMachine.UpdateMovementBlendValue(
					PlayerStringRefs.PlayerMoveBlendValue, 0, stateMachine.AnimBlendWeight, (float)delta);
			}
			else
			{
				stateMachine.UpdateMovementBlendValue(
					PlayerStringRefs.PlayerMoveBlendValue, 1, stateMachine.AnimBlendWeight, (float)delta);
			}

		}

		public override void ExitState()
		{
			base.ExitState();
		}
	}
}