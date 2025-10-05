using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrapScript : MonoBehaviour
{
    public int damage = 1;
    public Vector2 knockback = new Vector2(6f, 4f); // push & pop

    void Reset()
    {
        // Ensure this collider is a trigger so OnTriggerEnter2D fires
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Damage (if player has a Health component)
        var health = other.GetComponent<Health>();
        if (health != null) health.Damage(damage);

        // 2) Knockback away from trap
        var rb = other.attachedRigidbody;
        if (rb != null)
        {
            rb.velocity = new Vector2(0f, rb.velocity.y);
            float dir = Mathf.Sign(other.transform.position.x - transform.position.x);
            if (dir == 0) dir = 1f;
            rb.AddForce(new Vector2(knockback.x * dir, knockback.y), ForceMode2D.Impulse);
        }
    }
}
