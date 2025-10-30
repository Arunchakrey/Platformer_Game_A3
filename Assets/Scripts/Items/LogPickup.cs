using UnityEngine;

/// <summary>
/// Collectible log item that players can pick up and stack (max 2 logs).
/// </summary>
public class LogPickup : MonoBehaviour
{
    [Header("Log Settings")]
    [Tooltip("How close the player needs to be to pick up this log")]
    [SerializeField] private float pickupRange = 2f;

    [Header("Visual Feedback")]
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color highlightColor = Color.yellow;
    private Color originalColor;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSound;

    private bool isCollected = false;
    private Transform playerInRange;

    void Start()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    void Update()
    {
        // Check if player is in range
        CheckForPlayer();

        // Auto-pickup if player is close enough
        if (playerInRange != null && !isCollected)
        {
            TryPickup();
        }
    }

    private void CheckForPlayer()
    {
        // Find player within pickup range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRange);

        playerInRange = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player"))
            {
                playerInRange = hit.transform;

                // Highlight when player is near
                if (spriteRenderer != null && !isCollected)
                {
                    spriteRenderer.color = highlightColor;
                }
                break;
            }
        }

        // Remove highlight when player leaves
        if (playerInRange == null && spriteRenderer != null && !isCollected)
        {
            spriteRenderer.color = originalColor;
        }
    }

    private void TryPickup()
    {
        PlayerLogCarrier carrier = playerInRange.GetComponent<PlayerLogCarrier>();

        if (carrier != null && carrier.CanPickupLog())
        {
            carrier.AddLog();
            CollectLog();
        }
    }

    private void CollectLog()
    {
        isCollected = true;

        // Play pickup sound
        if (pickupSound != null && SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Instance.PlaySound(pickupSound);
        }

        // Disable visual
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = false;
        }

        // Disable collider
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }

        Debug.Log($"[LogPickup] Log collected at {transform.position}");

        // Destroy after a short delay to allow sound to play
        Destroy(gameObject, 0.5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Alternative trigger-based pickup
        if (other.CompareTag("Player") && !isCollected)
        {
            TryPickup();
        }
    }

    void OnDrawGizmos()
    {
        // Visualize pickup range in editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, pickupRange);
    }
}
