using Scalepact.Core;

namespace Scalepact.StateMachines.Player
{
    public partial class PlayerAttackState : PlayerBaseState
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

            if (stateMachine.MeleeBiteAbility.IsAbilityDurationTimerStopped() &&
                !stateMachine.IsOneShotAnimationActive(PlayerStringRefs.PlayerMeleeAttackIsActive))
            {
                stateMachine.ChangeToGroundMovement();
                return;
            }

            GetVelocityAndDirection();

            velocity = stateMachine.AddGravity(velocity, (float)delta);

            velocity = stateMachine.ApplyAttackMovement(
                direction, velocity, stateMachine.MeleeAttackMoveSpeed, (float)delta);
            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();

        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}