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

        public override void _Ready()
        {
            HealthComponent = GetNode<HealthComponent>("../HealthComponent");

            GD.Print("Setting health~ " + MaxHealth);
            HealthComponent.UpdateMaxHealth(MaxHealth);

            HealthComponent.TriggerDefeat += OnHealthComponentTriggerDefeat;
        }

        public void OnHealthComponentTriggerDefeat()
        {
            GD.Print("And thus, I die.");
            GetParent().QueueFree();
        }
    }
}