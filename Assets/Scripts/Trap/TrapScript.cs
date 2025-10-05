using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrapScript : MonoBehaviour
{
    [Header("Effects")]
    public int damage = 1;
    public Vector2 knockback = new Vector2(6f, 4f); // x = horizontal push, y = upward pop

    void Reset()
    {
        // Make sure the collider is a trigger
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Damage (if your project has a Health component)
        var health = other.GetComponent<Health>();
        if (health != null) health.Damage(damage);

        // 2) Knockback
        Rigidbody2D rb = other.attachedRigidbody;
        if (rb != null)
        {
            // Zero horizontal speed, keep current vertical
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);

            // Push player away from trap's center
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);
            // If player is exactly centered, default to pushing right
            if (dir == 0) dir = 1f;

            Vector2 impulse = new Vector2(knockback.x * dir, knockback.y);
            rb.AddForce(impulse, ForceMode2D.Impulse);
        }
    }
}
