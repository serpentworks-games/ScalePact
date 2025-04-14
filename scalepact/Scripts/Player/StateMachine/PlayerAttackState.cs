using Godot;

namespace Scalepact.Player
{
    public partial class PlayerAttackState : PlayerBaseState
    {
        public override void EnterState()
        {
            base.EnterState();
            stateMachine.ApplyMeleeAttack();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (!stateMachine.IsAttackActive())
            {
                stateMachine.ChangeState(PlayerStringRefs.PlayerMoveState);
                return;
            }

            GetVelocityAndDirection();

            velocity = stateMachine.ApplyAttackMovement(
                velocity, direction, stateMachine.MeleeAttackMoveSpeed, (float)delta);

            GroundedCharacterMovement(delta, velocity);

        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}