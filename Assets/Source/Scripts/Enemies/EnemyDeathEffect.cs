using UnityEngine;
using DG.Tweening;

namespace LastTrain.Enemies
{
    public class EnemyDeathEffect : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;
        [SerializeField] private float _torqueAmount = 180f;
        [SerializeField] private float _upwardForce = 3f;
        [SerializeField] private float _flyDuration = 1f;
        [SerializeField] private float _rotateDuration = 2f;
        [SerializeField] private float _disappearDelay = 0.1f;

        private Sequence _deathSequence;
        private Vector3 _initLocalPos;
        private Quaternion _initLocalRot;
        private Vector3 _initLocalScale;
        private bool _cached;

        private void Awake()
        {
            if (_visualRoot == null) _visualRoot = transform;
            CacheInitialPose();
        }

        private void OnEnable()
        {
            ResetEffect();
            RestoreInitialPose();
            if (_visualRoot != null && !_visualRoot.gameObject.activeSelf)
                _visualRoot.gameObject.SetActive(true);
        }

        private void OnDisable()
        {
            KillTweens();
        }

        public void Play()
        {
            if (_visualRoot == null) _visualRoot = transform;
            KillTweens();

            if (!_visualRoot.gameObject.activeSelf)
                _visualRoot.gameObject.SetActive(true);

            Vector3 launchVelocity = Vector3.up * _upwardForce + Random.insideUnitSphere;
            Vector3 targetPosition = _visualRoot.position + launchVelocity;
            Vector3 endEuler = Random.onUnitSphere * _torqueAmount;

            _deathSequence = DOTween.Sequence()
                .SetAutoKill(true)
                .SetLink(gameObject, LinkBehaviour.KillOnDisable);

            _deathSequence.Append(_visualRoot.DOMove(targetPosition, _flyDuration).SetEase(Ease.OutExpo));
            _deathSequence.Join(_visualRoot.DORotate(endEuler, _rotateDuration, RotateMode.FastBeyond360).SetEase(Ease.OutExpo));
            _deathSequence.AppendInterval(_disappearDelay);
        }

        public void ResetEffect()
        {
            _deathSequence?.Kill();
            _deathSequence = null;
        }

        public void KillTweens()
        {
            _deathSequence?.Kill(false);
            _deathSequence = null;

            if (_visualRoot != null) DOTween.Kill(_visualRoot, complete: false);
            _visualRoot?.DOKill(false);
        }

        private void CacheInitialPose()
        {
            if (_visualRoot == null) return;
            _initLocalPos = _visualRoot.localPosition;
            _initLocalRot = _visualRoot.localRotation;
            _initLocalScale = _visualRoot.localScale;
            _cached = true;
        }

        private void RestoreInitialPose()
        {
            if (!_cached || _visualRoot == null) return;
            _visualRoot.localPosition = _initLocalPos;
            _visualRoot.localRotation = _initLocalRot;
            _visualRoot.localScale = _initLocalScale;
        }
    }
}