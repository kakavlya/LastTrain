using UnityEngine;

namespace LastTrain.Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        [SerializeField] private float _turnSpeed = 120f;

        [SerializeField] private float _moveSpeed;
        public float TurnSpeed => _turnSpeed;

        public void SetTurnSpeed(float speed) => _turnSpeed = speed;

        public void SetSpeed(float speed)
        {
            if (float.IsNaN(speed) || float.IsInfinity(speed))
            {
                _moveSpeed = 0f;
                return;
            }

            _moveSpeed = speed;

            #if UNITY_EDITOR || DEVELOPMENT_BUILD
    if (Mathf.Approximately(speed, 0f))
    {
        Debug.Log($"[Movement] SetSpeed(0) on {name}\n{UnityEngine.StackTraceUtility.ExtractStackTrace()}");
    }
#endif
        }

        public void MoveForwardTo(Vector3 target)
        {
            float dt = Time.deltaTime;

            if (dt <= 0f) return;

            Vector3 to = (target - transform.position).WithY(0f);
            float sqr = to.sqrMagnitude;

            if (sqr > 1e-6f)
            {
                Vector3 dir = to / Mathf.Sqrt(sqr);
                var desired = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, desired, _turnSpeed * dt);
            }

            Vector3 fwd = transform.forward;

            if (float.IsNaN(fwd.x) || float.IsNaN(fwd.y) || float.IsNaN(fwd.z) ||
                float.IsInfinity(fwd.x) || float.IsInfinity(fwd.y) || float.IsInfinity(fwd.z))
            {
                transform.rotation = Quaternion.identity;
                fwd = Vector3.forward;
            }

            if (float.IsNaN(_moveSpeed) || float.IsInfinity(_moveSpeed)) _moveSpeed = 0f;

            transform.position += fwd * (_moveSpeed * dt);
        }

        public void MoveForward()
        {
            transform.position += transform.forward * (_moveSpeed * Time.deltaTime);
        }
    }
}

public static class Vector3Extensions
{
    public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
}

