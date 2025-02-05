using Godot;
using Scalepact.Utilities;
using System;

namespace Scalepact.Combat
{
    public partial class AttackDetection : RayCast3D
    {
        [Export] float damage;

        public void DealDamage()
        {
            if (!IsColliding()) return;

            var collider = GetCollider();

            if (collider == null || collider is not CollisionObject3D col) return;

            if (collider is EnemyEntityAccessor enemy)
            {
                enemy.HealthComponent.TakeDamage(damage);
            }

            AddException(col);
        }
    }
}