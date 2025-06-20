using Godot;

public partial class ItemPickup : Area3D
{
    [Export] float pickupRotationSpeed = 3f;
    [Export] Node3D pickupMesh;

    public override void _Ready()
    {
        base._Ready();
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        ///TODO: Proper functionality! This is mostly to test things
        /// Add to Gem count
        /// Particle effects, etc

        pickupMesh.RotateY(Mathf.DegToRad(pickupRotationSpeed));

        if (HasOverlappingBodies())
        {
            QueueFree();
        }
    }
}
