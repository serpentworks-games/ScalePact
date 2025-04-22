using Godot;

namespace Scalepact.Utilities
{
    public class UtilityFunctions
    {
        public static float ExpDecay(float a, float b, float decay, float delta)
        {
            return b + (a - b) * Mathf.Exp(-decay * delta);
        }

        public static float Vector3Distance(Vector3 a, Vector3 b)
        {
            var vector = a - b;
            return vector.Length();
        }
    }
}