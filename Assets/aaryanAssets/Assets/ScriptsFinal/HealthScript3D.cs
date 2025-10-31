using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class HealthScript3D : MonoBehaviour
{
    [Header("Health")]
    public int maxHealth = 5;
    public int CurrentHealth { get; private set; }
    public UnityEvent OnDied;

    [Header("UI")]
    public HealthUI healthUI;

    [Header("Visual")]
    [SerializeField] private Renderer playerRenderer;
    [SerializeField] private Color flashColor = Color.red;
    [SerializeField] private float flashTime = 0.2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip playerHitSound;

    [Header("Respawn")]
    public Vector3 respawnPosition = Vector3.zero;
    public float respawnDelay = 1f;

    private bool isDead;
    private Color originalColor;
    private Vector3 startPosition;
    private Rigidbody playerRigidbody;
    private MonoBehaviour playerController;

    void Awake()
    {
        Debug.Log("=== HEALTH SCRIPT AWAKE ===");
        
        // Get components
        if (!playerRenderer) playerRenderer = GetComponentInChildren<Renderer>();
        playerRigidbody = GetComponent<Rigidbody>();
        playerController = GetComponent<MonoBehaviour>(); // Get any controller script

        if (playerRenderer) originalColor = playerRenderer.material.color;

        // Remember start position
        startPosition = transform.position;
        if (respawnPosition == Vector3.zero)
            respawnPosition = startPosition;

        CurrentHealth = maxHealth;
        if (healthUI) healthUI.SetMaxHearts(maxHealth);

        Debug.Log($"Start position: {startPosition}");
        Debug.Log($"Respawn position: {respawnPosition}");
        Debug.Log($"Current position: {transform.position}");
    }

    private void OnTriggerEnter(Collider other)
    {
        TrapScript trap = other.GetComponent<TrapScript>();
        if (trap && trap.damage > 0)
        {
            Debug.Log($"Hit by trap! Damage: {trap.damage}, Health: {CurrentHealth}");
            TakeDamage(trap.damage);
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) 
        {
            Debug.Log("TakeDamage called but already dead!");
            return;
        }

        CurrentHealth = Mathf.Max(0, CurrentHealth - Mathf.Max(0, damage));
        Debug.Log($"Took {damage} damage. Health now: {CurrentHealth}");
        
        if (healthUI) healthUI.UpdateHearts(CurrentHealth);

        PlayHitSound();
        StartCoroutine(FlashRed());

        if (CurrentHealth <= 0)
        {
            Debug.Log("Health reached 0! Starting death sequence...");
            StartCoroutine(DieAndRespawn());
        }
    }

    private IEnumerator DieAndRespawn()
    {
        if (isDead) 
        {
            Debug.Log("DieAndRespawn called but already dead!");
            yield break;
        }
        
        isDead = true;
        Debug.Log("=== PLAYER DIED ===");
        
        // Disable player control and physics during respawn
        DisablePlayerControl();
        
        OnDied?.Invoke();
        
        Debug.Log($"BEFORE RESPAWN DELAY - Current position: {transform.position}");
        Debug.Log($"Respawn position set to: {respawnPosition}");
        
        // Wait for respawn delay (with reduced logging)
        float timer = 0f;
        while (timer < respawnDelay)
        {
            timer += Time.deltaTime;
            // Only log every 0.2 seconds to reduce spam
            if (Mathf.Approximately(timer % 0.2f, 0f) || timer >= respawnDelay)
            {
                Debug.Log($"Respawn delay: {timer:F2}/{respawnDelay} - Position: {transform.position}");
            }
            yield return null;
        }
        
        Debug.Log($"AFTER RESPAWN DELAY - Current position: {transform.position}");
        
        // Respawn player
        Respawn();
    }

    private void DisablePlayerControl()
    {
        // Stop all movement
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            playerRigidbody.isKinematic = true; // Disable physics
        }
        
        // Disable player controller
        if (playerController != null)
        {
            playerController.enabled = false;
        }
    }

    private void EnablePlayerControl()
    {
        // Re-enable physics
        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false;
        }
        
        // Re-enable player controller
        if (playerController != null)
        {
            playerController.enabled = true;
        }
    }

    public void Respawn()
    {
        Debug.Log("=== RESPAWNING PLAYER ===");
        Debug.Log($"Moving from: {transform.position}");
        Debug.Log($"Moving to: {respawnPosition}");
        
        // Reset position to respawn position
        transform.position = respawnPosition;
        Debug.Log($"Position after move: {transform.position}");
        
        // Make sure velocity is zero
        if (playerRigidbody != null)
        {
            playerRigidbody.linearVelocity = Vector3.zero;
            playerRigidbody.angularVelocity = Vector3.zero;
            Debug.Log($"Velocity after reset: {playerRigidbody.linearVelocity}");
        }
        
        // Reset health
        CurrentHealth = maxHealth;
        if (healthUI) 
        {
            healthUI.UpdateHearts(CurrentHealth);
            Debug.Log($"Health reset to: {CurrentHealth}");
        }
        
        // Re-enable player control
        EnablePlayerControl();
        
        // Reset death state
        isDead = false;
        
        Debug.Log($"=== RESPAWN COMPLETE ===");
        Debug.Log($"Final position: {transform.position}");
        Debug.Log($"Final health: {CurrentHealth}");
    }

    private void PlayHitSound()
    {
        if (audioSource != null && playerHitSound != null)
        {
            audioSource.PlayOneShot(playerHitSound);
            Debug.Log("Played hit sound");
        }
        else
        {
            Debug.Log("Hit sound not available");
        }
    }

    private IEnumerator FlashRed()
    {
        if (playerRenderer)
        {
            playerRenderer.material.color = flashColor;
            yield return new WaitForSeconds(flashTime);
            playerRenderer.material.color = originalColor;
            Debug.Log("Flash red completed");
        }
    }

    void Update()
    {
        // Reduced logging frequency to improve performance
        if (Time.frameCount % 180 == 0) // Every ~3 seconds at 60fps
        {
            Debug.Log($"Frame {Time.frameCount} - Position: {transform.position}, Dead: {isDead}");
        }
    }
}