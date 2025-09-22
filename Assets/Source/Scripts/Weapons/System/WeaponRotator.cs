using System;
using UnityEngine;
using UnityEngine.EventSystems;
using LastTrain.Weapons.Types;

namespace LastTrain.Weapons.System
{
    public class WeaponRotator : MonoBehaviour
    {
        [SerializeField] private WeaponsHandler _weaponHandler;
        [SerializeField] private AimingTargetProvider _targetProvider;
        [SerializeField] private float _rotationSpeed = 180f;

        private Transform _weaponPivot;
        private Transform _muzzle;

        public event Action<Vector3> Rotated;

        private void Update() => Rotate();

        private void OnDisable()
        {
            if (_weaponHandler != null)
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
        }

        private void Rotate()
        {
            if (_targetProvider == null || _weaponPivot == null)
                return;

            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
                return;

            var ad = _targetProvider.GetAim();
            Vector3 aimPoint = ad.WorldPoint;
            Vector3 direction = aimPoint - _weaponPivot.position;
            direction.y = 0f;

            if (direction.sqrMagnitude < 0.01f)
                return;

            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
            _weaponPivot.rotation = Quaternion.RotateTowards(
                _weaponPivot.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            Rotated?.Invoke(direction.normalized);
        }
    }
}
