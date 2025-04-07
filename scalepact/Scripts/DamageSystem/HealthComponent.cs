using Godot;

namespace Scalepact.DamageSystem
{
    public partial class HealthComponent : Node
    {
        [Export] public float MaxHealth { get; private set; }
        [Export] public float InvulnerabilityTime { get; private set; }

        [Signal] public delegate void OnDamageReceivedEventHandler();
        [Signal] public delegate void OnDeathTriggeredEventHandler();
        [Signal] public delegate void OnBecomeVulnerableEventHandler();

        public float CurrentHealth { get; private set; }
        public bool IsInvulnerable { get; private set; }

        float timeSinceLastHit = 0.0f;

        public override void _Ready()
        {
            ResetDamage();
        }

        public override void _Process(double delta)
        {
            if (IsInvulnerable)
            {
                timeSinceLastHit += (float)delta;

                if (timeSinceLastHit > InvulnerabilityTime)
                {
                    timeSinceLastHit = 0f;
                    IsInvulnerable = false;
                    EmitSignal(SignalName.OnBecomeVulnerable);
                }
            }
        }

        public void ResetDamage()
        {
            CurrentHealth = MaxHealth;
            IsInvulnerable = false;
            timeSinceLastHit = 0f;
        }

        public void TakeDamage(float damageAmount)
        {
            if (CurrentHealth <= 0) return; //don't apply damage if already dead!

            if (IsInvulnerable)
            {
                //TODO: Signal for when you're invulnerable
                GD.Print("Currently invulnerable!");
                return;
            }

            IsInvulnerable = true;
            CurrentHealth -= damageAmount;

            GD.Print(this.Name + "'s current health is: " + CurrentHealth);

            if (CurrentHealth <= 0) EmitSignal(SignalName.OnDeathTriggered);
            else EmitSignal(SignalName.OnDamageReceived);
        }

        public void InstaKill()
        {
            CurrentHealth = 0;
            EmitSignal(SignalName.OnDeathTriggered);
        }
    }
}