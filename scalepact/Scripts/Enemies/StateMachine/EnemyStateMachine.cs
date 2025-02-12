using Godot;
using Scalepact.Combat;
using Scalepact.StateMachines;

namespace Scalepact.Enemies
{
    public partial class EnemyStateMachine : StateMachine
    {
        [Export] public float MaxHealth { get; private set; } = 5f;

        public HealthComponent HealthComponent { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public CollisionShape3D CollisionShape { get; private set; }
        public ShapeCast3D PlayerDetector { get; private set; }

        string animDeathTransition = "parameters/DeathTransition/transition_request";
        string animAttack1Active = "parameters/AttackTrigger/active";
        string animAttack1Trigger = "parameters/AttackTrigger/request";

        public override void _Ready()
        {
            HealthComponent = GetNode<HealthComponent>("../HealthComponent");
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");
            CollisionShape = GetNode<CollisionShape3D>("../CollisionShape3D");
            PlayerDetector = GetNode<ShapeCast3D>("../TargetDetector");

            GD.Print("Setting health~ " + MaxHealth);
            HealthComponent.UpdateMaxHealth(MaxHealth);

            HealthComponent.TriggerDefeat += OnHealthComponentTriggerDefeat;

            base._Ready();
        }

        #region State Changers
        public void ChangeToAttack()
        {
            ChangeState("AttackState");
            OneShotAnimationRequest(animAttack1Trigger);
        }

        public void OnHealthComponentTriggerDefeat()
        {
            GD.Print("And thus, I die.");
            //ChangeState("DeathState");
            AnimationTransitionRequest(animDeathTransition, "Death");
            CollisionShape.Disabled = true;
            SetPhysicsProcess(false);
        }
        #endregion

        #region Combat Handlers
        public bool IsInAttackRange()
        {
            for (int i = 0; i < PlayerDetector.GetCollisionCount(); i++)
            {
                var col = PlayerDetector.GetCollider(i);
                GD.Print("Can see: " + col);
                return true;
            }
            return false;
        }
        #endregion

        #region Animation Handlers
        public void OneShotAnimationRequest(string animName)
        {
            AnimationTree.Set(animName, (int)AnimationNodeOneShot.OneShotRequest.Fire);
        }

        public void AnimationTransitionRequest(string propertyPath, string state)
        {
            AnimationTree.Set(propertyPath, state);
        }

        public bool IsOneShotAnimationActive(string animPath)
        {
            return (bool)AnimationTree.Get(animPath);
        }
        #endregion
    }
}