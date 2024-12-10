using Godot;
namespace Scalepact.StateMachines
{


    public partial class StateMachine : Node
    {
        [Export] State initialState;

        State currentState;

        public override void _Ready()
        {
            if (initialState == null)
            {
                if ((State)GetChild(0) != null)
                {
                    initialState = (State)GetChild(0);
                }
            }

            if (initialState != null)
                ChangeState(initialState.Name);
            else
                GD.PushError("No valid initial state found in StateMachine.");
        }

        public void ChangeState(string state)
        {
            State newState;
            if (HasNode(state))
            {
                newState = GetNode(state) as State;
            }
            else
            {
                GD.PushError($"{state} is an invalid state! Must be a state node's name");
                return;
            }

            if (currentState == newState) return;

            SetActiveState(newState);

        }

        void SetActiveState(State newState)
        {
            if (currentState != null)
            {
                currentState.SetProcessesOnState(false);
                currentState.ExitState();
            }

            currentState = newState;

            if (currentState != null)
            {
                currentState.SetProcessesOnState(true);
                currentState.EnterState();
            }


        }
    }
}