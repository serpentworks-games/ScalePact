using System;
using Godot;
using Scalepact.Utilities;

namespace Scalepact.DamageSystem
{
    public partial class AreaDamager : Area3D
    {
        [Export] float damageAmount;
        [Export] float timeBetweenDamageTriggers;
        [Export] bool isInstaKill;

        bool inArea;
        float timeSinceLastDamageTrigger;

        public void OnAreaEntered(CharacterBody3D body3D)
        {
            inArea = true;

            if (body3D is EntityAccessor entity)
            {
                if (isInstaKill)
                {
                    entity.HealthComponent.InstaKill();
                }
                entity.HealthComponent.TakeDamage(damageAmount);
            }
        }

        public override void _PhysicsProcess(double delta)
        {
            if (inArea)
            {
                for (int i = 0; i < GetOverlappingBodies().Count; i++)
                {
                    var bodies = GetOverlappingBodies();

                    if (bodies[i] is EntityAccessor entity)
                    {
                        if (isInstaKill)
                            entity.HealthComponent.InstaKill();
                        else
                        {
                            timeSinceLastDamageTrigger += (float)delta;

                            if (timeSinceLastDamageTrigger > timeBetweenDamageTriggers)
                            {
                                timeSinceLastDamageTrigger = 0f;
                                entity.HealthComponent.TakeDamage(damageAmount);
                            }
                        }
                    }
                }
            }
        }

        public void OnAreaExited(CharacterBody3D body3D)
        {
            inArea = false;
        }
    }
}