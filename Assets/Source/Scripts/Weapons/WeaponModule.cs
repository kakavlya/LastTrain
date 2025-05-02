using Assets.Source.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Source.Scripts.Weapons
{
    public class WeaponModule : MonoBehaviour
    {
        [SerializeField] private AimingTargetProvider _aimingTarget;
        [SerializeField] private WeaponShooter _weaponShooter;
        [SerializeField] private WeaponRotator _weaponRotator;

        private void Awake()
        {
            _weaponShooter.SetAimer(_aimingTarget);
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
