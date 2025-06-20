namespace Scalepact.Core
{
	public static class PlayerStringRefs
	{
		//Why: Because I am terrible at spelling

		//States
		public static string PlayerMoveState { get => "MoveState"; }
		public static string PlayerAttackState { get => "AttackState"; }
		public static string PlayerJumpState { get => "JumpState"; }
		public static string PlayerDeathState { get => "DeathState"; }
		public static string PlayerDashState { get => "DashState"; }

		//Animations
		public static string PlayerAnimPlayback { get => "parameters/playback"; }
		public static string PlayerMoveBlendValue { get => "parameters/MoveBlend/blend_position"; }
		public static string PlayerMeleeAttackRequest { get => "parameters/MeleeAttack/request"; }
		public static string PlayerMeleeAttackIsActive { get => "parameters/MeleeAttack/active"; }
		public static string PlayerDeathRequest { get => "parameters/Death/transition_request"; }
		public static string PlayerDeathCurrentState { get => "parameters/Death/current_state"; }

	}
}