using UnityEngine;

[CreateAssetMenu(menuName = "VFX/Trail VFX Settings", fileName = "TrailVfxSettings")]
public class TrailVfxSettings : ScriptableObject
{
    [Header("Length (world units) via time = length / speed")]
    [Min(0.01f)] public float desiredLength = 4.5f; // длиннее
    [Min(0.01f)] public float minTime = 0.05f;
    [Min(0.01f)] public float maxTime = 0.18f;

    [Header("Geometry")]
    [Min(0.001f)] public float width = 0.018f; // тоньше
    [Min(0.001f)] public float minVertexDistance = 0.035f;

    [Header("Curves & Colors")]
    public AnimationCurve widthCurve = new AnimationCurve(
        new Keyframe(0.00f, 1.00f, 0, 0),
        new Keyframe(0.15f, 0.85f, 0, 0),
        new Keyframe(0.60f, 0.28f, 0, 0),
        new Keyframe(1.00f, 0.00f, 0, 0)
    );
    public Gradient colorGradient = DefaultGradient();

    [Header("Misc")]
    [Tooltip("Добавка к ожиданию затухания при отстыковке")]
    public float fadePadding = 0.02f;

    static Gradient DefaultGradient()
    {
        var g = new Gradient();
        g.SetKeys(
            new[] {
                new GradientColorKey(new Color(1f, 0.85f, 0.55f), 0.05f),
                new GradientColorKey(new Color(1f, 0.70f, 0.30f), 0.40f),
                new GradientColorKey(new Color(1f, 0.55f, 0.20f), 1.00f),
            },
            new[] {
                new GradientAlphaKey(0.00f, 0.00f),
                new GradientAlphaKey(1.00f, 0.05f),
                new GradientAlphaKey(0.25f, 0.70f),
                new GradientAlphaKey(0.00f, 1.00f),
            }
        );
        return g;
    }
}
