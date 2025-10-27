
using System.Collections;

using UnityEngine;

using UnityEngine.Events;


public class PlayerHealthScript : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
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
        ApplyDifficultySettings();
        CurrentHealth = maxHealth;
        if (healthUI) healthUI.SetMaxHearts(maxHealth);
    }

    void ApplyDifficultySettings()
    {
        if (DifficultyManager.Instance != null)
        {
            maxHealth = DifficultyManager.Instance.GetPlayerMaxHealth();
            Debug.Log($"PlayerHealthScript: Max health set to {maxHealth} based on difficulty");
        }
        else
        {
            Debug.LogWarning("PlayerHealthScript: DifficultyManager not found, using default maxHealth");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }
        TrapScript trap = collision.GetComponent<TrapScript>();
        if (trap && trap.damage > 0)
        {
            TakeDamage(trap.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Max(0, damage));
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
