using System.Diagnostics;
using Godot;
using Scalepact.DamageSystem;

namespace Scalepact.Abilities
{
    public partial class ProjectileCombatAbility : AbilityBase
    {
        [Export] protected int damageAmount;
        [Export] PackedScene projectileScene;
        [Export] Node3D projectileStartLocation;

        public override void StartAbility()
        {
            base.StartAbility();
            InstantiateProjectile();
            GD.Print("Starting projectile!");
        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
            GD.Print("Processing Ability: " + this.Name);
            GD.Print("Remaining time: " + abilityDurationTimer.TimeLeft);
        }

        public override void ResolveAbility()
        {
            GD.Print("Resolving projectile");
            base.ResolveAbility();
        }

        public void InstantiateProjectile()
        {
            GD.Print("Firing projectile!");
            Projectile projectile = projectileScene.Instantiate<Projectile>();

            projectile.SetupProjectile(projectileStartLocation.Position, damageAmount);
            GetTree().CurrentScene.AddChild(projectile);
            projectile.Transform = projectileStartLocation.GlobalTransform;
        }
    }
}