using Godot;
using System;

namespace Scalepact.Combat
{
    public partial class HealthComponent : Node
    {
        [Signal] public delegate void HealthHasChangedEventHandler();
        [Signal] public delegate void TriggerDefeatEventHandler();

        public float MaxHealth { get; private set; }
        public float CurrentHealth
        {
            get => currentHealth;
            set
            {
                Mathf.Max(currentHealth, 0.0);
                if (currentHealth == 0.0) EmitSignal(SignalName.TriggerDefeat);
                EmitSignal(SignalName.HealthHasChanged);
            }
        }
        float currentHealth;

        public void UpdateMaxHealth(float maxHP)
        {
            MaxHealth = maxHP;
            currentHealth = maxHP;
        }

        public void TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;
        }
    }
}