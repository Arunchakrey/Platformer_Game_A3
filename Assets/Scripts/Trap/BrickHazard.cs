using UnityEngine;

/// <summary>
/// Falling brick hazard that damages and knocks back the player on impact.
/// Can fall from spawn points at intervals or drop when triggered.
/// </summary>
public class BrickHazard : MonoBehaviour
{
    [Header("Damage Settings")]
    [Tooltip("Damage dealt to player on impact")]
    [SerializeField] private int damage = 1;

    [Header("Knockback Settings")]
    [Tooltip("Horizontal knockback force applied to player")]
    [SerializeField] private float knockbackForceX = 10f;

    [Tooltip("Vertical knockback force applied to player")]
    [SerializeField] private float knockbackForceY = 5f;

    [Header("Physics")]
    [Tooltip("Gravity scale for falling brick")]
    [SerializeField] private float gravityScale = 3f;

    [Tooltip("How long brick stays on ground before disappearing")]
    [SerializeField] private float lifetimeOnGround = 3f;

    [Header("Visual Feedback")]
    [SerializeField] private ParticleSystem impactParticles;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header("Audio")]
    [SerializeField] private AudioClip impactSound;
    [SerializeField] private AudioClip whooshSound;

    private Rigidbody2D rb;
    private bool hasHitGround = false;
    private bool hasHitPlayer = false;
    private float groundTimer = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.gravityScale = gravityScale;
        }

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    void Start()
    {
        // Play whoosh sound when brick starts falling
        if (whooshSound != null && SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Instance.PlaySound(whooshSound, 0.5f);
        }
    }

    void Update()
    {
        // Count down lifetime after hitting ground
        if (hasHitGround)
        {
            groundTimer += Time.deltaTime;

            if (groundTimer >= lifetimeOnGround)
            {
                DestroyBrick();
            }

            // Fade out visual as it disappears
            if (spriteRenderer != null)
            {
                float alpha = 1f - (groundTimer / lifetimeOnGround);
                Color color = spriteRenderer.color;
                color.a = alpha;
                spriteRenderer.color = color;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if hit player
        if (collision.gameObject.CompareTag("Player") && !hasHitPlayer)
        {
            hasHitPlayer = true;
            HitPlayer(collision);
        }

        // Check if hit ground
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            HitGround();
        }
    }

    private void HitPlayer(Collision2D collision)
    {
        Debug.Log($"[BrickHazard] Hit player! Dealing {damage} damage and applying knockback.");

        // Apply damage using difficulty-adjusted TrapScript logic
        PlayerHealthScript playerHealth = collision.gameObject.GetComponent<PlayerHealthScript>();
        Health waterPlayerHealth = collision.gameObject.GetComponent<Health>();

        // Apply difficulty-adjusted damage
        int adjustedDamage = damage;
        if (DifficultyManager.Instance != null)
        {
            float hazardMultiplier = DifficultyManager.Instance.GetHazardDamageMultiplier();
            adjustedDamage = Mathf.Max(1, Mathf.RoundToInt(damage * hazardMultiplier));
        }

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(adjustedDamage);
        }
        else if (waterPlayerHealth != null)
        {
            waterPlayerHealth.TakeDamage(adjustedDamage);
        }

        // Apply knockback
        ApplyKnockback(collision);

        // Visual and audio feedback
        PlayImpactEffects();
    }

    private void ApplyKnockback(Collision2D collision)
    {
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();

        if (playerRb != null)
        {
            // Determine knockback direction based on brick position relative to player
            Vector2 knockbackDirection = (collision.transform.position - transform.position).normalized;
            knockbackDirection.y = 1f; // Always knock upward

            Vector2 knockbackForce = new Vector2(
                knockbackDirection.x * knockbackForceX,
                knockbackForceY
            );

            // Apply the knockback force
            playerRb.linearVelocity = Vector2.zero; // Reset velocity first
            playerRb.AddForce(knockbackForce, ForceMode2D.Impulse);

            Debug.Log($"[BrickHazard] Applied knockback force: {knockbackForce}");
        }
    }

    private void HitGround()
    {
        if (!hasHitGround)
        {
            hasHitGround = true;

            // Stop the brick's movement
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.gravityScale = 0f;
                rb.bodyType = RigidbodyType2D.Kinematic;
            }

            // Play impact effects
            PlayImpactEffects();

            Debug.Log("[BrickHazard] Hit ground, starting despawn timer.");
        }
    }

    private void PlayImpactEffects()
    {
        // Play impact sound
        if (impactSound != null && SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Instance.PlaySound(impactSound);
        }

        // Play impact particles
        if (impactParticles != null)
        {
            impactParticles.Play();
        }
    }

    private void DestroyBrick()
    {
        Debug.Log("[BrickHazard] Brick despawned after lifetime.");
        Destroy(gameObject);
    }

    void OnDrawGizmos()
    {
        // Visualize knockback range
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 1f);
    }
}
