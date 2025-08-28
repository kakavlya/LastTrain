using UnityEngine;

namespace LastTrain.Effects
{
    public class VisualWobble : MonoBehaviour
    {
        [Header("Target to wobble (leave empty to auto-find 'VisualRoot')")]
        [SerializeField] private Transform _visualRoot;

        [Header("Global Intensity (1 = normal, <1 = слабее, >1 = сильнее)")]
        [SerializeField] private float _massFactor = 1f;

        [Header("Sway (side-to-side)")]
        [SerializeField] private float _swayAmount = 0.2f;
        [SerializeField] private float _swaySpeed = 1.5f;

        [Header("Bounce (up-down)")]
        [SerializeField] private float _bounceAmount = 0.15f;
        [SerializeField] private float _bounceSpeed = 2.5f;

        [Header("Tilt (rotation forward-backward)")]
        [SerializeField] private float _tiltAmount = 6f;
        [SerializeField] private float _tiltSpeed = 2f;

        private Vector3 _initialLocalPos;
        private Quaternion _initialLocalRot;
        private float _swayOffset;
        private float _bounceOffset;
        private float _tiltOffset;
        private string _visualRootNaming = "VisualRoot";
        private float _maxOffset = 100f;

        private void Awake()
        {
            if (_visualRoot == null)
            {
                var found = transform.Find(_visualRootNaming);

                if (found != null)
                    _visualRoot = found;
            }

            if (_visualRoot != null)
            {
                _initialLocalPos = _visualRoot.localPosition;
                _initialLocalRot = _visualRoot.localRotation;
            }

            _swayOffset = Random.Range(0f, _maxOffset);
            _bounceOffset = Random.Range(0f, _maxOffset);
            _tiltOffset = Random.Range(0f, _maxOffset);
        }

        private void Update()
        {
            if (_visualRoot == null) return;

            float sway = Mathf.Sin((Time.time + _swayOffset) * _swaySpeed) * _swayAmount * _massFactor;
            float bounce = Mathf.PerlinNoise(0, (Time.time + _bounceOffset) * _bounceSpeed) * _bounceAmount * _massFactor;
            float tilt = Mathf.Sin((Time.time + _tiltOffset) * _tiltSpeed) * _tiltAmount * _massFactor;
            _visualRoot.localPosition = _initialLocalPos + (_visualRoot.right * sway) + (Vector3.up * bounce);
            _visualRoot.localRotation = _initialLocalRot * Quaternion.Euler(tilt, 0f, 0f);
        }
    }
}