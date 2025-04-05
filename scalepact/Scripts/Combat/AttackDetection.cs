using Godot;
using Scalepact.Utilities;

namespace Scalepact.Combat
{
    public partial class AttackDetection : RayCast3D
    {
        [Export] float damage;
        [Export] CollisionObject3D selfCollider;

        public void DealDamage()
        {
            GD.Print("Are we colliding?");
            if (!IsColliding()) return;

            var collider = GetCollider();
            GD.Print("We are colliding, checking if the collider exists");
            if (collider == null) return;

            GD.Print("Collider exists, checking if it's my own");
            if (collider == selfCollider) return;

            GD.Print("Collider isn't mine, continuing");

            if (collider is CollisionObject3D col)
            {
                GD.Print("Is collider have an accessor?");
                if (collider is EntityAccessor entity)
                {
                    GD.Print("Trying to apply damage on " + entity);
                    entity.HealthComponent.TakeDamage(damage);
                }

                AddException(col);
            }
        }
    }
}