using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponRotator : MonoBehaviour
    {
        [SerializeField] private WeaponsHandler _weaponHandler;
        [SerializeField] private AimingTargetProvider _targetProvider;
        [SerializeField] private float _rotationSpeed = 180f;

        private Transform _weaponPivot;

        public void Init()
        {
            _weaponHandler.OnWeaponChange += SetWeaponPivot;
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
            if (_targetProvider == null || !_targetProvider.AimPointWorld.HasValue)
                return;

            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Vector3 aimPoint = _targetProvider.AimPointWorld.Value;

            Vector3 direction = aimPoint - _weaponPivot.position;
            direction.y = 0f;

            if (direction.sqrMagnitude > 0.01f)
            {
                
                direction.Normalize();
                Quaternion targetRotation = Quaternion.LookRotation(direction, Vector3.up);
                _weaponPivot.rotation = Quaternion.RotateTowards(
                    _weaponPivot.rotation,
                    targetRotation,
                    _rotationSpeed * Time.deltaTime
                );
            }
        }
    }
}