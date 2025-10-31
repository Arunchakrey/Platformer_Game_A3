using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class PlayerHealthScript : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    private int difficultyAdjustedMaxHealth;
    public int CurrentHealth { get; private set; }
    public UnityEvent OnDied;

    [Header("UI (optional)")]
    public HealthUI healthUI;

    [Header("Visual")]
    [SerializeField] private SpriteRenderer spriteRenderer; // auto-filled if null
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashTime = 0.2f;

    private bool isDead;

    void Awake()
    {
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);

        Debug.Log($"[PlayerHealthScript] Awake called - checking DifficultyManager...");

        // Apply difficulty modifier to max health
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Difficulty currentDiff = DifficultyManager.Instance.GetDifficulty();
            float healthMultiplier = DifficultyManager.Instance.GetHealthMultiplier();
            difficultyAdjustedMaxHealth = Mathf.Max(1, Mathf.RoundToInt(maxHealth * healthMultiplier));
            Debug.Log($"[PlayerHealthScript] DifficultyManager found! Current: {currentDiff}");
            Debug.Log($"[PlayerHealthScript] Difficulty adjusted max health: {maxHealth} → {difficultyAdjustedMaxHealth} (x{healthMultiplier})");
        }
        else
        {
            Debug.LogWarning($"[PlayerHealthScript] DifficultyManager.Instance is NULL! Using default: {maxHealth}");
            difficultyAdjustedMaxHealth = maxHealth;
        }

        CurrentHealth = difficultyAdjustedMaxHealth;
        Debug.Log($"[PlayerHealthScript] Final - MaxHealth: {difficultyAdjustedMaxHealth}, CurrentHealth: {CurrentHealth}");
        if (healthUI) healthUI.SetMaxHearts(difficultyAdjustedMaxHealth);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
        // Note: TrapScript handles its own damage application directly
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // Apply difficulty modifier to damage taken
        int modifiedDamage = damage;
        if (DifficultyManager.Instance != null)
        {
            float damageMultiplier = DifficultyManager.Instance.GetDamageTakenMultiplier();
            modifiedDamage = Mathf.Max(1, Mathf.RoundToInt(damage * damageMultiplier));
            Debug.Log($"[PlayerHealthScript] Damage taken: {damage} → {modifiedDamage} (x{damageMultiplier})");
        }

        CurrentHealth = Mathf.Max(0, CurrentHealth - modifiedDamage);
        if (healthUI) healthUI.UpdateHearts(CurrentHealth);

        StartCoroutine(FlashRed());

        if (CurrentHealth <= 0)
            Die();
    }

    private IEnumerator FlashRed()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.2f);
        spriteRenderer.color = Color.white;
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        OnDied?.Invoke();
    }
}
