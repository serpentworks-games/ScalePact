using Godot;
using System;

namespace Scalepact.Gameplay
{
    public partial class WanderArea : Area3D
    {
        public Vector3 GeneratedPoint { get; private set; }

        float areaWidth, areaDepth, areaRadius;

        CollisionShape3D areaCollider;

        public override void _Ready()
        {
            base._Ready();
            areaCollider = GetNode<CollisionShape3D>("CollisionShape3D");

            if (areaCollider.Shape == null)
            {
                GD.PushError("No valid collision shape found. Please create a new one in editor");
                return;
            }

            switch (areaCollider.Shape)
            {
                case BoxShape3D:
                    {
                        Vector3 size = (Vector3)areaCollider.Shape.Get("size");
                        areaWidth = size.X;
                        areaDepth = size.Z;
                        break;
                    }

                default:
                    GD.PushError("No valid collision shape. Must be a box!");
                    return;
            }

            GeneratedPoint = GlobalPosition;
        }

        public void GenerateRandomPoint()
        {
            float x = (float)GD.RandRange(-areaWidth / 2, areaWidth / 2);
            float z = (float)GD.RandRange(-areaDepth / 2, areaDepth / 2);

            Vector3 point = new Vector3(x, 0, z);
            GeneratedPoint = point + GlobalPosition;
        }
    }
}

