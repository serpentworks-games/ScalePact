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
        [ExportCategory("Movement Variables")]
        [Export] public float MoveSpeed { get; private set; } = 5f;
        [Export] public float MeleeAttackMoveSpeed { get; private set; } = 2f;
        [Export] public float PhysicsFrameDecay { get; private set; } = 8.0f;
        [Export] public float GravityModifier { get; private set; } = 5f;

        [ExportCategory("Jump Variables")]
        [Export] public float JumpMoveSpeed { get; private set; } = 5.5f;
        [Export] public float JumpVelocity { get; private set; } = 4.5f;
        [Export] public float JumpHeightTime { get; private set; } = 0.2f;
        [Export] public float JumpBufferTime { get; private set; } = 0.3f;
        [Export] public float JumpCoyoteTime { get; private set; } = 0.2f;

        [ExportCategory("Glide Variables")]
        [Export] public float GlideForce { get; private set; } = 10f;

        [ExportCategory("Animation Variables")]
        [Export] public float AnimInterpolationDecay { get; private set; } = 20f;
        [Export] public float AnimBlendWeight { get; private set; } = 5f;

        [ExportCategory("Abilities")]
        [Export] public CombatAbility MeleeBiteAbility { get; private set; }
        [Export] public DashAbility DashAbility { get; private set; }
        [Export] public ProjectileCombatAbility FireBallAbility { get; private set; }

        [ExportCategory("Other Refs")]
        [Export] public CollisionShape3D Collider { get; private set; }

        //Public variables
        public Vector3 AttackDirection { get; set; } = Vector3.Zero;
        public bool WasOnFloor { get; set; } = false;

        //Node Refs
        public CharacterBody3D PlayerCharBody3D { get; private set; }
        public CameraController CameraController { get; private set; }
        public Node3D RigPivot { get; private set; }
        public Node3D Rig { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public HealthComponent HealthComponent { get; private set; }

        //Input states
        Vector2 inputDir;
        bool isJumpJustPressed;
        bool isJumpPressed;
        bool isDashPressed;
        bool isMeleeButtonPressed;
        bool isRangeButtonPressed;

        //Jumping
        bool jumpBuffer, coyoteTime, doubleJump = false;
        Timer jumpHeightTimer = new();
        Timer jumpBufferTimer = new();
        Timer coyoteTimeTimer = new();

        public static bool EnableControl { get; set; } = true;

        public override void _Ready()
        {
            PlayerCharBody3D = GetParent<CharacterBody3D>();
            CameraController = GetNode<CameraController>("../CamController");
            RigPivot = GetNode<Node3D>("../RigPivot");
            Rig = RigPivot.GetChild<Node3D>(0); //Rig itself
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");
            HealthComponent = GetNode<HealthComponent>("../HealthComponent");

            HealthComponent.OnDeathTriggered += OnDeathTriggered;

            SetupTimers();

            base._Ready();
        }

        public void SetupTimers()
        {
            jumpHeightTimer.Name = "JumpHeightTimer";
            jumpHeightTimer.WaitTime = JumpHeightTime;
            jumpHeightTimer.OneShot = true;
            jumpHeightTimer.Timeout += VariableJumpHeight;
            AddChild(jumpHeightTimer);

            jumpBufferTimer.Name = "JumpBufferTimer";
            jumpBufferTimer.WaitTime = JumpBufferTime;
            jumpBufferTimer.OneShot = true;
            jumpBufferTimer.Timeout += StopJumpBuffer;
            AddChild(jumpBufferTimer);

            coyoteTimeTimer.Name = "CoyoteTimeTimer";
            coyoteTimeTimer.WaitTime = JumpCoyoteTime;
            coyoteTimeTimer.OneShot = true;
            coyoteTimeTimer.Timeout += StopCoyoteTime;
            AddChild(coyoteTimeTimer);
        }

        #region Input
        public void GetInput()
        {
            inputDir = Input.GetVector("move_left", "move_right", "move_forward", "move_backward");
            isJumpJustPressed = Input.IsActionJustPressed("jump");
            isJumpPressed = Input.IsActionPressed("jump");
            isDashPressed = Input.IsActionPressed("ability_dash");
            isMeleeButtonPressed = Input.IsActionPressed("ability_melee_attack");
            isRangeButtonPressed = Input.IsActionPressed("ability_range_attack");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (!EnableControl) return;

            GetInput();

            if (isJumpJustPressed)
            {
                ChangeToJump();
            }

            //Handle gliding

            if (!GetMovementDirection().IsZeroApprox())
            {
                if (isDashPressed && DashAbility.IsAbilityAvailable())
                {
                    DashAbility.TriggerAbility();
                    ChangeToDash();
                }
            }

            if (PlayerCharBody3D.IsOnFloor())
            {
                if (isMeleeButtonPressed && MeleeBiteAbility.IsAbilityAvailable())
                {
                    MeleeBiteAbility.TriggerAbility();
                    ChangeToMeleeAttack();
                }

                if (isRangeButtonPressed && FireBallAbility.IsAbilityAvailable())
                {
                    FireBallAbility.TriggerAbility();
                    ChangeToRangeAttack();
                }
            }

            WasOnFloor = PlayerCharBody3D.IsOnFloor();

            HandleCoyoteTime();
            HandleJumpBuffer(PlayerCharBody3D.Velocity);
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
            jumpHeightTimer.Start();
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

        #region Jump Code
        public Vector3 AddGravity(Vector3 velocity, float delta)
        {
            if (!PlayerCharBody3D.IsOnFloor() || !coyoteTime)
            {
                velocity += PlayerCharBody3D.GetGravity() * delta * GravityModifier;
            }

            return velocity;
        }

        public Vector3 HandleJump(Vector3 velocity)
        {
            if (PlayerCharBody3D.IsOnFloor() || coyoteTime)
            {
                velocity = new()
                {
                    Y = JumpVelocity
                };

                if (coyoteTime)
                {
                    coyoteTime = false;
                }
            }
            else if (isJumpJustPressed && doubleJump)
            {
                doubleJump = false;
                velocity = new()
                {
                    Y = JumpVelocity
                };
            }
            else if (!jumpBuffer)
            {
                jumpBuffer = true;
                jumpBufferTimer.Start();
            }

            return velocity;
        }
        public void HandleCoyoteTime()
        {
            if (WasOnFloor && !PlayerCharBody3D.IsOnFloor() && PlayerCharBody3D.Velocity.Y >= 0)
            {
                coyoteTime = false;
                coyoteTimeTimer.Start();
            }
        }

        public void HandleJumpBuffer(Vector3 velocity)
        {
            if (!WasOnFloor && PlayerCharBody3D.IsOnFloor())
            {
                doubleJump = true;
                if (jumpBuffer)
                {
                    jumpBuffer = false;
                    HandleJump(velocity);
                }
            }
        }
        private void StopCoyoteTime()
        {
            coyoteTime = false;
        }

        private void StopJumpBuffer()
        {
            jumpBuffer = false;
        }

        private void VariableJumpHeight()
        {
            if (!isJumpPressed)
            {
                if (PlayerCharBody3D.Velocity.Y < 0)
                {
                    PlayerCharBody3D.Velocity = new()
                    {
                        Y = 0
                    };
                }
            }
        }

        #endregion

        #region Movement Code
        public Vector3 ApplyMovement(Vector3 direction, Vector3 velocity, float speedValue, float delta)
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
    }
}