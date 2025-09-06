using System;
using UnityEngine;
using UnityEngine.EventSystems;
using LastTrain.Weapons.Types;

namespace LastTrain.Weapons.System
{
    public class WeaponRotator : MonoBehaviour
    {
        [SerializeField] private Joystick _joystick;
        [SerializeField] private WeaponsHandler _weaponHandler;
        [SerializeField] private AimingTargetProvider _targetProvider;
        [SerializeField] private float _rotationSpeed = 180f;

        private Transform _weaponPivot;
        private Transform _muzzle;

        public event Action<Vector3> Rotated;

        private void Update()
        {
            Rotate();
        }

        private void OnDisable()
        {
            _weaponHandler.OnWeaponChange -= SetWeaponPivot;
        }

        public void Init()
        {
            _weaponHandler.OnWeaponChange += SetWeaponPivot;
        }

        private void SetWeaponPivot(Weapon weapon)
        {
            _weaponPivot = weapon.transform;
            _muzzle = weapon.FirepointPosition != null ? weapon.FirepointPosition : weapon.transform;

            _targetProvider.SetPlaneHeight(_muzzle.position.y);
        }

        private void Rotate()
        {
            if (_targetProvider == null || _weaponPivot == null) return;
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject()) return;

            Vector3 aimPoint = _targetProvider.GetTargetPoint();

            Vector3 direction = aimPoint - _weaponPivot.position;
            direction.y = 0f; 
            if (direction.sqrMagnitude < 0.01f) return;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            _weaponPivot.rotation = Quaternion.RotateTowards(
                _weaponPivot.rotation, targetRotation, _rotationSpeed * Time.deltaTime
            );

            if (_muzzle != null) Debug.DrawLine(_muzzle.position, aimPoint, Color.yellow);
            Debug.DrawRay(_weaponPivot.position, _weaponPivot.forward * 5f, Color.green);
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