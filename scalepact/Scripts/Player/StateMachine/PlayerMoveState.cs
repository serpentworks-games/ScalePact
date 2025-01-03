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
			base._PhysicsProcess(delta);

			CalculateVelocity((float)delta);

			stateMachine.PlayerCharBody3D.MoveAndSlide();
		}

		public override void ExitState()
		{
			base.ExitState();
		}
	}
}