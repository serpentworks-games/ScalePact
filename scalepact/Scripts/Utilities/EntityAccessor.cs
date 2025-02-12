using Godot;
using Scalepact.Combat;
using Scalepact.StateMachines;

namespace Scalepact.Utilities
{
	/// <summary>
	/// This class should ONLY be extended from unless there are
	/// shared nodes across entities
	/// </summary>
	public partial class EntityAccessor : CharacterBody3D
	{
		[Export] public StateMachine StateMachine { get; private set; }
		[Export] public HealthComponent HealthComponent { get; private set; }
	}
}