using Godot;
using System;

namespace Scalepact.Combat
{
    public partial class AttackDetection : RayCast3D
    {
        public void DealDamage()
        {
            if (!IsColliding()) return;

            var collider = GetCollider();
            if (collider == null) return;
            GD.Print($"We hit {collider}!");

            if (collider is CollisionObject3D)
            {
                AddException((CollisionObject3D)collider);
            }

        }
    }
}