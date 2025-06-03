using Godot;

namespace Scalepact.Abilities.PlayerAbilities
{
    public partial class DashAbility : AbilityBase
    {
        [Export] public float DashSpeedBoost { get; private set; } = 5f;

        public override void StartAbility()
        {
            abilityTimer.Start(abilityCooldownTimer);
            abilityTimeRemaining = abilityDuration;

            GD.Print("Time remaining set to: " + abilityDuration);
        }

        public override void ResolveAbility()
        {
            base.ResolveAbility();
        }
    }
}