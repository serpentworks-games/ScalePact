using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Enemies
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
    }
}