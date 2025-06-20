using Godot;

namespace Scalepact.InteractionSystem
{
    public partial class InteractionAction : Node3D
    {
        [Export] public InteractionActionType InteractionActionType { get; private set; } = InteractionActionType.None;
        [Export] public bool IsOneShot { get; private set; } = false;
        [Export] public float ActionStartDelay { get; private set; } = 0;
        [Export] public float ActionCooldown { get; private set; } = 0;

        float startTime = 0;
        protected bool isTriggered = false;

        public override void _Ready()
        {
            GetNode<InteractionReceiver>("../").Register(InteractionActionType, this);
        }


        public virtual void PerformInteraction() { }

        public virtual void OnInteract()
        {
            if (IsOneShot && isTriggered) return;

            isTriggered = true;

            if (ActionCooldown > 0)
            {
                if (Time.GetTicksMsec() > startTime + ActionCooldown)
                {
                    startTime = Time.GetTicksMsec() + ActionStartDelay;
                    ExecuteInteraction();
                }
            }
            else
            {
                ExecuteInteraction();
            }
        }

        void ExecuteInteraction()
        {
            if (ActionStartDelay > 0)
            {
                SceneTreeTimer timer = GetTree().CreateTimer(ActionStartDelay);
                timer.Timeout += PerformInteraction;
            }
            else
            {
                PerformInteraction();
            }
        }
    }
}
