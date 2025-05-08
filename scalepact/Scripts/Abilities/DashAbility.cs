using Godot;
using Scalepact.Player;

namespace Scalepact.Abilities.PlayerAbilities
{
    public partial class DashAbility : AbilityBase
    {
        [Export] public float DashSpeedBoost { get; private set; } = 5f;

        public Vector3 MoveDir { get; set; } = Vector3.Zero;

        public override void StartAbility()
        {
            if (stateMachine is PlayerStateMachine)
            {
                PlayerStateMachine player = (PlayerStateMachine)stateMachine;
                MoveDir = player.GetMovementDirection();

                if (!MoveDir.IsZeroApprox())
                {
                    player.ChangeToDash();

                    abilityTimer.Start(cooldownTimer);
                    abilityTimeRemaining = abilityDuration;
                }
            }
            base.StartAbility();

        }

        public override void ProcessAbility()
        {
            base.ProcessAbility();
        }

        public override void ResolveAbility()
        {
            MoveDir = Vector3.Zero;
        }
    }
}