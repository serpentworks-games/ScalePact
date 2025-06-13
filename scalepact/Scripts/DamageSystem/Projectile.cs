using Godot;

namespace Scalepact.DamageSystem
{
    public partial class Projectile : CharacterBody3D
    {
        [Export] float projectileSpeed = 4f;
        [Export] float maxProjectileLifetime = 5f;

        Vector3 startPos;

        [Export] Damager damager;
        SceneTreeTimer timer;

        public override void _Ready()
        {
            base._Ready();
            timer = GetTree().CreateTimer(maxProjectileLifetime);
            timer.Timeout += QueueFree;
        }

        public override void _PhysicsProcess(double delta)
        {
            var velocity = Velocity;
            Velocity = Vector3.Left * projectileSpeed;
            Velocity = Velocity.Rotated(Vector3.Up, GlobalRotation.Y);
            MoveAndSlide();
        }

        public void SetupProjectile(Vector3 startPos, int damage)
        {
            this.startPos = startPos;
            Position = startPos;
            damager.SetupDamager(damage, false);
        }

        public void OnDamagerAreaEntered(Node3D node3D)
        {
            if (node3D != null)
            {
                //particle effects
                //await them finishing
                //secondary queue free
                QueueFree();
            }
        }

    }
}