
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

    [Header("Camera Shake")]
    [SerializeField] private CameraShake cameraShake; 
    [SerializeField] private float hitAmplitude = 1.4f;
    [SerializeField] private float hitFrequency = 8f;
    [SerializeField] private float hitDuration = 0.18f;
    [SerializeField] private float deathAmplitude = 2.5f;
    [SerializeField] private float deathFrequency = 6f;
    [SerializeField] private float deathDuration = 0.35f;

    void Awake()
    {
        if (!spriteRenderer) spriteRenderer = GetComponentInChildren<SpriteRenderer>(true);
        CurrentHealth = maxHealth;
        if (healthUI) healthUI.SetMaxHearts(maxHealth);
        if (!cameraShake) cameraShake = FindObjectOfType<CameraShake>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy)
        {
            TakeDamage(enemy.damage);
            SoundEffectManager.Play("PlayerHit");
        }
        TrapScript trap = collision.GetComponent<TrapScript>();
        if (trap && trap.damage > 0)
        {
            TakeDamage(trap.damage);
            SoundEffectManager.Play("PlayerHit");
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Max(0, damage));
        if (healthUI) healthUI.UpdateHearts(CurrentHealth);

        StartCoroutine(FlashRed());

        if (cameraShake)
        {
            cameraShake.Shake(hitAmplitude, hitFrequency, hitDuration);
        }

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

        if (cameraShake)
        {
            cameraShake.Shake(deathAmplitude, deathFrequency, deathDuration);
        }

        OnDied?.Invoke();
    }
}
