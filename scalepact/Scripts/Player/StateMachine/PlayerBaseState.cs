using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerBaseState : State
    {
        protected new PlayerStateMachine stateMachine;
        protected Vector3 attackDirection = Vector3.Zero;

        public override void _Ready()
        {
            if (GetParent() != null && GetParent().HasMethod("ChangeState"))
                stateMachine = (PlayerStateMachine)GetParent();
            else
                GD.PushError($"Parent is not a valid StateMachine for state: {Name}");

            SetProcessesOnState(false);
        }
    }
}