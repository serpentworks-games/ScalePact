using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
	public partial class PlayerIdleState : State
	{
		public virtual void EnterState()
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

		public virtual void ExitState()
		{
			base.ExitState();
		}

		public override void _Input(InputEvent @event)
		{
			if (Input.IsActionPressed("ui_left") || Input.IsActionPressed("ui_right"))
			{
				ChangeState(PlayerStringRefs.PlayerMoveState);
			}
		}
	}
}