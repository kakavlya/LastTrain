using System.Collections;
using UnityEngine;

public sealed class TrailHandler : MonoBehaviour
{
    [SerializeField] private TrailRenderer _trail;
    [SerializeField] private TrailVfxSettings _settings;

    Transform _homeParent;
    Vector3 _initLocalPos; Quaternion _initLocalRot; Vector3 _initLocalScale;
    Coroutine _fadeCo; bool _fading;

    void Reset() { _trail = GetComponent<TrailRenderer>(); }

    void Awake()
    {
        if (!_trail) _trail = GetComponent<TrailRenderer>();
        _homeParent = transform.parent;

        _initLocalPos = transform.localPosition;
        _initLocalRot = transform.localRotation;
        _initLocalScale = transform.localScale;

        ApplyStaticSettings();
        _trail.emitting = false;
        _trail.Clear();
        // Check Autodestruct is unselected !
    }

    void ApplyStaticSettings()
    {
        if (_settings == null) return;
        _trail.widthMultiplier = _settings.width;               // тоньше/толще
        _trail.widthCurve = _settings.widthCurve;          // форма сужения
        _trail.colorGradient = _settings.colorGradient;       // градиент
        _trail.minVertexDistance = _settings.minVertexDistance;   // гладкость
        // Alignment = View и TextureMode = Stretch поставь в инспекторе
    }

    public void Play(float projectileSpeed)
    {
        if (_fadeCo != null) { StopCoroutine(_fadeCo); _fadeCo = null; }
        _fading = false;

        if (_homeParent) transform.SetParent(_homeParent, false);
        transform.localPosition = _initLocalPos;
        transform.localRotation = _initLocalRot;
        transform.localScale = _initLocalScale;

        gameObject.SetActive(true);
        float L = _settings ? _settings.desiredLength : 3f;
        float tMin = _settings ? _settings.minTime : 0.03f;
        float tMax = _settings ? _settings.maxTime : 0.25f;
        float speed = Mathf.Max(0.001f, projectileSpeed);

        _trail.time = Mathf.Clamp(L / speed, tMin, tMax);

        _trail.Clear();
        _trail.emitting = true;
    }

    public void BeginDetachFade()
    {
        if (!_trail || _fading) return;
        _fading = true;

        transform.SetParent(null, true); // остаётся там, где догорит
        _trail.emitting = false;

        if (_fadeCo != null) StopCoroutine(_fadeCo);
        _fadeCo = StartCoroutine(FadeAndReturn());
    }

    IEnumerator FadeAndReturn()
    {
        float pad = _settings ? _settings.fadePadding : 0.02f;
        float wait = Mathf.Max(0.01f, _trail.time) + pad;
        yield return new WaitForSeconds(wait);

        if (!_trail) yield break;

        _trail.Clear();
        gameObject.SetActive(false);
        transform.SetParent(_homeParent, true);

        _fading = false;
        _fadeCo = null;
    }

    void OnDisable()
    {
        if (_fading) return;
        _trail?.Clear();
        if (_trail) _trail.emitting = false;
    }
}
