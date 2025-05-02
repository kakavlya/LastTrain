using Assets.Source.Scripts.Weapons;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Projectile _projectile;
    [SerializeField] private AimingTargetProvider _aimingTarget;
    [SerializeField] private GameObject _ownerToIgnore;
    [SerializeField] private ParticleSystem _muzzleFlash;

    public void Fire()
    {
        if (_aimingTarget == null || !_aimingTarget.AimPointWorld.HasValue)
            return;

        Vector3 target = _aimingTarget.AimPointWorld.Value;
        target.y = 0f; 

        Vector3 firePosition = _firePoint.position;

        Vector3 direction = (target - firePosition).normalized;

        GameObject projectileGO = Instantiate(_projectile.gameObject, firePosition, Quaternion.LookRotation(direction));

        Rigidbody rb = projectileGO.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.velocity = direction * _projectile.Getspeed();

            if (_ownerToIgnore != null)
            {
                Collider[] ownerColliders = _ownerToIgnore.GetComponentsInChildren<Collider>();
                Collider projectileCollider = projectileGO.GetComponent<Collider>();

                foreach (var col in ownerColliders)
                    Physics.IgnoreCollision(projectileCollider, col);
            }

            if (_muzzleFlash != null)
            {
                _muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                _muzzleFlash.Play();
            }
        }


        //Destroy(projectileGO, _projectile.lifetime);
    }

    public void SetAimer(AimingTargetProvider weaponAimer)
    {
        _aimingTarget = weaponAimer;
    }
}
