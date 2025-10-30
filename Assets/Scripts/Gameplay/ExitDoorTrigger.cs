using UnityEngine;

/// <summary>
/// Trigger component for the exit door that notifies Level4Manager when player enters.
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class ExitDoorTrigger : MonoBehaviour
{
    [Header("Settings")]
    [Tooltip("Reference to Level4Manager (auto-finds if not set)")]
    [SerializeField] private Level4Manager level4Manager;

    [Header("Visual Feedback")]
    [Tooltip("Highlight color when player is near")]
    [SerializeField] private Color highlightColor = Color.green;

    [Tooltip("Show prompt text when player is near")]
    [SerializeField] private GameObject interactionPrompt;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool playerInRange = false;

    void Start()
    {
        // Auto-find Level4Manager if not assigned
        if (level4Manager == null)
        {
            level4Manager = FindAnyObjectByType<Level4Manager>();
        }

        // Ensure trigger is set
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.isTrigger = true;
        }

        // Get sprite renderer for highlight effect
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        // Hide prompt by default
        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        // Optional: Press E to exit when in range
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerExit();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;

            // Show interaction prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }

            // Highlight door
            if (spriteRenderer != null)
            {
                spriteRenderer.color = highlightColor;
            }

            Debug.Log("[ExitDoorTrigger] Player entered exit door area.");

            // Auto-trigger exit (or wait for E key based on your preference)
            TriggerExit();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;

            // Hide interaction prompt
            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            // Remove highlight
            if (spriteRenderer != null)
            {
                spriteRenderer.color = originalColor;
            }

            Debug.Log("[ExitDoorTrigger] Player left exit door area.");
        }
    }

    private void TriggerExit()
    {
        if (level4Manager != null)
        {
            level4Manager.OnPlayerExitLevel();

            // Disable further triggers
            Collider2D col = GetComponent<Collider2D>();
            if (col != null)
            {
                col.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("[ExitDoorTrigger] Level4Manager not found!");
        }
    }

    void OnDrawGizmos()
    {
        // Visualize trigger area
        Gizmos.color = Color.green;
        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
        CircleCollider2D circleCol = GetComponent<CircleCollider2D>();

        if (boxCol != null)
        {
            Gizmos.DrawWireCube(transform.position + (Vector3)boxCol.offset, boxCol.size);
        }
        else if (circleCol != null)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)circleCol.offset, circleCol.radius);
        }
    }
}
