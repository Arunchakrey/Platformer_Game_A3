using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript3D : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; }
    public UnityEvent OnDied;

    [Header("UI (optional)")]
    public HealthUI healthUI;

    [Header("Visual")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashTime = 0.2f;

    [Header("Audio")] // ADD THIS
    public AudioSource audioSource;
    public AudioClip playerHitSound;

    private bool isDead;
    private Color originalColor;

    void Awake()
    {
        if (!playerRenderer) playerRenderer = GetComponentInChildren<Renderer>();
        if (playerRenderer) originalColor = playerRenderer.material.color;

        CurrentHealth = maxHealth;
        if (healthUI) healthUI.SetMaxHearts(maxHealth);
    }

    private void OnTriggerEnter(Collider other)
    {
        Enemy enemy = other.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
        }

        TrapScript trap = other.GetComponent<TrapScript>();
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

        // PLAY HIT SOUND WHEN DAMAGED ← ADD THIS
        PlayHitSound();

        StartCoroutine(FlashRed());

        if (CurrentHealth <= 0)
            Die();
    }

    // ADD THIS METHOD
    private void PlayHitSound()
    {
        if (audioSource != null && playerHitSound != null)
        {
            audioSource.PlayOneShot(playerHitSound);
        }
    }

    private IEnumerator FlashRed()
    {
        if (playerRenderer)
        {
            playerRenderer.material.color = flashColor;
            yield return new WaitForSeconds(flashTime);
            playerRenderer.material.color = originalColor;
        }
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;
        OnDied?.Invoke();
    }
}