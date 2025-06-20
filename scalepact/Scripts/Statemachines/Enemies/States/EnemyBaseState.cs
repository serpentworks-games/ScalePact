using Godot;

namespace Scalepact.StateMachines.Enemies
{
    public partial class EnemyBaseState : State
    {
        protected new EnemyStateMachine stateMachine;
        protected Vector3 attackDirection = Vector3.Zero;

        public override void _Ready()
        {
            if (GetParent() != null && GetParent().HasMethod("ChangeState"))
                stateMachine = (EnemyStateMachine)GetParent();
            else
                GD.PushError($"Parent is not a valid StateMachine for state: {Name}");

            SetProcessesOnState(false);
        }

        public void MoveNavigationAgent(Vector3 targetPos, float moveSpeed)
        {
            stateMachine.OrientRig(stateMachine.Agent3D.GetNextPathPosition());

            Vector3 targetVelocity = Vector3.Zero;
            stateMachine.Agent3D.TargetPosition = targetPos;

            if (!stateMachine.Agent3D.IsTargetReached())
            {
                targetVelocity = stateMachine.GetLocalNavigationDirection() * moveSpeed;
                stateMachine.OrientRig(stateMachine.Agent3D.GetNextPathPosition());
            }

            stateMachine.Agent3D.Velocity = targetVelocity;
        }
    }
}