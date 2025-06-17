using Godot;
using Scalepact.Player;
using System;

namespace Scalepact.InteractionSystem
{
    public partial class InteractionArea : Area3D
    {
        [Export] public InteractionType InteractOn { get; private set; } = InteractionType.Collision;
        [Export] public InteractionAction[] Actions { get; private set; } = [];

        bool canUse;

        const string kInteractionInputKey = "interact";

        public void OnInteractAreaEntered(Node3D body)
        {
            if (InteractOn == InteractionType.InputAction)
            {
                canUse = true;
            }
            else
            {
                OnInteraction();
            }
        }

        public void OnInteractAreaExited(Node3D body)
        {
            if (InteractOn == InteractionType.InputAction)
            {
                canUse = false;
            }
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed(kInteractionInputKey) && canUse)
            {
                OnInteraction();
            }
        }

        void OnInteraction()
        {
            for (int i = 0; i < Actions.Length; i++)
            {
                Actions[i].OnInteract();
            }
        }
    }
}
