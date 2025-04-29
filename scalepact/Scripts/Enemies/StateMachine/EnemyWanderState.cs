using Godot;

namespace Scalepact.Enemies
{
    public partial class EnemyWanderState : EnemyBaseState
    {
        private float timeSinceArrivedatWaypoint = Mathf.Inf;

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
            timeSinceArrivedatWaypoint += (float)delta;

            base._PhysicsProcess(delta);
            if (stateMachine.IsInRange(stateMachine.ChaseRange))
            {
                stateMachine.ChangeToChaseState();
                return;
            }

            Vector3 nextPos = stateMachine.OriginalPosition;

            if (stateMachine.IsAtTargetPoint(stateMachine.GetCurrentLocationInArea()))
            {
                timeSinceArrivedatWaypoint = 0;
                stateMachine.GetNewPointInArea();
                stateMachine.StopAgent();
            }

            nextPos = stateMachine.GetCurrentLocationInArea();
            stateMachine.LastWaypointPos = nextPos;

            if (timeSinceArrivedatWaypoint > stateMachine.PointDwellTime)
            {
                MoveNavigationAgent(nextPos, stateMachine.BaseMoveSpeed);
            }
        }

        public override void ExitState()
        {
            base.ExitState();
        }
    }
}