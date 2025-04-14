using Godot;

namespace Scalepact.Utilities
{
    public class UtilityFunctions
    {
        public static float ExpDecay(float a, float b, float decay, float delta)
        {
            return b + (a - b) * Mathf.Exp(-decay * delta);
        }
    }
}