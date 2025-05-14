using UnityEngine;

public class Rifle : Weapon
{
    public override void Fire()
    {
        if (Time.time - _lastFireTime < _fireDelay)
            return;

        if (_aimingTarget == null || !_aimingTarget.AimPointWorld.HasValue)
            return;

        _lastFireTime = Time.time;

        Vector3 target = _aimingTarget.AimPointWorld.Value;
        target.y = 0f;
        Vector3 origin = _firePoint.position;
        Vector3 direction = (target - origin).normalized;

        var proj = _usePooling
        ? ProjectileTypesPool.Instance.Spawn(_projectilePrefab, origin, Quaternion.LookRotation(direction), owner: gameObject)
        : Instantiate(_projectilePrefab, origin, Quaternion.LookRotation(direction));


        proj.Configure(
            owner: _owner,
            usePooling: _usePooling
        );
    }
}
