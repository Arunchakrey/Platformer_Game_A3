using UnityEngine;

/// <summary>
/// Connects the Health component (water player) to HealthUI display.
/// </summary>
public class HealthUIConnector : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Health healthComponent;
    [SerializeField] private HealthUI healthUI;

    void Start()
    {
        // Auto-find if not assigned
        if (healthComponent == null)
        {
            healthComponent = GetComponent<Health>();
        }

        if (healthComponent != null && healthUI != null)
        {
            // Subscribe to health changes
            healthComponent.OnHealthChanged.AddListener(OnHealthChanged);

            // Initialize UI
            healthUI.SetMaxHearts(healthComponent.MaxHearts);
            healthUI.UpdateHearts(healthComponent.CurrentHearts);
        }
        else
        {
            Debug.LogWarning("[HealthUIConnector] Missing references! Assign Health component and HealthUI.");
        }
    }

    private void OnHealthChanged(int current, int max)
    {
        if (healthUI != null)
        {
            healthUI.UpdateHearts(current);
        }
    }

    void OnDestroy()
    {
        // Unsubscribe when destroyed
        if (healthComponent != null)
        {
            healthComponent.OnHealthChanged.RemoveListener(OnHealthChanged);
        }
    }
}
