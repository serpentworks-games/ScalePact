using Godot;
using Scalepact.Abilities.PlayerAbilities;

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
               direction, velocity, stateMachine.MoveSpeed * dashAbility.DashSpeedBoost, (float)delta);

            GroundedCharacterMovement(velocity, delta);
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}