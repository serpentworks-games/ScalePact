using Godot;
using Scalepact.Player;

namespace Scalepact.Abilities.PlayerAbilities
{
    public partial class BiteAttackAbility : AbilityBase
    {
        public override void StartAbility()
        {
            if (stateMachine is PlayerStateMachine)
            {
                PlayerStateMachine player = (PlayerStateMachine)stateMachine;
                CharacterBody3D charBody3D = player.PlayerCharBody3D;

                if (charBody3D != null)
                {
                    if (charBody3D.IsOnFloor())
                    {
                        player.ChangeToAttack();

                        abilityTimer.Start(cooldownTimer);
                        abilityTimeRemaining = abilityDuration;
                    }
                }
            }
            base.StartAbility();
        }
    }
}