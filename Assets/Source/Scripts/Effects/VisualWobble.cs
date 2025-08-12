using System.Collections;
using System.Xml.Linq;
using UnityEngine;

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

    // Internal
    private Vector3 initialLocalPos;
    private Quaternion initialLocalRot;

    private float swayOffset;
    private float bounceOffset;
    private float tiltOffset;

    private void Awake()
    {
        if (_visualRoot == null)
        {
            var found = transform.Find("VisualRoot");
            if (found != null) _visualRoot = found;
            else Debug.LogWarning($"{name}: EnemyVisualWobble couldn't find VisualRoot. Assign manually.");
        }

        if (_visualRoot != null)
        {
            initialLocalPos = _visualRoot.localPosition;
            initialLocalRot = _visualRoot.localRotation;
        }

        swayOffset = Random.Range(0f, 100f);
        bounceOffset = Random.Range(0f, 100f);
        tiltOffset = Random.Range(0f, 100f);
    }

    private void Update()
    {
        if (_visualRoot == null) return;

        float sway = Mathf.Sin((Time.time + swayOffset) * _swaySpeed) * _swayAmount * _massFactor;
        float bounce = Mathf.PerlinNoise(0, (Time.time + bounceOffset) * _bounceSpeed) * _bounceAmount * _massFactor;
        float tilt = Mathf.Sin((Time.time + tiltOffset) * _tiltSpeed) * _tiltAmount * _massFactor;

        _visualRoot.localPosition = initialLocalPos + (_visualRoot.right * sway) + (Vector3.up * bounce);
        _visualRoot.localRotation = initialLocalRot * Quaternion.Euler(tilt, 0f, 0f);
    }
}