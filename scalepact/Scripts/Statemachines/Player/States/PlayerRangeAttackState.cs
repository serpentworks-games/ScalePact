namespace Scalepact.StateMachines.Player
{
    public partial class PlayerRangeAttackState : PlayerBaseState
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

            if (stateMachine.FireBallAbility.IsAbilityDurationTimerStopped())
            {
                stateMachine.ChangeToGroundMovement();
                return;
            }

            GetVelocityAndDirection();

            velocity = stateMachine.AddGravity(velocity, (float)delta);

            velocity = stateMachine.ApplyAttackMovement(
                direction, velocity, 0, (float)delta);
            stateMachine.PlayerCharBody3D.Velocity = velocity;

            stateMachine.PlayerCharBody3D.MoveAndSlide();


        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}