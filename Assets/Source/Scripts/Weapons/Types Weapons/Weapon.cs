using Assets.Source.Scripts.Weapons;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform _firePoint;
    [SerializeField] protected Projectile _projectilePrefab;
    [SerializeField] protected AimingTargetProvider _aimingTarget;       

    [Header("Settings")]
    [SerializeField] protected bool _usePooling = true;                
    [SerializeField] protected float _fireDelay = 0.1f;   
    
    protected float _lastFireTime;
    protected GameObject _owner;

    public abstract void Fire();
}
