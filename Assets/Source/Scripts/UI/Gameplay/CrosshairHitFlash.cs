using System.Collections;
using UnityEngine;

public class CrosshairHitFlash : MonoBehaviour
{
    [Header("Punch")]
    [SerializeField] private float _punchScale = 1.12f;   
    [SerializeField] private float _holdTime = 0.06f;   
    [SerializeField] private float _returnTime = 0.12f; 
    [SerializeField] private float _minInterval = 0.03f;

    private RectTransform _rt;
    private Coroutine _co;
    private float _last;

    private void Awake()
    {
        _rt = (RectTransform)transform;
    }

    private void OnEnable()
    {
        CombatEvents.EnemyHit += OnHit;
    }

    private void OnDisable()
    {
        CombatEvents.EnemyHit -= OnHit;
        if (_co != null) StopCoroutine(_co);
        _rt.localScale = Vector3.one;
    }

    private void OnHit()
    {
        float now = Time.unscaledTime;
        if (now - _last < _minInterval) return;
        _last = now;

        if (_co != null) StopCoroutine(_co);
        _co = StartCoroutine(PunchRoutine());
    }

    private IEnumerator PunchRoutine()
    {

        _rt.localScale = Vector3.one * _punchScale;

        float t = 0f;
        while (t < _holdTime) { t += Time.unscaledDeltaTime; yield return null; }

        t = 0f;
        while (t < _returnTime)
        {
            t += Time.unscaledDeltaTime;
            float u = Mathf.Clamp01(t / _returnTime);
            u = 1f - Mathf.Pow(1f - u, 3f);
            _rt.localScale = Vector3.Lerp(Vector3.one * _punchScale, Vector3.one, u);
            yield return null;
        }

        _rt.localScale = Vector3.one;
        _co = null;
    }
}
