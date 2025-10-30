using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public float bounceForce = 10f;
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);
        }
    }

    private void HandlePlayerBounce(GameObject player)
    {
        var rb = player.GetComponent<Rigidbody2D>();
        var anim = player.GetComponent<Animator>();
        var health = player.GetComponent<PlayerHealthScript>();

        if (rb != null)
        {
            // stop current vertical velocity then impulse upward
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);

            if (anim) anim.SetTrigger("jump");
        }

        // Apply damage to player (with difficulty modifier)
        if (health != null && damage > 0)
        {
            int modifiedDamage = damage;
            if (DifficultyManager.Instance != null)
            {
                float hazardMultiplier = DifficultyManager.Instance.GetHazardDamageMultiplier();
                modifiedDamage = Mathf.Max(1, Mathf.RoundToInt(damage * hazardMultiplier));
                Debug.Log($"[TrapScript] Hazard damage: {damage} → {modifiedDamage} (x{hazardMultiplier})");
            }
            health.TakeDamage(modifiedDamage);
        }
    }
}
