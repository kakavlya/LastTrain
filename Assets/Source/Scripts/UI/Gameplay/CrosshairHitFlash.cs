using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CrosshairHitFlash : MonoBehaviour
{
    [Header("Flash color")]
    [SerializeField] private bool _useComplementary = true;          
    [SerializeField] private Color _fixedFlashColor = Color.cyan;    
    [Range(0f, 1.5f)][SerializeField] private float _valueBoost = 0.6f;     
    [Range(0f, 1f)][SerializeField] private float _saturationBoost = 0.2f; 

    [Header("Timing")]
    [SerializeField] private float _flashTime = 0.08f;
    [SerializeField] private float _fadeTime = 0.12f;
    [SerializeField] private float _minInterval = 0.03f;  // анти-спам для дробовика/дотов

    [Header("Punch (optional)")]
    [SerializeField] private bool _useScalePunch = true;
    [SerializeField] private float _punchScale = 1.12f;

    private Image _img;
    private RectTransform _transform;
    private Color _base;
    private Color _flash;
    private Coroutine _coroutine;
    private float _last;

    private void Awake()
    {
        _img = GetComponent<Image>();
        _transform = (RectTransform)transform;
        _base = _img.color;
        _flash = _useComplementary ? MakeFlashFromBase(_base) : _fixedFlashColor;
        _flash.a = 1f; 
    }

    private void OnEnable() => CombatEvents.EnemyHit += OnHit;
    private void OnDisable()
    {
        CombatEvents.EnemyHit -= OnHit;
        if (_coroutine != null) StopCoroutine(_coroutine);
        _img.color = _base;
        if (_useScalePunch) _transform.localScale = Vector3.one;
    }

    private void OnHit()
    {
        float now = Time.unscaledTime;
        if (now - _last < _minInterval) return; 
        _last = now;

        if (_coroutine != null) StopCoroutine(_coroutine);
        _coroutine = StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _img.color = _flash;

        if (_useScalePunch) _transform.localScale = Vector3.one * _punchScale;

        float t0 = Time.unscaledTime;
        while (Time.unscaledTime - t0 < _flashTime) yield return null;

        float t1 = Time.unscaledTime;
        while (Time.unscaledTime - t1 < _fadeTime)
        {
            float unscaledTime = (Time.unscaledTime - t1) / _fadeTime;
            _img.color = Color.Lerp(_flash, _base, unscaledTime);
            if (_useScalePunch) _transform.localScale = Vector3.Lerp(Vector3.one * _punchScale, Vector3.one, unscaledTime);
            yield return null;
        }

        _img.color = _base;
        if (_useScalePunch) _transform.localScale = Vector3.one;
        _coroutine = null;
    }

    private Color MakeFlashFromBase(Color baseCol)
    {
        Color.RGBToHSV(baseCol, out float h, out float s, out float v);
        h = (h + 0.5f) % 1f; 
        s = Mathf.Clamp01(s + _saturationBoost);
        v = Mathf.Clamp01(v + _valueBoost);
        return Color.HSVToRGB(h, s, v, true);
    }
}
