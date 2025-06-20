using Godot;

namespace Scalepact.InteractionSystem.Senders
{
    public partial class SendOnAreaEnter : SendInteractionCommand
    {
        public void OnInteractAreaEntered(Node3D body)
        {
            SendInteraction();
        }
    }
}