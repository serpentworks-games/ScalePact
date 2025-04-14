using Godot;

namespace Scalepact.Abilities
{
    public partial class BiteAttackAbility : AbilityBase
    {
        public override void StartAbility()
        {
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
            base.StartAbility();
        }
    }
}