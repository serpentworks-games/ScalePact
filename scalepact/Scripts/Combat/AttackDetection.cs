using Godot;
using Scalepact.Utilities;

namespace Scalepact.Combat
{
    public partial class AttackDetection : RayCast3D
    {
        [Export] float damage;

        public void DealDamage()
        {
            if (!IsColliding()) return;

            var collider = GetCollider();

            if (collider == null) return;

            if (collider is CollisionObject3D col)
            {
                if (collider is EnemyEntityAccessor enemy)
                {
                    enemy.HealthComponent.TakeDamage(damage);
                }

                AddException(col);
            }
        }
    }
}