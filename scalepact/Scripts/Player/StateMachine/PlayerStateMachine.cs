using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerStateMachine : StateMachine
    {
        [Export] public float MoveSpeed { get; private set; } = 5f;
        [Export] public float JumpVelocity { get; private set; } = 4.5f;

        public CharacterBody3D PlayerCharBody3D { get; private set; }
        public CameraController CameraController { get; private set; }

        public override void _Ready()
        {
            PlayerCharBody3D = GetParent<CharacterBody3D>();
            CameraController = GetNode<CameraController>("../CamController");

            base._Ready();
        }
    }
}