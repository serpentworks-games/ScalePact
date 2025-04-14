using Godot;
using Scalepact.Abilities;

namespace Scalepact.Player
{
    public partial class PlayerDashState : PlayerBaseState
    {
        [Export] DashAbility dashAbility;

        public override void EnterState()
        {
            base.EnterState();
            GD.Print("Zoom!");
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!dashAbility.IsInAbility)
            {
                stateMachine.ChangeToGroundMovement();
                return;
            }

            GetVelocityAndDirection();

            velocity = stateMachine.ResolveMovementPhysics(
                velocity, dashAbility.MoveDir,
                stateMachine.MoveSpeed + dashAbility.DashSpeedBoost, (float)delta);

            GroundedCharacterMovement(delta, velocity);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}