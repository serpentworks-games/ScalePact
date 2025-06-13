using Scalepact.Player;

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

        velocity = stateMachine.ApplyAttackMovement(
            direction, velocity, 0, (float)delta);

        GroundedCharacterMovement(velocity, delta);

    }

    public override void ExitState()
    {
        base.ExitState();
    }
}