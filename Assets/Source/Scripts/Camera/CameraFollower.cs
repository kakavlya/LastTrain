using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class CameraFollower : MonoBehaviour
{
    [Header("Input Reference")]
    [SerializeField] private PlayerInput _playerInput;

    [Header("Target Settings")]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _targetOffset = new Vector3(0, 70f, -100f);

    [Header("Camera Settings")]
    [SerializeField] private float _distance = 100f;
    [SerializeField] private float _rotationSpeed = 90f;
    [SerializeField] private float _fixedPitch = 20f;

    [Header("Smoothing")]
    [SerializeField] private float _rotationSmoothing = 10f;

    private float _rotationAngle;

    private void Awake()
    {
        _playerInput.Rotated += UpdateCameraPositionAndRotate;
    }

    private void UpdateCameraPositionAndRotate(float rotateValue)
    {
        _rotationAngle += rotateValue * _rotationSpeed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.Euler(_fixedPitch, _rotationAngle, 0f);
        Quaternion smoothRotation = Quaternion.Lerp(transform.rotation, targetRotation, _rotationSmoothing * Time.deltaTime);

        Vector3 offset = new Vector3(0, 0, -_distance);
        Vector3 targetPosition = targetRotation * offset + _target.position + _targetOffset;

        transform.rotation = smoothRotation;
        transform.position = targetPosition;
    }
}
