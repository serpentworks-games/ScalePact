using Godot;
using Scalepact.Combat;
using Scalepact.StateMachines;
using System;

namespace Scalepact.Enemies
{
    public partial class EnemyStateMachine : StateMachine
    {
        [Export] public float MaxHealth { get; private set; } = 5f;

        public HealthComponent HealthComponent { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public CollisionShape3D CollisionShape { get; private set; }

        public override void _Ready()
        {
            HealthComponent = GetNode<HealthComponent>("../HealthComponent");
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");
            CollisionShape = GetNode<CollisionShape3D>("../CollisionShape3D");

            GD.Print("Setting health~ " + MaxHealth);
            HealthComponent.UpdateMaxHealth(MaxHealth);

            HealthComponent.TriggerDefeat += OnHealthComponentTriggerDefeat;
        }

        public void OnHealthComponentTriggerDefeat()
        {
            GD.Print("And thus, I die.");
            OneShotAnimationRequest("parameters/Death/request");
        }

        public void OneShotAnimationRequest(string animName)
        {
            AnimationTree.Set(animName, (int)AnimationNodeOneShot.OneShotRequest.Fire);
            CollisionShape.Disabled = true;
        }
    }
}