using Godot;
using Scalepact.Utilities;

namespace Scalepact.DamageSystem
{
    public partial class Damager : ShapeCast3D
    {
        [Export] float damage;
        [Export] CollisionObject3D selfCollider;
        [Export] bool isInstaKill;

        public void DealDamage()
        {
            for (int i = 0; i < GetCollisionCount(); i++)
            {
                var collider = GetCollider(i);

                if (collider == selfCollider || selfCollider == null) continue;

                if (collider is CollisionObject3D col)
                {
                    if (col is EntityAccessor entity)
                    {
                        if (entity == null) continue;

                        if (isInstaKill)
                        {
                            entity.HealthComponent.InstaKill();
                        }
                        entity.HealthComponent.TakeDamage(damage);
                    }
                    AddException(col);
                }
            }
        }
    }
}