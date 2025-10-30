using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHearts = 5;
    private int spawnedHealth = 3;
    private int difficultyAdjustedMaxHearts;
    public int MaxHearts => difficultyAdjustedMaxHearts;
    public int CurrentHearts { get; private set; }

    public UnityEvent<int, int> OnHealthChanged = new(); //(current, max)
    public UnityEvent OnDied = new();

    void Awake()
    {
        Debug.Log($"[Health] Awake called - checking DifficultyManager...");

        // Apply difficulty modifier to max hearts
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Difficulty currentDiff = DifficultyManager.Instance.GetDifficulty();
            float healthMultiplier = DifficultyManager.Instance.GetHealthMultiplier();
            difficultyAdjustedMaxHearts = Mathf.Max(1, Mathf.RoundToInt(maxHearts * healthMultiplier));
            Debug.Log($"[Health] DifficultyManager found! Current: {currentDiff}");
            Debug.Log($"[Health] Difficulty adjusted max hearts: {maxHearts} → {difficultyAdjustedMaxHearts} (x{healthMultiplier})");
        }
        else
        {
            Debug.LogWarning($"[Health] DifficultyManager.Instance is NULL! Using default: {maxHearts}");
            difficultyAdjustedMaxHearts = maxHearts;
        }

        CurrentHearts = Mathf.Max(1, spawnedHealth);
        Debug.Log($"[Health] Final values - MaxHearts: {MaxHearts}, CurrentHearts: {CurrentHearts}");
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
    }

    public void Damage(int amount = 1)
    {
        if (CurrentHearts <= 0) return;

        // Apply difficulty modifier to damage taken
        int modifiedAmount = amount;
        if (DifficultyManager.Instance != null)
        {
            float damageMultiplier = DifficultyManager.Instance.GetDamageTakenMultiplier();
            modifiedAmount = Mathf.Max(1, Mathf.RoundToInt(amount * damageMultiplier));
            Debug.Log($"[Health] Damage taken: {amount} → {modifiedAmount} (x{damageMultiplier})");
        }

        CurrentHearts = Mathf.Max(0, CurrentHearts - modifiedAmount);
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
        if (CurrentHearts == 0)
        {
            Debug.Log("[Health] Died -> invoking OnDied");
            OnDied?.Invoke();
        }
    }

    // Alias for TakeDamage to match common naming convention
    public void TakeDamage(int amount = 1)
    {
        Damage(amount);
    }

    public void Heal(int amount = 1)
    {
        CurrentHearts = Mathf.Min(MaxHearts, CurrentHearts + Mathf.Max(1, amount));
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
    }
}