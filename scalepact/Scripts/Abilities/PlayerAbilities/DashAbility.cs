using Godot;

namespace Scalepact.Abilities.PlayerAbilities
{
    public partial class DashAbility : NonCombatAbility
    {
        [Export] public float DashSpeedBoost { get; private set; } = 5f;

        public override void StartAbility()
        {
            base.StartAbility();
        }
    }
}