using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class EnemyHitbox : MonoBehaviour
{
    [SerializeField] private int damage = 1;
    [SerializeField] private float knockbackForce = 6f;
    [SerializeField] private Vector2 knockbackDir = new Vector2(1, 0.5f); // relative to enemy facing

    private bool _armed = true; // simple debounce if needed

    private void Reset()
    {
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_armed) return;
        if (!other.CompareTag("Player")) return;

        // Apply damage if your PlayerHealthScript is on the player root
        if (other.TryGetComponent(out PlayerHealthScript hp))
        {
            // Call your damage method name here:
            // If your method is TakeDamage(int), use that; otherwise ApplyDamage(int).
            hp.SendMessage("TakeDamage", damage, SendMessageOptions.DontRequireReceiver);
            hp.SendMessage("ApplyDamage", damage, SendMessageOptions.DontRequireReceiver);
        }

        // Optional knockback
        if (other.attachedRigidbody != null)
        {
            // Flip knockback by enemy facing
            float dir = Mathf.Sign(transform.root.localScale.x);
            Vector2 kb = new Vector2(knockbackDir.x * dir, Mathf.Abs(knockbackDir.y)).normalized * knockbackForce;

            var v = other.attachedRigidbody.linearVelocity;
            other.attachedRigidbody.linearVelocity = new Vector2(v.x, Mathf.Min(v.y, 0)); // cancel downward spike
            other.attachedRigidbody.AddForce(kb, ForceMode2D.Impulse);
        }
    }
}
