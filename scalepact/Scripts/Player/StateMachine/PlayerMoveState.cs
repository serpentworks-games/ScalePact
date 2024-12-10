using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
	public partial class PlayerMoveState : State
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
			base._PhysicsProcess(delta);
		}

		public override void ExitState()
		{
			base.ExitState();
		}

		public override void _Input(InputEvent @event)
		{
			if (Input.IsActionPressed("ui_up"))
			{
				ChangeState(PlayerStringRefs.PlayerIdleState);
			}
		}
	}
}