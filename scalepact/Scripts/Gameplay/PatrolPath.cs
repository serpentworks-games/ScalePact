using Godot;

namespace Scalepact.Gameplay
{
    public partial class PatrolPath : Node3D
    {
        public Vector3 GetWaypoint(int i)
        {
            return GetChild<Node3D>(i).GlobalPosition;
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == GetChildCount()) return 0;

            return i + 1;
        }
    }
}