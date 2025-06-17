using Godot;
using System;

namespace Scalepact.InteractionSystem
{
    public partial class InteractionAction : Node3D
    {
        [Export] public bool IsOneShot { get; private set; } = false;
        [Export] public float ActionStartDelay { get; private set; } = 0;
        //[Export] public float ActionCooldown { get; private set; } = -1f;

        float timeSinceTriggered = Mathf.Inf;
        protected bool isTriggered = false;

        public virtual void PerformInteraction() { }

        public virtual void OnInteract()
        {
            if (IsOneShot && isTriggered) return;

            isTriggered = true;

            ExecuteInteraction();
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
