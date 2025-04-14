using Godot;
using Scalepact.Player;
using Scalepact.Utilities;
using System;

namespace Scalepact.Abilities
{
    public enum AbilityName
    {
        ability_melee_attack,
        ability_range_attack,
        ability_dash
    }

    public partial class AbilityBase : Node3D
    {
        [Export] AbilityName abilityName;
        [Export] protected float abilityDuration;
        [Export] protected float cooldownTimer;
        [Export] protected PlayerStateMachine player;

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

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!abilityTimer.IsStopped()) return;

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