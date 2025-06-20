using Godot;

namespace Scalepact.Core
{
	public partial class CameraController : Node3D
	{
		[ExportCategory("Camera Variables")]
		[Export] public float MouseSensitivity { get; private set; } = 0.001f;
		[Export] public float InterpolationDecay { get; private set; } = 10f;
		[Export] public float UpperCameraBround { get; private set; } = -60f;
		[Export] public float LowerCameraBound { get; private set; } = -5f;

		[ExportGroup("References")]
		[Export] public Node3D HorizontalPivot { get; private set; }
		[Export] public Node3D VerticalPivot { get; private set; }
		[Export] public SpringArm3D SpringArm3D { get; private set; }

		public Vector2 LookDir { get; private set; } = Vector2.Zero;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			Input.MouseMode = Input.MouseModeEnum.Captured;
		}

		public override void _PhysicsProcess(double delta)
		{
			HandleCameraRotation();
			HandleSpringArmRotation((float)delta);
		}

		public override void _UnhandledInput(InputEvent @event)
		{
			if (@event.IsActionPressed("ui_cancel"))
				Input.MouseMode = Input.MouseModeEnum.Visible;

			if (Input.MouseMode == Input.MouseModeEnum.Captured)
			{

				if (@event is InputEventMouseMotion)
				{
					var mouseEvent = @event as InputEventMouseMotion;
					LookDir = -mouseEvent.Relative * MouseSensitivity;
				}
			}
		}

		void HandleCameraRotation()
		{
			HorizontalPivot.RotateY(LookDir.X);
			VerticalPivot.RotateX(LookDir.Y);

			Vector3 vertRotation = VerticalPivot.Rotation;

			float clampedVerticalRotation = Mathf.Clamp(
				VerticalPivot.Rotation.X,
				Mathf.DegToRad(UpperCameraBround),
				Mathf.DegToRad(LowerCameraBound));

			vertRotation.X = clampedVerticalRotation;

			VerticalPivot.Rotation = vertRotation;

			LookDir = Vector2.Zero;
		}

		void HandleSpringArmRotation(float delta)
		{
			SpringArm3D.GlobalTransform = SpringArm3D.GlobalTransform.InterpolateWith(
				VerticalPivot.GlobalTransform,
				1.0f - Mathf.Exp(-InterpolationDecay * delta)
				);
		}
	}
}