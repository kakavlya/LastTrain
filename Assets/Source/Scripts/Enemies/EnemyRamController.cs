using UnityEngine;

namespace LastTrain.Enemies
{
    [RequireComponent(typeof(EnemyMovement), typeof(BoxCollider))]
    [DisallowMultipleComponent]
    public class EnemyRamController : MonoBehaviour
    {
        [SerializeField] private float _accel = 25f;
        [SerializeField] private float _decel = 45f;
        [SerializeField] private float _impactSlowFactor = 0.25f;

        private Transform _player;
        private BoxCollider _playerCollider;
        private EnemyMovement _movement;
        private EnemyHealth _health;
        private float _safeOffset;
        private float _holdDistance;
        private float _holdSpeed;
        private float _chargeSpeed;
        private float _impactPause;
        private float _holdPauseMin;
        private float _holdPauseMax;
        private int _damage;
        private State _state;
        private float _stateTimer;
        private float _currentSpeed;
        private float _targetSpeed;
        private bool _isAlive;
        private bool _canDealDamageWindow;

        public enum State { Hold, Charge, Impact }

        public void SetTurnSpeed(float turnSpeed) => _movement?.SetTurnSpeed(turnSpeed);

        private void Update()
        {
            if (!_isAlive) return;
            if (_player == null || _playerCollider == null) return;

            float dt = Time.deltaTime;
            float rate = (_currentSpeed < _targetSpeed) ? _accel : _decel;
            _currentSpeed = Mathf.MoveTowards(_currentSpeed, _targetSpeed, rate * dt);
            _movement.SetSpeed(_currentSpeed);

            switch (_state)
            {
                case State.Hold: UpdateHold(); break;

                case State.Charge: UpdateCharge(); break;

                case State.Impact: UpdateImpact(); break;
            }
        }

        public void Init(
            Transform player,
            BoxCollider playerCollider,
            float impactOffset,
            float holdDistance,
            float holdSpeed,
            float chargeSpeed,
            float impactPause,
            Vector2 holdPauseRange,
            int damage
        )
        {
            _player = player;
            _playerCollider = playerCollider;
            _movement = GetComponent<EnemyMovement>();
            _health = GetComponent<EnemyHealth>();
            _health.OnDeath.AddListener(HandleDeath);
            _safeOffset = Mathf.Max(0.01f, impactOffset);
            _holdDistance = Mathf.Max(0.01f, holdDistance);
            _holdSpeed = Mathf.Max(0f, holdSpeed);
            _chargeSpeed = Mathf.Max(0f, chargeSpeed);
            _impactPause = Mathf.Max(0f, impactPause);
            _holdPauseMin = Mathf.Min(holdPauseRange.x, holdPauseRange.y);
            _holdPauseMax = Mathf.Max(holdPauseRange.x, holdPauseRange.y);
            _isAlive = true;
            _damage = damage;

            _isAlive = true;
            _canDealDamageWindow = false;

            _currentSpeed = 0f;
            _targetSpeed = _holdSpeed;                

            EnterHold();
        }

        private void EnterHold()
        {
            _state = State.Hold;
            _stateTimer = Random.Range(_holdPauseMin, _holdPauseMax);
            _targetSpeed = _holdSpeed;
        }

        private void UpdateHold()
        {
            _stateTimer -= Time.deltaTime;
            Vector3 anchor = SafeAnchorOnPlane();
            Vector3 dirOut = SafeOutwardsDir(anchor);
            Vector3 target = anchor + dirOut * _holdDistance;
            target.y = transform.position.y;
            _movement.MoveForwardTo(target);

            if (_stateTimer <= 0f)
                EnterCharge();
        }

        private void EnterCharge()
        {
            _state = State.Charge;
            _targetSpeed = _chargeSpeed;
            _canDealDamageWindow = true;
        }

        private void UpdateCharge()
        {
            if (!_isAlive) return;

            Vector3 anchor = SafeAnchorOnPlane();
            Vector3 toAnchor = (anchor - transform.position);
            toAnchor.y = 0f;
            float dist = toAnchor.magnitude;

            if (dist > 1e-6f)
            {
                Vector3 dir = toAnchor / dist;
                float step = _currentSpeed * Time.deltaTime;

                if (dist - _safeOffset <= step)
                {
                    transform.position = anchor - dir * _safeOffset;

                    if (_canDealDamageWindow)
                    {
                        var dmg = _player.GetComponent<IDamageable>();
                        if (dmg != null)  
                            dmg.TakeDamage(_damage);

                        _canDealDamageWindow = false; 
                    }

                    EnterImpact();
                    return;
                }
            }

            _movement.MoveForwardTo(anchor);
        }

        private void EnterImpact()
        {
            _state = State.Impact;
            _stateTimer = _impactPause;
            _targetSpeed = Mathf.Max(_holdSpeed, _chargeSpeed * Mathf.Clamp01(_impactSlowFactor));
            _currentSpeed = Mathf.Min(_currentSpeed, _targetSpeed);
            _canDealDamageWindow = false;
        }


        private void UpdateImpact()
        {
            _stateTimer -= Time.deltaTime;
            Vector3 anchor = SafeAnchorOnPlane();
            Vector3 toAnchor = anchor - transform.position;
            toAnchor.y = 0f;
            float dist = toAnchor.magnitude;

            if (dist > 1e-6f)
            {
                Vector3 dir = toAnchor / dist;
                float step = _currentSpeed * Time.deltaTime;

                if (dist - _safeOffset <= step)
                {
                    transform.position = anchor - dir * _safeOffset;
                }
                else
                {
                    _movement.MoveForwardTo(anchor);
                }
            }

            if (_stateTimer <= 0f)
                EnterCharge();
        }

        private Vector3 SafeAnchorOnPlane()
        {
            Vector3 anchor = _playerCollider.ClosestPoint(transform.position);
            anchor.y = transform.position.y;

            if (float.IsNaN(anchor.x) || float.IsNaN(anchor.y) || float.IsNaN(anchor.z))
                anchor = new Vector3(_player.position.x, transform.position.y, _player.position.z);

            return anchor;
        }

        private void HandleDeath()
        {
          
            if (!_isAlive) return;
            _isAlive = false;
            _canDealDamageWindow = false;

            _movement?.SetSpeed(0f);
            enabled = false;
        }

        private Vector3 SafeOutwardsDir(Vector3 anchor)
        {
            Vector3 d = transform.position - anchor;
            d.y = 0f;

            if (d.sqrMagnitude < 1e-6f)
            {
                d = transform.position - _player.position;
                d.y = 0f;
                if (d.sqrMagnitude < 1e-6f) d = Vector3.forward;
            }

            return d.normalized;
        }
    }
}
