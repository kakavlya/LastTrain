using System.Collections;
using UnityEngine;

namespace LastTrain.Projectiles.Effects
{
    public sealed class TrailHandler : MonoBehaviour
    {
        private const float _defaultTrailLength = 3f;
        private const float _defaultMinTime = 0.03f;
        private const float _defaultMaxTime = 0.25f;
        private const float _minProjectileSpeed = 0.001f;
        private const float _defaultFadePadding = 0.02f;
        private const float _minWaitTime = 0.01f;

        [SerializeField] private TrailRenderer _trail;
        [SerializeField] private TrailVfxSettings _settings;

        private Transform _homeParent;
        private Vector3 _initLocalPos;
        private Quaternion _initLocalRot;
        private Vector3 _initLocalScale;
        private Coroutine _fadeCo;
        private bool _fading;

        private void Reset()
        {
            _trail = GetComponent<TrailRenderer>();
        }

        private void Awake()
        {
            if (!_trail)
                _trail = GetComponent<TrailRenderer>();

            _homeParent = transform.parent;
            _initLocalPos = transform.localPosition;
            _initLocalRot = transform.localRotation;
            _initLocalScale = transform.localScale;
            ApplyStaticSettings();
            _trail.emitting = false;
            _trail.Clear();
        }

        private void OnDisable()
        {
            if (_fading)
                return;

            _trail?.Clear();

            if (_trail)
                _trail.emitting = false;
        }

        public void Play(float projectileSpeed)
        {
            if (_fadeCo != null)
            {
                StopCoroutine(_fadeCo);
                _fadeCo = null;
            }

            _fading = false;

            if (_homeParent)
                transform.SetParent(_homeParent, false);

            transform.localPosition = _initLocalPos;
            transform.localRotation = _initLocalRot;
            transform.localScale = _initLocalScale;
            gameObject.SetActive(true);
            float l = _settings ? _settings.DesiredLength : _defaultTrailLength;
            float tMin = _settings ? _settings.MinTime : _defaultMinTime;
            float tMax = _settings ? _settings.MaxTime : _defaultMaxTime;
            float speed = Mathf.Max(_minProjectileSpeed, projectileSpeed);
            _trail.time = Mathf.Clamp(l / speed, tMin, tMax);
            _trail.Clear();
            _trail.emitting = true;
        }

        public void BeginDetachFade()
        {
            if (!_trail || _fading)
                return;

            _fading = true;
            transform.SetParent(null, true);
            _trail.emitting = false;

            if (_fadeCo != null)
                StopCoroutine(_fadeCo);

            _fadeCo = StartCoroutine(FadeAndReturn());
        }

        private IEnumerator FadeAndReturn()
        {
            float pad = _settings ? _settings.FadePadding : _defaultFadePadding;
            float wait = Mathf.Max(_minWaitTime, _trail.time) + pad;
            yield return new WaitForSeconds(wait);

            if (!_trail)
                yield break;

            _trail.Clear();
            gameObject.SetActive(false);
            transform.SetParent(_homeParent, true);
            _fading = false;
            _fadeCo = null;
        }

        private void ApplyStaticSettings()
        {
            if (_settings == null)
                return;

            _trail.widthMultiplier = _settings.Width;
            _trail.widthCurve = _settings.WidthCurve;
            _trail.colorGradient = _settings.ColorGradient;
            _trail.minVertexDistance = _settings.MinVertexDistance;
        }
    }
}