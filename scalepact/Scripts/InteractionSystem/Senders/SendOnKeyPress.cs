using Godot;

namespace Scalepact.InteractionSystem.Senders
{
    public partial class SendOnKeyPress : SendInteractionCommand
    {
        const string kInteractionInputKey = "interact";

        public void OnInteractAreaEntered(Node3D body)
        {
            canUse = true;
        }

        public void OnInteractAreaExited(Node3D body)
        {
            canUse = false;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (@event.IsActionPressed(kInteractionInputKey) && canUse)
            {
                SendInteraction();
            }
        }
    }
}