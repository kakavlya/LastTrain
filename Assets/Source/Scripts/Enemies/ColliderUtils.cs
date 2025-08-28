using UnityEngine;

namespace LastTrain.Enemies
{
    public class ColliderUtils
    {
        public static float Distance(Collider a, Collider b,
       out Vector3 direction, out bool isOverlapped)
        {
            isOverlapped = Physics.ComputePenetration(
                a, a.transform.position, a.transform.rotation,
                b, b.transform.position, b.transform.rotation,
                out direction, out float penetrationDepth);

            if (isOverlapped)
            {
                return 0f;
            }

            Vector3 pointA = a.ClosestPoint(b.transform.position);
            Vector3 pointB = b.ClosestPoint(pointA);

            Vector3 delta = pointA - pointB;
            float dist = delta.magnitude;

            if (dist > 1e-5f)
                direction = delta / dist;
            else
                direction = Vector3.up;

            return dist;
        }
    }
}