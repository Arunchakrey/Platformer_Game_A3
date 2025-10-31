using UnityEngine;

public class TrapScript3D : MonoBehaviour
{
    public float bounceForce = 8f;
    public int damage = 1;

    private void OnTriggerEnter(Collider other) // Changed to 3D
    {
        if (other.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(other.gameObject);
        }
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody rb = player.GetComponent<Rigidbody>(); // Changed to 3D Rigidbody
        Animator anim = player.GetComponent<Animator>();

        if (rb)
        {
            // Reset vertical velocity and apply bounce
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

            // Apply bounce force in 3D
            rb.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
            Debug.Log("Bounce!!!");

            // Trigger jump animation if needed
            if (anim != null)
                anim.SetTrigger("jump");
        }

        // Apply damage to player
        HealthScript3D health = player.GetComponent<HealthScript3D>();
        if (health != null)
        {
            health.TakeDamage(damage);
        }
    }
}