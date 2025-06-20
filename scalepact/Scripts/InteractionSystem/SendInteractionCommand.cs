using Godot;

namespace Scalepact.InteractionSystem
{
    public partial class SendInteractionCommand : Node
    {
        [Export] public InteractionActionType InteractionActionType { get; private set; }
        [Export] public InteractionReceiver InteractionReceiver { get; private set; }
        [Export] public bool IsOneShot { get; private set; } = false;
        [Export] public float CommandCooldown { get; private set; } = 1f;

        bool isTriggered = false;
        float lastSendTime;
        protected bool canUse = false;

        public void SendInteraction()
        {
            if (InteractionReceiver == null) return;

            if (IsOneShot && isTriggered) return;

            if (Time.GetTicksMsec() - lastSendTime < CommandCooldown) return;

            isTriggered = true;

            lastSendTime = Time.GetTicksMsec();

            InteractionReceiver.Receive(InteractionActionType);
        }
    }
}