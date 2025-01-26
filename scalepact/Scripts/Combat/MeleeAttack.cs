using Godot;
using System;

namespace Scalepact.Combat
{
    public partial class MeleeAttack : RayCast3D
    {
        public void DealDamage()
        {
            if (!IsColliding()) return;

            CollisionObject3D collider = (CollisionObject3D)GetCollider();
            if (collider == null) return;
            GD.Print($"We hit {collider}!");
            AddException(collider);
        }
    }
}