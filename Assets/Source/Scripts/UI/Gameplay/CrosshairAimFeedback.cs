using UnityEngine;
using UnityEngine.UI;
using LastTrain.Weapons.System;
using LastTrain.Weapons.Types;
using LastTrain.Enemies; // для EnemyHealth (опционально)

[RequireComponent(typeof(Image))]
public class CrosshairAimFeedback : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AimingTargetProvider _aim;
    [SerializeField] private WeaponsHandler _weapons;

    [Header("Detection")]
    [Tooltip("Слои врагов. Если 0 — будетfallback по компонентам EnemyHealth/IDamageable.")]
    [SerializeField] private LayerMask _enemyMask = 0;
    [Tooltip("Что блокирует выстрел от дула (земля/стены/укрытия). НЕ включай Enemy/Player/Weapon/UI/Ignore Raycast.")]
    [SerializeField] private LayerMask _losBlockMask = 0;
    [SerializeField] private float _camCheckDistance = 5000f;
    [SerializeField] private float _selfEpsilon = 0.02f; 

    [Header("Colors")]
    [SerializeField] private Color _defaultColor = Color.red;
    [SerializeField] private Color _reachableColor = Color.green;
    [SerializeField] private bool _useTooFarColor = true;                 
    [SerializeField] private float _lerpSpeed = 12f;

    [Header("Debug")]
    [SerializeField] private bool _debugDraw = false;

    private Image _img;
    private Weapon _weapon;

    private enum State { None, EnemyTooFar, EnemyReachable }

    private void Awake()
    {
        _img = GetComponent<Image>();
        _img.color = _defaultColor;
    }

    private void OnEnable()
    {
        if (_weapons != null) _weapons.OnWeaponChange += OnWeaponChange;
        if (_weapons != null) _weapon = _weapons.CurrentWeapon;
    }

    private void OnDisable()
    {
        if (_weapons != null) _weapons.OnWeaponChange -= OnWeaponChange;
    }

    private void OnWeaponChange(Weapon w) => _weapon = w;

    private void Update()
    {
        var state = GetState(out var ad, out var hit);

        if (_debugDraw && ad.CamRay.direction != Vector3.zero)
            Debug.DrawLine(ad.CamRay.origin, ad.WorldPoint, Color.cyan);

        var target = _defaultColor;
        switch (state)
        {
            case State.EnemyReachable: target = _reachableColor; break;
            case State.None: target = _defaultColor; break;
        }

        _img.color = Color.Lerp(_img.color, target, Time.unscaledDeltaTime * _lerpSpeed);

        if (_debugDraw && state != State.None && _weapon != null && _weapon.Muzzle != null)
            Debug.DrawLine(_weapon.Muzzle.position, hit.point, state == State.EnemyReachable ? Color.green : Color.yellow);
    }

    private State GetState(out AimData aimDirection, out RaycastHit enemyHit)
    {
        aimDirection = default;
        enemyHit = default;

        if (_aim == null || _weapon == null || _weapon.Muzzle == null)
            return State.None;

        aimDirection = _aim.GetAim();
        Ray camRay = aimDirection.CamRay;

        if (_enemyMask != 0)
        {
            if (!Physics.Raycast(camRay, out enemyHit, _camCheckDistance, _enemyMask, QueryTriggerInteraction.Ignore))
                return State.None;
        }
        else
        {
            if (!Physics.Raycast(camRay, out enemyHit, _camCheckDistance, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
                return State.None;

            bool isEnemy = enemyHit.collider.GetComponentInParent<EnemyHealth>() != null
                        || enemyHit.collider.GetComponentInParent<IDamageable>() != null;
            if (!isEnemy) return State.None;
        }

        Vector3 muzzle = _weapon.Muzzle.position;
        Vector3 toDistance = enemyHit.point - muzzle;
        float dist = toDistance.magnitude;
        if (dist <= 1e-4f) return State.None;

        if (_weapon.MaxRange > 0f && dist > _weapon.MaxRange)
            return State.EnemyTooFar;

        EnsureDefaultBlockMask();
        Vector3 dir = toDistance / dist;
        Vector3 originNoSelf = muzzle + dir * _selfEpsilon;

        if (Physics.Raycast(originNoSelf, dir, dist, _losBlockMask, QueryTriggerInteraction.Ignore))
            return State.EnemyTooFar;

        return State.EnemyReachable;
    }

    private void EnsureDefaultBlockMask()
    {
        if (_losBlockMask != 0) return;

        int exclude = 0;
        exclude |= LayerMask.GetMask("Enemy");
        exclude |= LayerMask.GetMask("Player");
        exclude |= LayerMask.GetMask("Weapon");
        exclude |= LayerMask.GetMask("UI");
        exclude |= LayerMask.GetMask("Ignore Raycast");

        _losBlockMask = Physics.DefaultRaycastLayers & ~exclude;
    }
}
