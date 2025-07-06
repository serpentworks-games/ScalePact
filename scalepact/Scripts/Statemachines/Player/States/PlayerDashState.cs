namespace Scalepact.StateMachines.Player
{
    public partial class PlayerDashState : PlayerBaseState
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
            if (stateMachine.DashAbility.IsAbilityDurationTimerStopped())
            {
                stateMachine.ChangeToGroundMovement();
                return;
            }

            GetVelocityAndDirection();

            velocity = stateMachine.AddGravity(velocity, (float)delta);

            velocity = stateMachine.ApplyMovement(
                direction,
                velocity,
                stateMachine.MoveSpeed * stateMachine.DashAbility.DashSpeedBoost,
                (float)delta);

            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}