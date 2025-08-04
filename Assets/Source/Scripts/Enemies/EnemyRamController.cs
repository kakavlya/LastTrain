using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
public class EnemyRamController : MonoBehaviour
{
    public enum State { Hold, Charge, Impact }

    private Transform _player;
    private EnemyMovement _movement;

    private float _holdDistance;
    private float _impactRadius;
    private float _retreatDistance;

    private float _holdSpeed;
    private float _chargeSpeed;
    private float _retreatSpeed;
    private float _turnSpeed;

    private float _impactPause;
    private float _holdPauseMin;
    private float _holdPauseMax;

    private int _damage;

    private State _state;
    private float _stateTimer;
    private Vector3 _retreatTarget;

    public void Init(
        Transform player,
        float holdDistance,
        float impactRadius,
        float retreatDistance,
        float holdSpeed,
        float chargeSpeed,
        float retreatSpeed,
        float impactPause,
        float turnSpeed,
        Vector2 holdPauseRange, // x = min, y = max
        int damage
    )
    {
        _player = player;
        _movement = GetComponent<EnemyMovement>();

        _holdDistance = holdDistance;
        _impactRadius = impactRadius;
        _retreatDistance = retreatDistance;

        _holdSpeed = holdSpeed;
        _chargeSpeed = chargeSpeed;
        _retreatSpeed = retreatSpeed;
        _turnSpeed = turnSpeed;

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

        if (Vector3.Distance(transform.position, _player.position) <= _impactRadius)
            EnterImpact();
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
