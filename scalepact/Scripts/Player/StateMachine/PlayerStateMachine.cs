using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerStateMachine : StateMachine
    {
        [Export] public float MoveSpeed { get; private set; } = 5f;
        [Export] public float JumpVelocity { get; private set; } = 4.5f;
        [Export] public float AnimInterpolationDecay { get; private set; } = 20f;
        [Export] public float AnimBlendWeight { get; private set; } = 5f;

        public CharacterBody3D PlayerCharBody3D { get; private set; }
        public CameraController CameraController { get; private set; }
        public Node3D RigPivot { get; private set; }
        public AnimationTree AnimationTree { get; private set; }

        public override void _Ready()
        {
            PlayerCharBody3D = GetParent<CharacterBody3D>();
            CameraController = GetNode<CameraController>("../CamController");
            RigPivot = GetNode<Node3D>("../RigPivot");
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");

            base._Ready();
        }

        public void UpdateMovementAnimTree(Vector3 direction, float velocity, float delta)
        {
            AnimationTree.Set(
                PlayerStringRefs.PlayerMoveBlendValue,
                Mathf.MoveToward(
                    (float)AnimationTree.Get(PlayerStringRefs.PlayerMoveBlendValue),
                    Mathf.Clamp(velocity, 0, 1),
                    delta * AnimBlendWeight
                ));
        }

    }
}