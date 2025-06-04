using UnityEngine;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponModule : MonoBehaviour
    {
        [SerializeField] private AimingTargetProvider _aimingTarget;
        [SerializeField] private WeaponShooter _weaponShooter;
        [SerializeField] private WeaponRotator _weaponRotator;

        private void Start()
        {
            _weaponShooter.Initialize(gameObject, _aimingTarget, false);
            _weaponRotator.SetAimingTargetProvider(_aimingTarget);
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _weaponShooter.Fire();
            }
        }
    }
}
