using Godot;

namespace Scalepact.InteractionSystem.Actions
{
    public partial class PlayNodeAnimationAction : InteractionAction
    {
        [Export] string animName;
        [Export] AnimationPlayer animationPlayer;

        public override void PerformInteraction()
        {
            animationPlayer.Play(animName);
        }
    }

}