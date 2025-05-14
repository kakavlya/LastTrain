using UnityEngine;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponModule : MonoBehaviour
    {
        [SerializeField] private AimingTargetProvider _aimingTarget;
        [SerializeField] private Weapon _weapon;
        [SerializeField] private WeaponRotator _weaponRotator;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                _weapon.Fire();
            }
        }
    }
}
