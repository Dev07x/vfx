using UnityEngine;

public class SwordController : MonoBehaviour
{
    public SwordSlashTrail slashTrailEffect;
    public KeyCode slashKey = KeyCode.Mouse0;
    public float slashCooldown = 0.7f;

    private float cooldownTimer = 0f;
    private Animator swordAnimator; // Optional - reference to sword's animator

    void Start()
    {
        // If you have an animator component
        swordAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        // Handle cooldown timer
        if (cooldownTimer > 0)
        {
            cooldownTimer -= Time.deltaTime;
        }

        // Check for slash input when not on cooldown
        if (cooldownTimer <= 0 && Input.GetKeyDown(slashKey))
        {
            PerformSlash();
            cooldownTimer = slashCooldown;
        }
    }

    void PerformSlash()
    {
        // Trigger the slash trail effect
        if (slashTrailEffect != null)
        {
            slashTrailEffect.PerformSlash();
        }

        // If you have an animator, trigger the slash animation
        if (swordAnimator != null)
        {
            swordAnimator.SetTrigger("Slash");
        }

        // You could add other effects here (sound, particles, etc.)
        Debug.Log("Slash performed!");
    }
}