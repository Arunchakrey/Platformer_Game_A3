using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHearts = 5;
    private int spawnedHealth = 3;
    public int MaxHearts => maxHearts;
    public int CurrentHearts { get; private set; }

    public UnityEvent<int, int> OnHealthChanged = new(); //(current, max)
    public UnityEvent OnDied = new();

    void Awake()
    {
        ApplyDifficultySettings();
        CurrentHearts = Mathf.Max(1, spawnedHealth);
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
    }

    void ApplyDifficultySettings()
    {
        if (DifficultyManager.Instance != null)
        {
            maxHearts = DifficultyManager.Instance.GetPlayerMaxHealth();
            spawnedHealth = Mathf.Max(1, maxHearts - 2); // Start with max-2 health
            Debug.Log($"Health (Water): Max hearts set to {maxHearts}, spawned with {spawnedHealth} based on difficulty");
        }
        else
        {
            Debug.LogWarning("Health (Water): DifficultyManager not found, using default maxHearts");
        }
    }

    public void Damage(int amount = 1)
    {
        if (CurrentHearts <= 0) return;
        CurrentHearts = Mathf.Max(0, CurrentHearts - Mathf.Max(1, amount));
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
        if (CurrentHearts == 0)
        {
            Debug.Log("[Health] Died -> invoking OnDied");
            OnDied?.Invoke();
        }
    }

    public void Heal(int amount = 1)
    {
        CurrentHearts = Mathf.Min(MaxHearts, CurrentHearts + Mathf.Max(1, amount));
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
    }
}