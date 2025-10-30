using UnityEngine;
using TMPro;

/// <summary>
/// Allows the player to carry logs (max 2 stacked) and deliver them to the build site.
/// </summary>
public class PlayerLogCarrier : MonoBehaviour
{
    [Header("Carrying Settings")]
    [Tooltip("Maximum number of logs player can carry")]
    [SerializeField] private int maxLogs = 2;

    [Tooltip("Current number of logs being carried")]
    private int currentLogs = 0;

    [Header("Visual Feedback")]
    [Tooltip("UI text to display log count")]
    [SerializeField] private TextMeshProUGUI logCountText;

    [Tooltip("Parent transform for visual log sprites")]
    [SerializeField] private Transform logVisualParent;

    [Tooltip("Prefab for visual log representation")]
    [SerializeField] private GameObject logVisualPrefab;

    [Tooltip("Vertical offset between stacked logs")]
    [SerializeField] private float logStackOffset = 0.5f;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupLogSound;
    [SerializeField] private AudioClip deliverLogSound;

    private GameObject[] visualLogs;

    void Start()
    {
        visualLogs = new GameObject[maxLogs];
        UpdateUI();
    }

    /// <summary>
    /// Check if player can pick up another log.
    /// </summary>
    public bool CanPickupLog()
    {
        return currentLogs < maxLogs;
    }

    /// <summary>
    /// Add a log to the player's inventory.
    /// </summary>
    public void AddLog()
    {
        if (currentLogs < maxLogs)
        {
            currentLogs++;
            UpdateUI();
            UpdateVisuals();

            // Play pickup sound
            if (pickupLogSound != null && SoundEffectManager.Instance != null)
            {
                SoundEffectManager.Instance.PlaySound(pickupLogSound);
            }

            Debug.Log($"[PlayerLogCarrier] Picked up log. Total: {currentLogs}/{maxLogs}");
        }
    }

    /// <summary>
    /// Remove logs from player's inventory when delivering.
    /// </summary>
    public int DeliverLogs()
    {
        int logsDelivered = currentLogs;

        if (logsDelivered > 0)
        {
            currentLogs = 0;
            UpdateUI();
            UpdateVisuals();

            // Play delivery sound
            if (deliverLogSound != null && SoundEffectManager.Instance != null)
            {
                SoundEffectManager.Instance.PlaySound(deliverLogSound);
            }

            Debug.Log($"[PlayerLogCarrier] Delivered {logsDelivered} logs to build site.");
        }

        return logsDelivered;
    }

    /// <summary>
    /// Get current number of logs carried.
    /// </summary>
    public int GetLogCount()
    {
        return currentLogs;
    }

    /// <summary>
    /// Check if player has any logs.
    /// </summary>
    public bool HasLogs()
    {
        return currentLogs > 0;
    }

    private void UpdateUI()
    {
        if (logCountText != null)
        {
            logCountText.text = $"Logs: {currentLogs}/{maxLogs}";
        }
    }

    private void UpdateVisuals()
    {
        // Destroy existing visual logs
        for (int i = 0; i < visualLogs.Length; i++)
        {
            if (visualLogs[i] != null)
            {
                Destroy(visualLogs[i]);
                visualLogs[i] = null;
            }
        }

        // Create new visual logs based on current count
        if (logVisualPrefab != null && logVisualParent != null)
        {
            for (int i = 0; i < currentLogs; i++)
            {
                Vector3 position = logVisualParent.position + new Vector3(0, i * logStackOffset, 0);
                GameObject logVisual = Instantiate(logVisualPrefab, position, Quaternion.identity, logVisualParent);
                visualLogs[i] = logVisual;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Auto-deliver when entering build site
        if (other.CompareTag("BuildSite") && HasLogs())
        {
            BuildSite buildSite = other.GetComponent<BuildSite>();
            if (buildSite != null)
            {
                int logsDelivered = DeliverLogs();
                buildSite.ReceiveLogs(logsDelivered);
            }
        }
    }

    void OnDrawGizmos()
    {
        // Visualize log carry position
        if (logVisualParent != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(logVisualParent.position, 0.3f);

            // Draw stack positions
            for (int i = 0; i < maxLogs; i++)
            {
                Vector3 position = logVisualParent.position + new Vector3(0, i * logStackOffset, 0);
                Gizmos.DrawWireCube(position, Vector3.one * 0.5f);
            }
        }
    }
}
