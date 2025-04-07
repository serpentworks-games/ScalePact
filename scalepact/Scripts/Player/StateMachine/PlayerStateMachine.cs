using Godot;
using Scalepact.DamageSystem;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerStateMachine : StateMachine
    {
        [ExportCategory("Movement Speeds")]
        [Export] public float MoveSpeed { get; private set; } = 5f;
        [Export] public float MeleeAttackMoveSpeed { get; private set; } = 2f;
        [Export] public float JumpMoveSpeed { get; private set; } = 5.5f;

        [ExportCategory("Jump and Glide Specifics")]
        [Export] public float JumpVelocity { get; private set; } = 4.5f;

        [ExportCategory("Animation Variables")]
        [Export] public float AnimInterpolationDecay { get; private set; } = 20f;
        [Export] public float AnimBlendWeight { get; private set; } = 5f;

        //Public variables
        public Vector3 AttackDirection { get; private set; } = Vector3.Zero;
        [Export] public CollisionShape3D Collider { get; private set; }

        //Refs
        public CharacterBody3D PlayerCharBody3D { get; private set; }
        public CameraController CameraController { get; private set; }
        public Node3D RigPivot { get; private set; }
        public Node3D Rig { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public Damager BiteAttackCollider { get; private set; }
        public HealthComponent HealthComponent { get; private set; }

        public override void _Ready()
        {
            PlayerCharBody3D = GetParent<CharacterBody3D>();
            CameraController = GetNode<CameraController>("../CamController");
            RigPivot = GetNode<Node3D>("../RigPivot");
            Rig = RigPivot.GetChild<Node3D>(0); //Rig itself
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");
            BiteAttackCollider = GetNode<Damager>("%BiteAttackCollider");
            HealthComponent = GetNode<HealthComponent>("../HealthComponent");

            HealthComponent.OnDeathTriggered += OnDeathTriggered;

            base._Ready();
        }

        #region State Changers
        public void ChangeToAttack()
        {
            ChangeState(PlayerStringRefs.PlayerAttackState);
            OneShotAnimationRequest(PlayerStringRefs.PlayerMeleeAttackRequest);
        }
        public void ChangeToJump()
        {
            ChangeState(PlayerStringRefs.PlayerJumpState);
        }
        public void ChangeToGroundMovement()
        {
            ChangeState(PlayerStringRefs.PlayerMoveState);
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
        public Vector3 ApplyGroundMovementVelocity(Vector3 direction, Vector3 velocity, float delta)
        {
            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * MoveSpeed;
                velocity.Z = direction.Z * MoveSpeed;

                LookTowardDirection(direction, delta);
            }
            else
            {
                velocity.X = Mathf.MoveToward(PlayerCharBody3D.Velocity.X, 0, MoveSpeed);
                velocity.Z = Mathf.MoveToward(PlayerCharBody3D.Velocity.Z, 0, MoveSpeed);
            }

            return velocity;
        }

        public Vector3 ApplyGravity(float delta, Vector3 velocity)
        {
            if (!PlayerCharBody3D.IsOnFloor())
                velocity += PlayerCharBody3D.GetGravity() * delta;
            return velocity;
        }

        public Vector3 ApplyJumpVelocity(Vector3 velocity)
        {
            if (PlayerCharBody3D.IsOnFloor())
                velocity.Y = JumpVelocity;
            return velocity;
        }

        public Vector3 ApplyJumpMovement(Vector3 velocity, Vector3 direction, float delta)
        {
            if (direction != Vector3.Zero)
            {
                velocity.X = direction.X * MoveSpeed;
                velocity.Z = direction.Z * MoveSpeed;

                LookTowardDirection(direction, delta);
            }
            else
            {
                velocity.X = Mathf.MoveToward(PlayerCharBody3D.Velocity.X, 0, MoveSpeed);
                velocity.Z = Mathf.MoveToward(PlayerCharBody3D.Velocity.Z, 0, MoveSpeed);
            }

            return velocity;
        }

        public Vector3 ApplyAttackMovement(Vector3 velocity, Vector3 direction, float delta)
        {
            AttackDirection = direction;
            if (AttackDirection.IsZeroApprox())
            {
                AttackDirection = Rig.GlobalBasis * new Vector3(0, 0, 1);
            }

            velocity.X = AttackDirection.X * MeleeAttackMoveSpeed;
            velocity.Z = AttackDirection.Z * MeleeAttackMoveSpeed;

            LookTowardDirection(AttackDirection, delta);

            return velocity;
        }

        public Vector3 GetMovementDirection()
        {
            Vector2 inputDir = Input.GetVector(
                "move_left", "move_right", "move_forward", "move_backward");

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
        public void OneShotAnimationRequest(string animName)
        {
            AnimationTree.Set(animName, (int)AnimationNodeOneShot.OneShotRequest.Fire);
        }

        public void AnimationTransitionRequest(string propertyPath, string state)
        {
            AnimationTree.Set(propertyPath, state);
        }

        public bool IsAttackActive()
        {
            return (bool)AnimationTree.Get("parameters/MeleeAttack/active");
        }

        public void UpdateMovementAnimTree(float velocity, float delta)
        {
            AnimationTree.Set(
                PlayerStringRefs.PlayerMoveBlendValue,
                Mathf.MoveToward(
                    (float)AnimationTree.Get(PlayerStringRefs.PlayerMoveBlendValue),
                    Mathf.Clamp(velocity, 0, 1),
                    delta * AnimBlendWeight
                ));
        }
        #endregion

        #region Combat Code
        public void ApplyMeleeAttack()
        {
            BiteAttackCollider.DealDamage();
            BiteAttackCollider.ClearExceptions();
        }
        #endregion

    }
}