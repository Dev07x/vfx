using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Animator))]
public class AnimationPreviewer : MonoBehaviour
{
    public AnimationClip animationClip;
    [Range(0f, 1f)] public float normalizedTime = 0f;

    private void OnValidate()
    {
        PreviewAnimation();
    }

    private void Update()
    {
        if (!Application.isPlaying)
        {
            PreviewAnimation();
        }
    }

    private void PreviewAnimation()
    {
        if (animationClip != null)
        {
            animationClip.SampleAnimation(gameObject, animationClip.length * normalizedTime);
        }
    }
}
