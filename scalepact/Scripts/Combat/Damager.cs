using Godot;
using Scalepact.Utilities;

namespace Scalepact.Combat
{
    public partial class Damager : ShapeCast3D
    {
        [Export] float damage;
        [Export] CollisionObject3D selfCollider;

        public void DealDamage()
        {
            GD.Print("Trying to deal damage...");

            for (int i = 0; i < GetCollisionCount(); i++)
            {
                var collider = GetCollider(i);

                GD.Print("Hitting: " + collider);
                if (collider == selfCollider) continue;

                if (collider is CollisionObject3D col)
                {
                    if (col is EntityAccessor entity)
                    {
                        entity.HealthComponent.TakeDamage(damage);
                    }
                    AddException(col);
                }
            }
        }
    }
}