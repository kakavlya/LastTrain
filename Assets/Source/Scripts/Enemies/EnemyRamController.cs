using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyRamController : MonoBehaviour
{
    public enum State { Hold, Charge, Impact }

    private Transform _player;
    private BoxCollider _playerCollider; 
    private EnemyMovement _movement;
    private float _safeOffset = 5f;

    private float _holdDistance;

    private float _holdSpeed;
    private float _chargeSpeed;

    private float _impactPause;
    private float _holdPauseMin;
    private float _holdPauseMax;

    private int _damage;

    private State _state;
    private float _stateTimer;
    private Vector3 _retreatTarget;


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
        _safeOffset = impactOffset;
        _holdDistance = holdDistance;        

        _holdSpeed = holdSpeed;
        _chargeSpeed = chargeSpeed;

        _impactPause = impactPause;
        _holdPauseMin = holdPauseRange.x;
        _holdPauseMax = holdPauseRange.y;
        _damage = damage;
        EnterHold();
    }

    private void Update()
    {
        if (_player == null)
            return; 

        _stateTimer -= Time.deltaTime;

        switch (_state)
        {
            case State.Hold: UpdateHold(); break;
            case State.Charge: UpdateCharge(); break;
            case State.Impact: UpdateImpact(); break;
        }
    }

    private void EnterHold()
    {
        _state = State.Hold;
        _stateTimer = Random.Range(_holdPauseMin, _holdPauseMax);
        _movement.SetSpeed(_holdSpeed);
    }

    private void UpdateHold()
    {
        _stateTimer -= Time.deltaTime;

        Vector3 dir = (transform.position - _player.position).WithY(0f).normalized;
        Vector3 targetPos = _player.position + dir * _holdDistance;

        _movement.MoveForwardTo(targetPos);

        if (_stateTimer <= 0f)
            EnterCharge();
    }

    private void EnterCharge()
    {
        _state = State.Charge;
        _movement.SetSpeed(_chargeSpeed);
    }

    private void UpdateCharge()
    {
        _movement.MoveForwardTo(_player.position);

        // closest point on player collider
        Vector3 closest = _playerCollider.ClosestPoint(transform.position);
        Vector3 dirFromSurface = (transform.position - closest).WithY(0f);
        float dist = dirFromSurface.magnitude;

        if (dist <= _safeOffset)
        {
            if (dist > 0.001f)                    // null divide protection
                transform.position = closest + dirFromSurface.normalized * _safeOffset;
            else
                transform.position = closest - _player.forward * _safeOffset;

            EnterImpact();
        }
    }

    private void EnterImpact()
    {
        _state = State.Impact;
        _stateTimer = _impactPause;

        _movement.SetSpeed(0f);
        _player.GetComponent<IDamageable>()?.TakeDamage(20);
    }

    private void UpdateImpact()
    {
        _stateTimer -= Time.deltaTime;

        float t = 1f - (_stateTimer / _impactPause);         // 0 → 1
        float speedNow = Mathf.Lerp(0f, _chargeSpeed, t);
        _movement.SetSpeed(speedNow);

        if (_stateTimer <= 0f)
            EnterCharge();
    }

}
