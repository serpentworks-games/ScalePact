using Godot;
using Scalepact.Abilities;
using Scalepact.Abilities.PlayerAbilities;
using Scalepact.Core;
using Scalepact.DamageSystem;
using Scalepact.Utilities;

namespace Scalepact.StateMachines.Player
{
    public partial class PlayerStateMachine : StateMachine
    {
        [ExportCategory("Movement Speeds")]
        [Export] public float MoveSpeed { get; private set; } = 5f;
        [Export] public float MeleeAttackMoveSpeed { get; private set; } = 2f;
        [Export] public float PhysicsFrameDecay { get; private set; } = 8.0f;

        [ExportCategory("Jump and Glide Specifics")]
        [Export] public float JumpMoveSpeed { get; private set; } = 5.5f;
        [Export] public float JumpVelocity { get; private set; } = 4.5f;
        [Export] public float JumpFalloff { get; private set; } = 2f;

        [ExportCategory("Animation Variables")]
        [Export] public float AnimInterpolationDecay { get; private set; } = 20f;
        [Export] public float AnimBlendWeight { get; private set; } = 5f;

        [ExportCategory("Abilities")]
        [Export] public CombatAbility MeleeBiteAbility { get; private set; }
        [Export] public DashAbility DashAbility { get; private set; }
        [Export] public ProjectileCombatAbility FireBallAbility { get; private set; }

        //Public variables
        public Vector3 AttackDirection { get; private set; } = Vector3.Zero;
        [ExportCategory("Other Refs")]
        [Export] public CollisionShape3D Collider { get; private set; }

        //Refs
        public CharacterBody3D PlayerCharBody3D { get; private set; }
        public CameraController CameraController { get; private set; }
        public Node3D RigPivot { get; private set; }
        public Node3D Rig { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public HealthComponent HealthComponent { get; private set; }

        Vector2 inputDir;


        public override void _Ready()
        {
            PlayerCharBody3D = GetParent<CharacterBody3D>();
            CameraController = GetNode<CameraController>("../CamController");
            RigPivot = GetNode<Node3D>("../RigPivot");
            Rig = RigPivot.GetChild<Node3D>(0); //Rig itself
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");
            HealthComponent = GetNode<HealthComponent>("../HealthComponent");

            HealthComponent.OnDeathTriggered += OnDeathTriggered;

            base._Ready();
        }

        public static bool EnableControl { get; set; } = true;

        #region Input
        public override void _UnhandledInput(InputEvent @event)
        {
            if (!EnableControl) return;

            inputDir = Input.GetVector(
                "move_left", "move_right", "move_forward", "move_backward");

            if (@event.IsActionPressed("jump")) ChangeToJump();

            if (!PlayerCharBody3D.IsOnFloor() && @event.IsActionPressed("jump"))
            {
                //switch to gliding
                //return;
                GD.Print("Activating gliding!");
            }

            if (!GetMovementDirection().IsZeroApprox())
            {
                if (@event.IsActionPressed("ability_dash") && DashAbility.IsAbilityAvailable())
                {
                    DashAbility.TriggerAbility();
                    ChangeToDash();
                    GD.Print("Triggering Dash!");
                }
            }

            if (PlayerCharBody3D.IsOnFloor())
            {
                if (@event.IsActionPressed("ability_melee_attack") && MeleeBiteAbility.IsAbilityAvailable())
                {
                    MeleeBiteAbility.TriggerAbility();
                    ChangeToMeleeAttack();
                }

                if (@event.IsActionPressed("ability_range_attack") && FireBallAbility.IsAbilityAvailable())
                {
                    FireBallAbility.TriggerAbility();
                    ChangeToRangeAttack();
                }
            }

            base._UnhandledInput(@event);
        }
        #endregion

        #region State Changers
        public void ChangeToMeleeAttack()
        {
            ChangeState(PlayerStringRefs.PlayerAttackState);
            OneShotAnimationRequest(PlayerStringRefs.PlayerMeleeAttackRequest);
        }
        public void ChangeToRangeAttack()
        {
            ChangeState("RangeAttackState");
        }
        public void ChangeToJump()
        {
            ChangeState(PlayerStringRefs.PlayerJumpState);
        }
        public void ChangeToGroundMovement()
        {
            ChangeState(PlayerStringRefs.PlayerMoveState);
        }
        public void ChangeToDash()
        {
            ChangeState(PlayerStringRefs.PlayerDashState);
        }
        public void OnDeathTriggered()
        {
            GD.Print("Triggering death!");
            ChangeState(PlayerStringRefs.PlayerDeathState);
            AnimationTransitionRequest(PlayerStringRefs.PlayerDeathRequest, "Dead");
            SetPhysicsProcess(false);
        }
        #endregion

        #region Movement Code
        public Vector3 ResolveMovementPhysics(Vector3 direction, Vector3 velocity, float speedValue, float delta)
        {
            velocity = ApplyVelocityEasing(direction, velocity, speedValue, delta);

            if (direction != Vector3.Zero)
            {
                LookTowardDirection(direction, delta);
            }
            return velocity;
        }

        public Vector3 ApplyVelocityEasing(Vector3 direction, Vector3 velocity, float speedValue, float delta)
        {

            velocity.X = UtilityFunctions.ExpDecay(
                velocity.X,
                direction.X * speedValue,
                PhysicsFrameDecay, delta
            );
            velocity.Z = UtilityFunctions.ExpDecay(
                velocity.Z,
                direction.Z * speedValue,
                PhysicsFrameDecay, delta
            );
            return velocity;
        }

        public Vector3 ApplyGravity(float delta, Vector3 velocity)
        {
            if (!PlayerCharBody3D.IsOnFloor())
                velocity += (JumpFalloff - 1) * PlayerCharBody3D.GetGravity() * delta;
            return velocity;
        }

        public Vector3 ApplyJumpVelocity(Vector3 velocity)
        {
            if (PlayerCharBody3D.IsOnFloor())
                velocity.Y = JumpVelocity;
            return velocity;
        }

        public Vector3 ApplyAttackMovement(Vector3 direction, Vector3 velocity, float speedValue, float delta)
        {
            velocity = ApplyVelocityEasing(direction, velocity, speedValue, delta);

            AttackDirection = direction;
            if (AttackDirection.IsZeroApprox())
            {
                AttackDirection = Rig.GlobalBasis * new Vector3(0, 0, 1);
            }

            LookTowardDirection(AttackDirection, delta);

            return velocity;
        }

        public Vector3 GetMovementDirection()
        {
            Vector3 inputVector = new Vector3(inputDir.X, 0, inputDir.Y).Normalized();

            return CameraController.HorizontalPivot.GlobalTransform.Basis * inputVector;
        }

        public void LookTowardDirection(Vector3 direction, float delta)
        {
            Transform3D targetTransform = RigPivot.GlobalTransform.LookingAt(
                RigPivot.GlobalPosition + direction, Vector3.Up, true);

            RigPivot.GlobalTransform = RigPivot.GlobalTransform.InterpolateWith(
                targetTransform, 1.0f - Mathf.Exp(-AnimInterpolationDecay * delta)
            );

            Collider.GlobalRotationDegrees = new Vector3(90f, RigPivot.GlobalRotationDegrees.Y, 0.0f);
        }
        #endregion

        #region Animation Functions

        #endregion

        #region Combat Code

        #endregion

    }
}