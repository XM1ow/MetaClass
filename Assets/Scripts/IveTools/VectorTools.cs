using UnityEngine;
using static UnityEngine.Mathf;

namespace Ive
{
    public static class VectorTools
    {
        public static Vector3 ToVector3XZ(this Vector2 originVector)
        {
            return new Vector3(originVector.x, 0f, originVector.y);
        }
        
        public static Vector2 ToVector2XZ(this Vector3 originVector)
        {
            return new Vector2(originVector.x, originVector.z);
        }

        public static Vector3 Offset(this Vector3 sender, Vector3 offset)
        {
            return new Vector3(sender.x + offset.x, sender.y + offset.y, sender.z + offset.z);
        }

        public static Vector3 OffsetX(this Vector3 sender, float offsetX)
        {
            return new Vector3(sender.x + offsetX, sender.y, sender.z);
        }
        
        public static Vector3 OffsetY(this Vector3 sender, float offsetY)
        {
            return new Vector3(sender.x, sender.y + offsetY, sender.z);
        }
        
        public static Vector3 OffsetZ(this Vector3 sender, float offsetZ)
        {
            return new Vector3(sender.x, sender.y, sender.z + offsetZ);
        }

        public static float Dot(this Vector2 v1, Vector2 v2)
        {
            var ans = v1.x * v2.x + v1.y * v2.y;
            return ans;
        }

        public static float Angle(this Vector2 from, Vector2 to)
        {
            return Vector3.Angle(from, to);
        }

        public static float SignedAngle(this Vector2 from, Vector2 to)
        {
            return Vector3.SignedAngle(from, to, Vector3.forward);
        }
        
        public static Vector2 RotateTowards(this Vector2 from, Vector2 to, float maxRotateAngle = Infinity)
        {
            // TODO Implementation
            return new Vector2();
        }
    }
}

