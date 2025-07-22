using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponRotator : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private WeaponsHandler _weaponHandler;
        [SerializeField] private AimingTargetProvider _targetProvider;
        [SerializeField] private float _rotationSpeed = 180f;

        private Transform _weaponPivot;
        private bool _isMobilePlatform;

        public event Action<Vector3> Rotated;

        public void Init()
        {
            _weaponHandler.OnWeaponChange += SetWeaponPivot;

            if (PlatformDetector.Instance != null && PlatformDetector.Instance.CurrentControlScheme == PlatformDetector.ControlScheme.Joystick)
            {
                _isMobilePlatform = true;
            }
            else
            {
                _isMobilePlatform = false;
            }
        }

        private void OnDisable()
        {
            _weaponHandler.OnWeaponChange -= SetWeaponPivot;
        }

        private void SetWeaponPivot(Weapon weapon)
        {
            _weaponPivot = weapon.transform;
        }

        private void Update()
        {
            if (_isMobilePlatform)
            {
                RotateWithJoystick();
            }
            else
            {
                RotateWithMouse();
            }
        }

        private void RotateWithMouse()
        {
            if (_targetProvider == null || !_targetProvider.AimPointWorld.HasValue)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3 aimPoint = _targetProvider.AimPointWorld.Value;

            Vector3 direction = aimPoint - _weaponPivot.position;
            direction.y = 0f;

            RotateTowardsDirection(direction);
        }

        private void RotateWithJoystick()
        {
            Vector3 direction = new Vector3(_joystick.Horizontal, 0, _joystick.Vertical);

            if (direction.sqrMagnitude > 0.01f)
            {
                direction.Normalize();
                RotateTowardsDirection(direction);

            }
        }

        private void RotateTowardsDirection(Vector3 direction)
        {
            if (direction.sqrMagnitude > 0.01f)
            {

                direction.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                _weaponPivot.rotation = Quaternion.RotateTowards(
                    _weaponPivot.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );

                Rotated?.Invoke(direction);
            }
        }
    }
}