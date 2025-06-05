using Godot;

namespace Scalepact.Abilities
{
    public partial class AbilityBase : Node3D
    {
        [Export] protected float abilityDuration;
        [Export] protected float abilityCooldownTime;

        protected bool isInAbility;

        protected bool isOnCoolDown;

        protected Timer abilityCoolDownTimer;
        protected Timer abilityDurationTimer;

        public override void _Ready()
        {
            abilityCoolDownTimer = GetNode<Timer>("AbilityCoolDownTimer");
            abilityDurationTimer = GetNode<Timer>("AbilityDurationTimer");

            if (abilityCoolDownTimer == null || abilityDurationTimer == null)
            {
                GD.PrintErr("Missing one or more timers for this ability!");
                return;
            }

            base._Ready();
        }

        public virtual void StartAbility()
        {
            abilityDurationTimer.Start(abilityDuration);
            isInAbility = true;
        }

        public virtual void ProcessAbility() { }

        public virtual void ResolveAbility()
        {
            isOnCoolDown = true;
            abilityCoolDownTimer.Start(abilityCooldownTime);

            isInAbility = false;
            abilityDurationTimer.Stop();
        }
        public virtual void ResetAbility()
        {
            abilityCoolDownTimer.Stop();
            isOnCoolDown = false;
        }

        public void TriggerAbility()
        {
            if (isInAbility || isOnCoolDown) return;
            StartAbility();
        }

        public bool IsAbilityCoolDownTimerStopped()
        {
            return abilityCoolDownTimer.IsStopped();
        }

        public bool IsAbilityDurationTimerStopped()
        {
            return abilityDurationTimer.IsStopped();
        }

        public bool IsAbilityAvailable()
        {
            return !isInAbility && !isOnCoolDown;
        }

        public void OnAbilityCoolDownTimerEnded()
        {
            ResetAbility();
        }

        public void OnAbilityDurationTimerEnded()
        {
            ResolveAbility();
        }

        public override void _PhysicsProcess(double delta)
        {
            if (isInAbility && !isOnCoolDown)
            {
                ProcessAbility();
            }
        }


    }
}