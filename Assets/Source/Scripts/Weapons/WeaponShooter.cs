using Assets.Source.Scripts.Weapons;
using UnityEngine;

public class WeaponShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform _firePoint;
    [SerializeField] private Projectile _projectilePrefab;
    [SerializeField] private AimingTargetProvider _aimingTarget;
    [SerializeField] private ParticleSystem _muzzleFlash;        
    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private AudioSource _audioSource;

    [Header("Settings")]
    [SerializeField] private bool _usePooling = false;                
    [SerializeField] private float _fireDelay = 0.1f;                 
    private float _lastFireTime;

    private GameObject _owner;

    public void Initialize(GameObject owner, AimingTargetProvider aimer, bool usePooling = false)
    {
        _aimingTarget = aimer;
        _owner = owner;
        _usePooling = usePooling;
    }

    public void Fire()
    {

        if (Time.time - _lastFireTime < _fireDelay)
            return;

        if (_aimingTarget == null || !_aimingTarget.AimPointWorld.HasValue)
            return;

        _lastFireTime = Time.time;


        if (_muzzleFlash != null)
        {
            _muzzleFlash.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            _muzzleFlash.Play();
        }
        if (_audioSource != null && _shootSound != null)
            _audioSource.PlayOneShot(_shootSound);


        Vector3 target = _aimingTarget.AimPointWorld.Value;
        target.y = 0f;
        Vector3 origin = _firePoint.position;
        Vector3 direction = (target - origin).normalized;


        var proj = _usePooling
        ? ProjectilePool.Instance.Spawn(origin, Quaternion.LookRotation(direction), owner: this.gameObject, damage: 50)
        : Instantiate(_projectilePrefab, origin, Quaternion.LookRotation(direction));


        proj.Configure(
            owner: _owner,
            usePooling: _usePooling,
            damage: proj.Damage,       
            lifetime: proj.Lifetime,
            speed: proj.Speed
        );
    }
}
