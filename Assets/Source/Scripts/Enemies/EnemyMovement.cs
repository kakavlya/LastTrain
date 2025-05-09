using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float _turnSpeed = 120f; // градусы в секунду
    private float _moveSpeed;

    public void SetSpeed(float speed) => _moveSpeed = speed;
    public void MoveForwardTo(Vector3 target)
    {
        Vector3 dir = (target - transform.position).WithY(0f).normalized;
        if (dir.sqrMagnitude > 0.001f)
        {
            Quaternion desired = Quaternion.LookRotation(dir, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(
                transform.rotation,
                desired,
                _turnSpeed * Time.deltaTime
            );
        }

        transform.position += transform.forward * (_moveSpeed * Time.deltaTime);
    }

    public void MoveForward()
    {
        transform.position += transform.forward * (_moveSpeed * Time.deltaTime);
    }
}

public static class Vector3Extensions
{
    public static Vector3 WithY(this Vector3 v, float y) => new Vector3(v.x, y, v.z);
}
