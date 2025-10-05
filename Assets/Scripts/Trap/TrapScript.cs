using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TrapScript : MonoBehaviour
{
<<<<<<< HEAD
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
=======
    public float bounceForce = 10f;
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D  collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);
            Debug.Log("Same Tag as expected: Player");
        }
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator anim = player.GetComponent<Animator>();
        Debug.Log("Got player Object");

        if (rb)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            //apply bounce force
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            Debug.Log("Jump!!!");
            anim.SetTrigger("jump");
            //parameter for animation


        }
    }
        
>>>>>>> 06c448357a009321dc8654cdf0205bc1a303a5dd
}
