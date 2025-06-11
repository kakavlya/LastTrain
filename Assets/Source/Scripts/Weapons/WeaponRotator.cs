using System.Collections;
using UnityEngine;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponRotator : MonoBehaviour
    {
        [SerializeField] private Transform _weaponPivot;
        [SerializeField] private AimingTargetProvider _targetProvider;
        [SerializeField] private float _rotationSpeed = 360f;

        public void SetAimingTargetProvider(AimingTargetProvider aimingTargetProvider)
        {
            _targetProvider = aimingTargetProvider;
        }
        private void Update()
        {
            if (_targetProvider == null || !_targetProvider.AimPointWorld.HasValue)
                return;
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