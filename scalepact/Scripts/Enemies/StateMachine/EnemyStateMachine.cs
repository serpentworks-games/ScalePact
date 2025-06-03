using Godot;
using Scalepact.Abilities.EnemyAbilities;
using Scalepact.DamageSystem;
using Scalepact.Gameplay;
using Scalepact.StateMachines;
using Scalepact.Utilities;

namespace Scalepact.Enemies
{
    public partial class EnemyStateMachine : StateMachine
    {
        //Exported Variables
        [Export] public float BaseMoveSpeed { get; private set; } = 2.5f;
        [Export] public float ChaseMoveSpeed { get; private set; } = 3f;

        [Export] public PatrolPath PatrolPath { get; private set; }
        [Export] public WanderArea WanderArea { get; private set; }
        [Export] public float PointDwellTime { get; private set; } = 1f;
        [Export] public float SuspicionTime { get; private set; } = 3f;

        [Export] public EnemyMeleeAttackAbility MeleeAttackAbility { get; private set; }

        //Public Variables
        public Node3D Player { get; private set; }
        public Vector3 OriginalPosition { get; private set; }
        public Vector3 LastWaypointPos { get; set; }

        //Node Refs
        public HealthComponent HealthComponent { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public CollisionShape3D CollisionShape { get; private set; }
        public ShapeCast3D AttackRange { get; private set; }
        public ShapeCast3D ChaseRange { get; private set; }
        public Damager AttackCollider { get; private set; }
        public NavigationAgent3D Agent3D { get; private set; }
        public CharacterBody3D Body3D { get; private set; }
        public Node3D Rig { get; private set; }

        //Private Variables
        int currentWaypointIndex;

        //Animation private variables
        private const int kMoveThreshold = 1;
        float blendTarget;

        //Animator Strings
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

            HealthComponent.OnDeathTriggered += ChangeToDeathState;
            Agent3D.VelocityComputed += OnNavAgentVelocityComputed;

            OriginalPosition = Body3D.GlobalPosition;

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
            MeleeAttackAbility.TriggerAbility();
        }
        public void ChangeToChaseState()
        {
            ChangeState("ChaseState");
        }
        public void ChangeToPatrolState()
        {
            ChangeState("PatrolState");
        }
        public void ChangeToWanderState()
        {
            ChangeState("WanderState");
        }
        public void ChangeToSuspicionState()
        {
            ChangeState("SuspicionState");
        }

        public void ChangeToDeathState()
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

        #region Patrol and Wander Handlers
        //Shared
        public bool IsAtTargetPoint(Vector3 targetPoint)
        {
            Vector3 adjustedGlobal = Vector3.Zero;
            if (PatrolPath != null)
            {
                adjustedGlobal = new Vector3(Body3D.GlobalPosition.X, PatrolPath.GlobalPosition.Y, Body3D.GlobalPosition.Z);
            }
            else if (WanderArea != null)
            {
                adjustedGlobal = new Vector3(Body3D.GlobalPosition.X, WanderArea.GlobalPosition.Y, Body3D.GlobalPosition.Z);
            }
            else
            {
                adjustedGlobal = Body3D.GlobalPosition;
            }

            return UtilityFunctions.Vector3Distance(adjustedGlobal, targetPoint) < Agent3D.TargetDesiredDistance;
        }

        public Vector3 GetGuardOrLastWaypointPosition()
        {
            if (PatrolPath == null && WanderArea == null)
                return OriginalPosition;
            else
                return LastWaypointPos;
        }

        //Patrol Paths
        public Vector3 GetCurrentWaypoint()
        {
            return PatrolPath.GetWaypoint(currentWaypointIndex);
        }

        public void GetNextWaypoint()
        {
            currentWaypointIndex = PatrolPath.GetNextIndex(currentWaypointIndex);
        }

        //Wander Areas
        public Vector3 GetCurrentLocationInArea()
        {
            return WanderArea.GeneratedPoint;
        }

        public void GetNewPointInArea()
        {
            WanderArea.GenerateRandomPoint();
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
            //AttackCollider.DealDamage();
            //AttackCollider.ClearExceptions();
        }
        #endregion
    }
}