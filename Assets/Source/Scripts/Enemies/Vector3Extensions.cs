using UnityEngine;

namespace LastTrain.Enemies
{
    public static class Vector3Extensions
    {
        public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
    }
}
