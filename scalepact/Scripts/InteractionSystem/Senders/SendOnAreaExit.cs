using Godot;

namespace Scalepact.InteractionSystem.Senders
{
    public partial class SendOnAreaExit : SendInteractionCommand
    {
        public void OnInteractAreaExited(Node3D body)
        {
            SendInteraction();
        }
    }
}