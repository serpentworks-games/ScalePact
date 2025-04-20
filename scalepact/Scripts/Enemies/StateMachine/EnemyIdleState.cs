namespace Scalepact.Enemies
{
    public partial class EnemyIdleState : EnemyBaseState
    {
        public override void EnterState()
        {
            base.EnterState();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
            if (stateMachine.IsInRange(stateMachine.ChaseRange))
            {
                stateMachine.ChangeToChaseState();
                return;
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
        }

        public override void ExitState()
        {
            base.ExitState();

        }
    }
}