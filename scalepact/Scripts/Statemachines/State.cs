using Godot;

namespace Scalepact.StateMachines
{
    public partial class State : Node
    {
        StateMachine stateMachine;

        public override void _Ready()
        {
            if (GetParent() != null && GetParent().HasMethod("ChangeState"))
                stateMachine = (StateMachine)GetParent();
            else
                GD.PushError($"Parent is not a valid StateMachine for state: {Name}");

            SetProcessesOnState(false);
        }

        public void SetProcessesOnState(bool setting)
        {
            SetProcess(setting);
            SetPhysicsProcess(setting);
            SetProcessInput(setting);
            SetProcessUnhandledInput(setting);
            SetProcessUnhandledKeyInput(setting);
        }

        public virtual void EnterState()
        {
            GD.Print($"Entered state: {Name}");
            return;
        }

        public override void _Process(double delta)
        {
            //GD.Print($"Processing tick on state: {Name}");
            base._Process(delta);

        }

        public override void _PhysicsProcess(double delta)
        {
            //GD.Print($"Processing physics tick on state: {Name}");
            base._PhysicsProcess(delta);

        }


        public virtual void ExitState()
        {
            GD.Print($"Exited state: {Name}");
            return;
        }

        public void ChangeState(string state)
        {
            if (stateMachine != null)
                stateMachine.ChangeState(state);
            else
                GD.PushError($"StateMachine reference is not set for state: {Name}");
        }
    }
}