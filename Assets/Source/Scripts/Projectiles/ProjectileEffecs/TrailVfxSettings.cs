using UnityEngine;

namespace LastTrain.Projectiles.Effects
{
    [CreateAssetMenu(menuName = "VFX/Trail VFX Settings", fileName = "TrailVfxSettings")]
    public class TrailVfxSettings : ScriptableObject
    {
        private const float Color1Time = 0.05f;
        private const float Color2Time = 0.40f;
        private const float Color3Time = 1.00f;

        private static readonly Color _color1 = new Color(1f, 0.85f, 0.55f);
        private static readonly Color _color2 = new Color(1f, 0.70f, 0.30f);
        private static readonly Color _color3 = new Color(1f, 0.55f, 0.20f);
        private static readonly GradientAlphaKey _alphaKey1 = new GradientAlphaKey(0.00f, 0.00f);
        private static readonly GradientAlphaKey _alphaKey2 = new GradientAlphaKey(1.00f, 0.05f);
        private static readonly GradientAlphaKey _alphaKey3 = new GradientAlphaKey(0.25f, 0.70f);
        private static readonly GradientAlphaKey _alphaKey4 = new GradientAlphaKey(0.00f, 1.00f);

        [Header("Length (world units) via time = length / speed")]
        [Min(0.01f)][SerializeField] private float _desiredLength = 4.5f;
        [Min(0.01f)][SerializeField] private float _minTime = 0.05f;
        [Min(0.01f)][SerializeField] private float _maxTime = 0.18f;

        [Header("Geometry")]
        [Min(0.001f)][SerializeField] private float _width = 0.018f;
        [Min(0.001f)][SerializeField] private float _minVertexDistance = 0.035f;

        [Header("Curves & Colors")]
        [SerializeField]
        private AnimationCurve _widthCurve = new AnimationCurve(
            new Keyframe(0.00f, 1.00f, 0, 0),
            new Keyframe(0.15f, 0.85f, 0, 0),
            new Keyframe(0.60f, 0.28f, 0, 0),
            new Keyframe(1.00f, 0.00f, 0, 0));

        [SerializeField] private Gradient _colorGradient = DefaultGradient();

        [Header("Misc")]
        [Tooltip("ƒобавка к ожиданию затухани€ при отстыковке")]
        [SerializeField] private float _fadePadding = 0.02f;

        public float DesiredLength => _desiredLength;

        public float MinTime => _minTime;

        public float MaxTime => _maxTime;

        public float Width => _width;

        public float MinVertexDistance => _minVertexDistance;

        public AnimationCurve WidthCurve => _widthCurve;

        public Gradient ColorGradient => _colorGradient;

        public float FadePadding => _fadePadding;

        private static Gradient DefaultGradient()
        {
            var g = new Gradient();
            g.SetKeys(
                new[] {
                    new GradientColorKey(_color1, Color1Time),
                    new GradientColorKey(_color2, Color2Time),
                    new GradientColorKey(_color3, Color3Time),
                },
                new[] {_alphaKey1, _alphaKey2, _alphaKey3, _alphaKey4,});

            return g;
        }
    }
}