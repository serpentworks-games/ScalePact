namespace Scalepact.StateMachines.Enemies
{
    public partial class EnemySuspicionState : EnemyBaseState
    {
        float timeSinceEnteredState = 0;

        public override void EnterState()
        {
            base.EnterState();
            stateMachine.StopAgent();
        }

        public override void _Process(double delta)
        {
            base._Process(delta);
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            timeSinceEnteredState += (float)delta;

            if (timeSinceEnteredState > stateMachine.SuspicionTime)
            {
                MoveNavigationAgent(stateMachine.GetGuardOrLastWaypointPosition(), stateMachine.BaseMoveSpeed);

                if (stateMachine.IsAtTargetPoint(stateMachine.GetGuardOrLastWaypointPosition()))
                {
                    timeSinceEnteredState = 0;
                    stateMachine.ChangeToIdleState();
                    return;
                }
            }
            else
            {
                if (stateMachine.IsInRange(stateMachine.ChaseRange))
                {
                    stateMachine.ChangeToChaseState();
                    return;
                }
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}