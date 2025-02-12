namespace Scalepact.Enemies
{
    public partial class EnemyAttackState : EnemyBaseState
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
            if (!stateMachine.IsOneShotAnimationActive("parameters/AttackTrigger/active"))
            {
                stateMachine.ChangeState("MoveState");
                return;
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }

    }
}