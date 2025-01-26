using Godot;
using Scalepact.Combat;
using Scalepact.StateMachines;

namespace Scalepact.Player
{
    public partial class PlayerStateMachine : StateMachine
    {
        [Export] public float MoveSpeed { get; private set; } = 5f;
        [Export] public float JumpVelocity { get; private set; } = 4.5f;
        [Export] public float AnimInterpolationDecay { get; private set; } = 20f;
        [Export] public float AnimBlendWeight { get; private set; } = 5f;
        [Export] public float MeleeAttackMoveSpeed { get; private set; } = 2f;

        //Public variables
        public Vector3 AttackDirection { get; private set; } = Vector3.Zero;

        //Refs
        public CharacterBody3D PlayerCharBody3D { get; private set; }
        public CameraController CameraController { get; private set; }
        public Node3D RigPivot { get; private set; }
        public Node3D Rig { get; private set; }
        public AnimationTree AnimationTree { get; private set; }
        public MeleeAttack MeleeAttackRayCast { get; private set; }

        public override void _Ready()
        {
            PlayerCharBody3D = GetParent<CharacterBody3D>();
            CameraController = GetNode<CameraController>("../CamController");
            RigPivot = GetNode<Node3D>("../RigPivot");
            Rig = RigPivot.GetChild<Node3D>(0); //Rig itself
            AnimationTree = GetNode<AnimationTree>("../AnimationTree");
            MeleeAttackRayCast = GetNode<MeleeAttack>("%MeleeAttackCast");

            base._Ready();
        }

        #region State Changers
        public void ChangeToAttack()
        {
            ChangeState(PlayerStringRefs.PlayerAttackState);
            AnimationRequest(PlayerStringRefs.PlayerMeleeAttackRequest);
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
            //TODO: make this transition into the jump state
            if (Input.IsActionJustPressed("jump") && PlayerCharBody3D.IsOnFloor())
                velocity.Y = JumpVelocity;
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
        }
        #endregion

        #region Animation Functions
        public void AnimationRequest(string animName)
        {
            AnimationTree.Set(animName, (int)AnimationNodeOneShot.OneShotRequest.Fire);
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
            MeleeAttackRayCast.DealDamage();
            MeleeAttackRayCast.ClearExceptions();
        }
        #endregion

    }
}