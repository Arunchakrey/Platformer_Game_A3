using UnityEngine;
using UnityEngine.Events;
using TMPro;

/// <summary>
/// Build site where players deliver logs to repair the house.
/// Tracks progress and notifies Level4Manager when complete.
/// </summary>
public class BuildSite : MonoBehaviour
{
    [Header("Build Requirements")]
    [Tooltip("Total logs needed to complete the build")]
    [SerializeField] private int totalLogsRequired = 10;

    [Tooltip("Current number of logs delivered")]
    private int logsDelivered = 0;

    [Header("Visual Feedback")]
    [Tooltip("UI text to display build progress")]
    [SerializeField] private TextMeshProUGUI progressText;

    [Tooltip("SpriteRenderer for the house - changes appearance as logs are delivered")]
    [SerializeField] private SpriteRenderer houseRenderer;

    [Tooltip("Sprites showing different build stages (0%, 25%, 50%, 75%, 100%)")]
    [SerializeField] private Sprite[] buildStageSprites;

    [Tooltip("Particle effect when logs are delivered")]
    [SerializeField] private ParticleSystem deliveryParticles;

    [Header("Audio")]
    [SerializeField] private AudioClip deliverySound;
    [SerializeField] private AudioClip completionSound;

    [Header("Events")]
    [Tooltip("Event fired when build is completed")]
    public UnityEvent OnBuildComplete;

    private bool isComplete = false;

    void Start()
    {
        UpdateUI();
        UpdateVisual();
    }

    /// <summary>
    /// Receive logs from the player and update progress.
    /// </summary>
    public void ReceiveLogs(int logCount)
    {
        if (isComplete)
        {
            Debug.Log("[BuildSite] Build already complete!");
            return;
        }

        logsDelivered += logCount;

        // Cap at maximum required
        if (logsDelivered > totalLogsRequired)
        {
            logsDelivered = totalLogsRequired;
        }

        Debug.Log($"[BuildSite] Received {logCount} logs. Progress: {logsDelivered}/{totalLogsRequired}");

        // Update visuals and UI
        UpdateUI();
        UpdateVisual();
        PlayDeliveryEffects();

        // Check if build is complete
        if (logsDelivered >= totalLogsRequired && !isComplete)
        {
            CompleteBuild();
        }
    }

    private void CompleteBuild()
    {
        isComplete = true;

        Debug.Log("[BuildSite] Build complete! House repaired!");

        // Play completion sound
        if (completionSound != null && SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Instance.PlaySound(completionSound);
        }

        // Fire completion event
        OnBuildComplete?.Invoke();

        // Notify Level4Manager
        Level4Manager manager = FindAnyObjectByType<Level4Manager>();
        if (manager != null)
        {
            manager.OnBuildingComplete();
        }
    }

    private void UpdateUI()
    {
        if (progressText != null)
        {
            float percentage = (float)logsDelivered / totalLogsRequired * 100f;
            progressText.text = $"Build Progress: {logsDelivered}/{totalLogsRequired} ({percentage:F0}%)";

            // Change color based on progress
            if (isComplete)
            {
                progressText.color = Color.green;
            }
            else if (percentage >= 75f)
            {
                progressText.color = Color.yellow;
            }
            else
            {
                progressText.color = Color.white;
            }
        }
    }

    private void UpdateVisual()
    {
        if (houseRenderer != null && buildStageSprites != null && buildStageSprites.Length > 0)
        {
            // Calculate which stage sprite to show based on progress
            float progress = (float)logsDelivered / totalLogsRequired;
            int stageIndex = Mathf.FloorToInt(progress * (buildStageSprites.Length - 1));
            stageIndex = Mathf.Clamp(stageIndex, 0, buildStageSprites.Length - 1);

            if (buildStageSprites[stageIndex] != null)
            {
                houseRenderer.sprite = buildStageSprites[stageIndex];
            }
        }
    }

    private void PlayDeliveryEffects()
    {
        // Play delivery sound
        if (deliverySound != null && SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Instance.PlaySound(deliverySound);
        }

        // Play delivery particles
        if (deliveryParticles != null)
        {
            deliveryParticles.Play();
        }
    }

    /// <summary>
    /// Get current progress percentage.
    /// </summary>
    public float GetProgress()
    {
        return (float)logsDelivered / totalLogsRequired;
    }

    /// <summary>
    /// Check if build is complete.
    /// </summary>
    public bool IsComplete()
    {
        return isComplete;
    }

    /// <summary>
    /// Get logs still needed.
    /// </summary>
    public int GetLogsRemaining()
    {
        return Mathf.Max(0, totalLogsRequired - logsDelivered);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Visual feedback when player enters build site area
        if (other.CompareTag("Player"))
        {
            Debug.Log("[BuildSite] Player entered build site area.");
        }
    }

    void OnDrawGizmos()
    {
        // Visualize build site trigger area
        Gizmos.color = isComplete ? Color.green : Color.yellow;
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
