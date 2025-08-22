using Assets.Source.Scripts.Enemies;
using UnityEngine;

[RequireComponent(typeof(EnemyMovement), typeof(BoxCollider))]
[DisallowMultipleComponent]
public class EnemyRamController : EnemyController
{
    public enum State { Hold, Charge, Impact }

    private Transform _player;
    private BoxCollider _playerCollider;
    private BoxCollider _enemyCollider;
    private EnemyMovement _movement;

    private float _safeOffset;
    private float _holdDistance;
    private float _holdSpeed;
    private float _chargeSpeed;
    private float _impactPause;
    private float _holdPauseMin;
    private float _holdPauseMax;
    private int _damage;

    private float _maxAccel = 30f;
    private float _maxDecel = 40f;
    private float _speedSmoothTime = 0.12f;
    private float _checkRadius = 10f;
    private AnimationCurve _impactRecover = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private float _checkRadiusSqr;


    private State _state;
    private float _stateTimer;
    private float _currentSpeed;
    private float _targetSpeed;
    private float _speedVel;

    public void Init(
        Transform player,
        BoxCollider playerCollider,
        float impactOffset,
        float holdDistance,
        float holdSpeed,
        float chargeSpeed,
        float impactPause,
        Vector2 holdPauseRange, // x = min, y = max
        int damage
    )
    {
        _player = player;
        _playerCollider = playerCollider;

        _movement = GetComponent<EnemyMovement>();
        _enemyCollider = GetComponent<BoxCollider>();

        _safeOffset = impactOffset;
        _holdDistance = holdDistance;
        _holdSpeed = holdSpeed;
        _chargeSpeed = chargeSpeed;
        _impactPause = impactPause;
        _holdPauseMin = holdPauseRange.x;
        _holdPauseMax = holdPauseRange.y;
        _damage = damage;

        _checkRadiusSqr = _checkRadius * _checkRadius;

        EnterHold();
    }

    /// <summary>
    /// Опциональный шаг: передать тюнинг динамики из SO (ускорение/торможение/сглаживание/радиус/кривая).
    /// Если не вызывать — будут дефолты выше.
    /// </summary>
    public void ConfigureDynamics(
        float maxAccel,
        float maxDecel,
        float speedSmoothTime,
        float checkRadius,
        AnimationCurve impactRecover
    )
    {
        _maxAccel = maxAccel;
        _maxDecel = maxDecel;
        _speedSmoothTime = speedSmoothTime;
        _checkRadius = Mathf.Max(0.01f, checkRadius);
        _impactRecover = impactRecover ?? AnimationCurve.Linear(0, 0, 1, 1);
        _checkRadiusSqr = _checkRadius * _checkRadius;
    }

    public void SetTurnSpeed(float turnSpeed)
    {
        _movement?.SetTurnSpeed(turnSpeed);
    }

    private void Update()
    {
        if (_player == null) return;

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case State.Hold: UpdateHold(); break;
            case State.Charge: UpdateCharge(); break;
            case State.Impact: UpdateImpact(); break;
        }

        // smoothing speed
        _currentSpeed = SmoothSpeed(_currentSpeed, _targetSpeed, Time.deltaTime);
        _movement.SetSpeed(_currentSpeed);
    }

    // -------- HOLD --------
    private void EnterHold()
    {
        _state = State.Hold;
        _stateTimer = Random.Range(_holdPauseMin, _holdPauseMax);
        _targetSpeed = _holdSpeed;
    }

    private void UpdateHold()
    {
        Vector3 anchor = _playerCollider.ClosestPoint(transform.position); // поверхность игрока
        Vector3 fromAnchor = transform.position - anchor;
        fromAnchor.y = 0f;
        Vector3 dir = fromAnchor.sqrMagnitude > 0.0001f ? fromAnchor.normalized : -transform.forward;

        Vector3 targetPos = anchor + dir * _holdDistance;
        _movement.MoveForwardTo(targetPos);

        if (_stateTimer <= 0f)
            EnterCharge();
    }


    private void EnterCharge()
    {
        _state = State.Charge;
        _targetSpeed = _chargeSpeed;
    }

    private void UpdateCharge()
    {
        _movement.MoveForwardTo(GetChargeTargetPoint());

        bool overlapped;
        Vector3 sepDir;
        float gap = ColliderUtils.Distance(_enemyCollider, _playerCollider, out sepDir, out overlapped);
        float distToSafe = overlapped ? 0f : Mathf.Max(0f, gap - _safeOffset);

        float v = Mathf.Max(0.01f, _currentSpeed);
        float brakingDist = (v * v) / (2f * Mathf.Max(0.01f, _maxDecel)) + 0.1f;

        if (distToSafe <= brakingDist)
        {
            float k = Mathf.Clamp01(distToSafe / Mathf.Max(0.01f, brakingDist));
            float desired = Mathf.Lerp(0f, _chargeSpeed, k);
            _targetSpeed = Mathf.Min(_targetSpeed, desired);
        }
        else
        {
            _targetSpeed = Mathf.Max(_targetSpeed, _chargeSpeed * 0.9f);
        }

        if (overlapped || distToSafe <= 0.02f)
        {
            float maxBack = _currentSpeed * Time.deltaTime;
            float correction = overlapped ? _safeOffset : (_safeOffset - gap);
            if (correction > 0f)
                transform.position += sepDir * Mathf.Min(correction, maxBack);

            _player.GetComponent<IDamageable>()?.TakeDamage(_damage);
            EnterImpact();
        }
    }


    private void EnterImpact()
    {
        _state = State.Impact;
        _stateTimer = _impactPause;
        _targetSpeed = 0f; // for smoothing
    }

    private void UpdateImpact()
    {
        float t = 1f - Mathf.Clamp01(_stateTimer / Mathf.Max(0.0001f, _impactPause)); // 0 1
        float curve = (_impactRecover != null && _impactRecover.length > 0) ? _impactRecover.Evaluate(t) : t;
        _targetSpeed = Mathf.Lerp(0f, _chargeSpeed, curve);

        if (_stateTimer <= 0f)
            EnterCharge();
    }

    private float SmoothSpeed(float current, float target, float dt)
    {        
        float delta = target - current;
        float maxDelta = (target > current ? _maxAccel : _maxDecel) * dt;
        float clampedTarget = current + Mathf.Clamp(delta, -maxDelta, maxDelta);

        return Mathf.SmoothDamp(current, clampedTarget, ref _speedVel, _speedSmoothTime, Mathf.Infinity, dt);
    }

    private Vector3 GetChargeTargetPoint()
    {
        Vector3 closest = _playerCollider.ClosestPoint(transform.position);
        Vector3 toClosest = closest - transform.position;
        toClosest.y = 0f;

        float dist = toClosest.magnitude;
        if (dist < 0.0001f)
            return transform.position;

        Vector3 dir = toClosest / dist;
        return closest - dir * _safeOffset;
    }
}
