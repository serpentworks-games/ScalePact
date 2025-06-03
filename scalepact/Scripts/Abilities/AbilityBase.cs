using Godot;

namespace Scalepact.Abilities
{
    public partial class AbilityBase : Node3D
    {
        [Export] protected float abilityDuration;
        [Export] protected float abilityCooldownTimer;

        public bool IsInAbility { get; private set; }

        protected float abilityTimeRemaining = Mathf.Inf;

        protected Timer abilityTimer;

        public override void _Ready()
        {
            abilityTimer = GetNode<Timer>("Timer");
            base._Ready();
        }

        public virtual void StartAbility() { }
        public virtual void ProcessAbility() { }
        public virtual void ResolveAbility() { abilityTimer.Stop(); }

        public void TriggerAbility()
        {
            if (!abilityTimer.IsStopped() || IsInAbility == true) return;
            StartAbility();
            IsInAbility = true;
        }

        public bool IsAbilityTimerStopped()
        {
            return abilityTimer.IsStopped();
        }

        public override void _PhysicsProcess(double delta)
        {
            if (IsInAbility == false || abilityTimer.IsStopped()) return;

            abilityTimeRemaining -= (float)delta;

            if (abilityTimeRemaining <= 0)
            {
                ResolveAbility();
                GD.Print("Ability Over!");
                IsInAbility = false;
            }
            else
            {
                ProcessAbility();
                GD.Print("Is doing ability!");
            }
        }


    }
}