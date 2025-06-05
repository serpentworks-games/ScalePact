using Godot;

namespace Scalepact.Player
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

            velocity = stateMachine.ResolveMovementPhysics(
               direction, velocity, stateMachine.MoveSpeed * stateMachine.DashAbility.DashSpeedBoost, (float)delta);

            GroundedCharacterMovement(velocity, delta);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}