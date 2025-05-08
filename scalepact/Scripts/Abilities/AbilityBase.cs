using Godot;
using Scalepact.StateMachines;

namespace Scalepact.Abilities
{
    public enum AbilityName
    {
        NONE,
        ability_melee_attack,
        ability_range_attack,
        ability_dash
    }

    public partial class AbilityBase : Node3D
    {
        [Export] AbilityName abilityName;
        [Export] protected float abilityDuration;
        [Export] protected float cooldownTimer;
        [Export] protected StateMachine stateMachine;

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
        public virtual void ResolveAbility() { }

        public void SetIsInAbility(bool newState)
        {
            IsInAbility = newState;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!abilityTimer.IsStopped()) return;

            if (abilityName == AbilityName.NONE) return;

            if (@event.IsActionPressed(abilityName.ToString()))
            {
                StartAbility();
                IsInAbility = true;
            }
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