using Godot;

namespace Scalepact.Utilities
{
    public class UtilityFunctions
    {
        #region Math Functions
        public static float ExpDecay(float a, float b, float decay, float delta)
        {
            return b + (a - b) * Mathf.Exp(-decay * delta);
        }

        public static float Vector3Distance(Vector3 a, Vector3 b)
        {
            var vector = a - b;
            return vector.Length();
        }

        public static float RepeatValue(float time, float length)
        {
            return Mathf.Clamp(time - Mathf.Floor(time / length) * length, 0, length);
        }

        public static float Clamp01(float value)
        {
            if (value < 0f) return 0f;
            else if (value > 1f) return 1f;
            else return value;
        }

        public static float ClampedLerp(float a, float b, float t)
        {
            return a + (b - a) * Clamp01(t);
        }

        public static float PingPong(float time, float length)
        {
            time = RepeatValue(time, length * 2f);
            return length - Mathf.Abs(time - length);
        }
        #endregion

        public static void QueueFreeWithDelay(Node node, float delay)
        {
            Timer timer = new Timer();
            timer.WaitTime = delay;
            timer.Timeout += node.QueueFree;
        }
    }
}