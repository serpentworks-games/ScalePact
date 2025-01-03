using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerStateMachine : StateMachine
    {
        [Export] public float MoveSpeed { get; private set; } = 5f;
        [Export] public float JumpVelocity { get; private set; } = 4.5f;
        [Export] public float AnimInterpolationDecay { get; private set; } = 20f;

        public CharacterBody3D PlayerCharBody3D { get; private set; }
        public CameraController CameraController { get; private set; }
        public Node3D RigPivot { get; private set; }

        public override void _Ready()
        {
            PlayerCharBody3D = GetParent<CharacterBody3D>();
            CameraController = GetNode<CameraController>("../CamController");
            RigPivot = GetNode<Node3D>("../RigPivot");

            base._Ready();
        }


    }
}