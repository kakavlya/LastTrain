using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Assets.Source.Scripts.Enemies
{
    public class EnemyDeathEffect : MonoBehaviour
    {
        [SerializeField] private Transform _visualRoot;
        [SerializeField] private float _torqueAmount = 180f;
        [SerializeField] private float _upwardForce = 3f;
        [SerializeField] private float _duration = 2f;

        private Sequence _deathSequence;

        [SerializeField] private float _flyDuration = 1f;
        [SerializeField] private float _rotateDuration = 2f;
        [SerializeField] private float _disappearDelay = 0.1f;
        [SerializeField] private float _deactivateTime = 1f; 

        public void Play()
        {
            if (_visualRoot == null)
                _visualRoot = transform;

            _deathSequence?.Kill();

            Vector3 launchVelocity = Vector3.up * _upwardForce + Random.insideUnitSphere;
            Vector3 targetPosition = _visualRoot.position + launchVelocity;
            Vector3 rotationAxis = Random.onUnitSphere * _torqueAmount;

            _deathSequence = DOTween.Sequence();
            _deathSequence.Append(_visualRoot.DOMove(targetPosition, _flyDuration).SetEase(Ease.OutExpo));
            _deathSequence.Join(_visualRoot.DORotate(rotationAxis, _rotateDuration, RotateMode.FastBeyond360).SetEase(Ease.OutExpo));
            _deathSequence.AppendInterval(_disappearDelay);

            _deathSequence.InsertCallback(_deactivateTime, () =>
            {
                gameObject.SetActive(false);
            });

            _deathSequence.OnComplete(() =>
            {
                gameObject.SetActive(false); // fallback
            });
        }

        public void ResetEffect()
        {
            _deathSequence?.Kill();
            _deathSequence = null;
        }
    }

}