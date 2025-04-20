using Godot;
namespace Scalepact.StateMachines
{
    public partial class StateMachine : Node
    {
        [Export] State initialState;

        protected State currentState;
        protected AnimationTree animationTree;

        public override void _Ready()
        {
            animationTree = GetNode<AnimationTree>("../AnimationTree");
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

            currentState?.SetProcessesOnState(false);
            currentState?.ExitState();

            currentState = newState;

            currentState?.SetProcessesOnState(true);
            currentState?.EnterState();
        }

        public void OneShotAnimationRequest(string animName)
        {
            animationTree.Set(animName, (int)AnimationNodeOneShot.OneShotRequest.Fire);
        }

        public void AnimationTransitionRequest(string propertyPath, string state)
        {
            animationTree.Set(propertyPath, state);
        }

        public bool IsOneShotAnimationActive(string animPath)
        {
            return (bool)animationTree.Get(animPath);
        }

        public void UpdateMovementBlendValue(string moveBlendValueName, float velocity, float animBlendWeight, float delta)
        {
            animationTree.Set(
                moveBlendValueName,
                Mathf.MoveToward(
                    (float)animationTree.Get(moveBlendValueName),
                    Mathf.Clamp(velocity, 0, 1),
                    delta * animBlendWeight
                )
            );
        }
    }
}