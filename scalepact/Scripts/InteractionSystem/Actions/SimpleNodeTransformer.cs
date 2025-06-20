using Godot;
using Scalepact.Utilities;

namespace Scalepact.InteractionSystem.Actions
{
    public partial class SimpleNodeTransformer : InteractionAction
    {
        public enum LoopType
        {
            Once, PingPong, Repeat
        }

        [Export] public LoopType loopType;
        [Export] public Node3D nodeToMove;
        [Export] public float loopDuration = 1f;
        [Export] public Curve accelCurve;

        float time = 0f;
        float pos = 0f;
        float dir = 1f;

        public override void OnInteract()
        {
            if (IsOneShot && isTriggered) return;
            isTriggered = true;
        }

        public virtual void PerformTransform(float pos, float delta) { }

        public override void _PhysicsProcess(double delta)
        {
            if (isTriggered)
            {
                if (ActionStartDelay > 0)
                {

                }
                time += dir * (float)delta / loopDuration;
                switch (loopType)
                {
                    case LoopType.Once:
                        LoopOnce();
                        break;
                    case LoopType.PingPong:
                        LoopPingPong();
                        break;
                    case LoopType.Repeat:
                        LoopRepeat();
                        break;
                }
                PerformTransform(pos, (float)delta);
            }
        }

        void LoopOnce()
        {
            pos = UtilityFunctions.Clamp01(time);

            if (pos >= 1)
            {
                dir *= -1f;
                isTriggered = false;
            }
        }

        void LoopPingPong()
        {
            pos = UtilityFunctions.PingPong(time, 1f);
        }

        void LoopRepeat()
        {
            pos = UtilityFunctions.RepeatValue(time, 1f);
        }

    }
}