using Assets.Source.Scripts.Weapons;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform FirePoint;
    [SerializeField] protected Projectile ProjectilePrefab;

    [SerializeField] private AimingTargetProvider _aimingTarget;       

    [Header("Settings")]
    [SerializeField] protected bool UsePooling = true;                
    [SerializeField] protected float FireDelay = 0.1f;   
    
    protected Vector3 Direction;
    protected GameObject Owner;

    private float _lastFireTime;

    private void OnEnable()
    {
        Owner = GetComponent<GameObject>();
    }

    private bool FirePossible()
    {
        if (Time.time - _lastFireTime < FireDelay)
            return false;

        if (_aimingTarget == null || !_aimingTarget.AimPointWorld.HasValue)
            return false;

        _lastFireTime = Time.time;
        return true;
    }

    private void CalculateDirection()
    {
        Vector3 target = _aimingTarget.AimPointWorld.Value;
        target.y = 0f;
        Vector3 origin = FirePoint.position;
        Vector3 direction = (target - origin).normalized;
        Direction = direction;
    }

    public virtual void Fire()
    {
        CalculateDirection();

        if (FirePossible() == false)
            return;


        OnWeaponFire();
    }

    protected abstract void OnWeaponFire();
}
