using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;
    [SerializeField] private Vector3 _angles;

    private void Awake()
    {
        transform.rotation = Quaternion.Euler(_angles);
    }

    private void Update()
    {
        transform.position = _target.position + _offset;
    }
}
