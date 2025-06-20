using Godot;
using Scalepact.Core;

namespace Scalepact.DamageSystem
{
    public partial class Damager : Area3D
    {
        [Export] CollisionObject3D selfCollider;
        [Export] float timeBetweenDamageTriggers = -1.0f;
        [Export] bool isInstaKill;

        protected int damageAmount;
        protected bool isInDamageArea;
        protected float timeSinceLastDamageTrigger = Mathf.Inf;

        CollisionShape3D areaShape;

        public virtual void SetupDamager(int damageAmount, bool shouldToggleCollider)
        {
            this.damageAmount = damageAmount;
            if (shouldToggleCollider)
            {
                areaShape = GetNode<CollisionShape3D>("CollisionShape3D");
                ToggleCollider(true);
            }
        }

        public void ToggleCollider(bool newState)
        {
            areaShape.Disabled = !newState;
        }

        public void OnAreaEntered(Node3D node3D)
        {
            isInDamageArea = true;
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);

            if (isInDamageArea)
            {
                for (int i = 0; i < GetOverlappingBodies().Count; i++)
                {
                    var bodies = GetOverlappingBodies();

                    if (bodies[i] is CollisionObject3D col)
                    {
                        if (col == selfCollider || col == null) continue;

                        if (col is EntityAccessor entity)
                        {
                            if (isInstaKill)
                            {
                                entity.HealthComponent.InstaKill();
                            }
                            else
                            {
                                if (timeBetweenDamageTriggers == -1.0f)
                                {
                                    entity.HealthComponent.TakeDamage(damageAmount);
                                }
                                else
                                {
                                    timeSinceLastDamageTrigger += (float)delta;
                                    if (timeSinceLastDamageTrigger > timeBetweenDamageTriggers)
                                    {
                                        timeSinceLastDamageTrigger = 0.0f;
                                        entity.HealthComponent.TakeDamage(damageAmount);
                                    }
                                }
                            }
                        }
                    }
                }

            }
        }

        public void OnAreaExited(Node3D node3D)
        {
            isInDamageArea = false;
        }
    }
}