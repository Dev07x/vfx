using UnityEngine;

public class SwordSlashTrail : MonoBehaviour
{
    [Header("Trail Settings")]
    public Material trailMaterial;
    public float trailTime = 0.5f;
    public float minVertexDistance = 0.1f;
    public float startWidth = 0.5f;
    public float endWidth = 0.0f;
    public Gradient trailColor;
    public bool emitting = false;

    [Header("Animation Settings")]
    public float slashDuration = 0.4f;
    public AnimationCurve slashCurve;

    // References
    private TrailRenderer trail;
    private Material instancedMaterial;
    private float activeTimer = 0f;

    private void Awake()
    {
        // Create and configure trail renderer at runtime
        trail = gameObject.AddComponent<TrailRenderer>();

        // Create an instance of the material to avoid affecting other objects
        instancedMaterial = new Material(trailMaterial);
        trail.material = instancedMaterial;

        // Basic trail settings
        trail.time = trailTime;
        trail.minVertexDistance = minVertexDistance;
        trail.startWidth = startWidth;
        trail.endWidth = endWidth;
        trail.colorGradient = trailColor;
        trail.emitting = false;
    }

    private void Update()
    {
        // Control slash emission
        if (emitting)
        {
            activeTimer += Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(activeTimer / slashDuration);

            // Apply the animation curve to control the intensity
            float curveValue = slashCurve.Evaluate(normalizedTime);
            instancedMaterial.SetFloat("_Intensity", Mathf.Lerp(1.5f, 0.0f, normalizedTime) * curveValue);

            // Enable the trail
            trail.emitting = true;

            // Stop emitting when the duration is reached
            if (activeTimer >= slashDuration)
            {
                emitting = false;
                activeTimer = 0f;
                trail.emitting = false;
            }
        }
        else
        {
            // Ensure trail is not emitting when inactive
            trail.emitting = false;
        }
    }

    // Call this method to trigger a slash effect
    public void PerformSlash()
    {
        emitting = true;
        activeTimer = 0f;
        trail.Clear(); // Clear the previous trail
    }
}