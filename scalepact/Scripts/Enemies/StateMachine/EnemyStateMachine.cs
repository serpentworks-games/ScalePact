using Godot;
using Scalepact.DamageSystem;
using Scalepact.StateMachines;
using Scalepact.Utilities;

namespace Scalepact.Enemies
{
    public partial class EnemyStateMachine : StateMachine
    {
        [Export] public float BaseMoveSpeed { get; private set; } = 2.5f;
        [Export] public float ChaseMoveSpeed { get; private set; } = 3f;

        public HealthComponent HealthComponent { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public CollisionShape3D CollisionShape { get; private set; }
        public ShapeCast3D AttackRange { get; private set; }
        public ShapeCast3D ChaseRange { get; private set; }
        public Damager AttackCollider { get; private set; }
        public NavigationAgent3D Agent3D { get; private set; }
        public CharacterBody3D Body3D { get; private set; }
        public Node3D Rig { get; private set; }

        public Node3D Player { get; private set; }

        private const int kMoveThreshold = 1;
        float blendTarget;

        string animDeathTransition = "parameters/DeathTransition/transition_request";
        string animAttack1Active = "parameters/AttackTrigger/active";
        string animAttack1Trigger = "parameters/AttackTrigger/request";
        string animMoveBlendValue = "parameters/MoveSpace/blend_position";


        public override void _Ready()
        {
            HealthComponent = GetNode<HealthComponent>("../HealthComponent");
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");
            CollisionShape = GetNode<CollisionShape3D>("../CollisionShape3D");
            AttackRange = GetNode<ShapeCast3D>("../RigPivot/AttackRange");
            ChaseRange = GetNode<ShapeCast3D>("../RigPivot/ChaseRange");
            AttackCollider = GetNode<Damager>("%MeleeAttackCollider");
            Agent3D = GetNode<NavigationAgent3D>("../NavigationAgent3D");
            Player = GetTree().GetFirstNodeInGroup("Player") as Node3D;
            Body3D = GetParent<CharacterBody3D>();
            Rig = GetNode<Node3D>("../RigPivot");

            HealthComponent.OnDeathTriggered += OnDeathTriggered;
            Agent3D.VelocityComputed += OnNavAgentVelocityComputed;

            base._Ready();
        }

        public override void _PhysicsProcess(double delta)
        {
            base._PhysicsProcess(delta);
            UpdateMovementBlendValue(animMoveBlendValue, blendTarget, 5, (float)delta);
        }

        #region State Changers
        public bool IsInRange(ShapeCast3D rangeChecker)
        {
            for (int i = 0; i < rangeChecker.GetCollisionCount(); i++)
            {
                var col = rangeChecker.GetCollider(i);
                if (col is PlayerEntityAccessor)
                    return true;
            }
            return false;
        }
        public void ChangeToIdleState()
        {
            ChangeState("IdleState");
        }
        public void ChangeToAttackState()
        {
            ChangeState("AttackState");
            OneShotAnimationRequest(animAttack1Trigger);
        }
        public void ChangeToChaseState()
        {
            ChangeState("ChaseState");
        }

        public void OnDeathTriggered()
        {
            GD.Print("And thus, I die.");
            ChangeState("DeathState");
            AnimationTransitionRequest(animDeathTransition, "Death");
            CollisionShape.Disabled = true;
            SetPhysicsProcess(false);
        }
        #endregion

        #region AI Navigation Handlers

        public void OrientRig(Vector3 targetPos)
        {
            targetPos.Y = Rig.GlobalPosition.Y;

            if (Rig.GlobalPosition.IsEqualApprox(targetPos)) return;

            Rig.LookAt(targetPos, Vector3.Up, true);
        }

        public Vector3 GetLocalNavigationDirection()
        {
            Vector3 dest = Agent3D.GetNextPathPosition();
            var localPos = dest - GetParent<CharacterBody3D>().GlobalPosition;
            return localPos.Normalized();
        }

        public void OnNavAgentVelocityComputed(Vector3 safeVelocity)
        {
            GD.Print("SafeVelocity is: " + safeVelocity.Length());
            if (safeVelocity.Length() > kMoveThreshold)
                blendTarget = 1;
            else
                blendTarget = 0;

            Body3D.Velocity = safeVelocity;
            Body3D.MoveAndSlide();
        }
        public void StopAgent()
        {
            Agent3D.Velocity = Vector3.Zero;
        }
        #endregion

        #region Combat Handlers

        #endregion

        #region Animation Handlers

        #endregion

        #region Animation Events
        public void TryApplyDamage()
        {
            GD.Print("Attack!");
            AttackCollider.DealDamage();
            AttackCollider.ClearExceptions();
        }
        #endregion
    }
}