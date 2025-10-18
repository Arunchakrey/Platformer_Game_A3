using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class FallingBrick : MonoBehaviour
{
    [Header("Drop Trigger")]
    public Collider2D sensor;            // a child trigger that detects the player underneath
    public string playerTag = "Player";
    public float dropDelay = 0.05f;      // slight delay before falling (feels better)

    [Header("Damage & Knockback")]
    public int damage = 1;
    public Vector2 knockback = new Vector2(6f, 4f);  // x pushes away, y pops up

    [Header("Reset")]
    public bool resetAfterFall = true;
    public float resetAfterSeconds = 2.0f;

    Rigidbody2D rb;
    Vector3 startPos;
    Quaternion startRot;
    bool hasDropped;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = transform.position;
        startRot = transform.rotation;

        // Brick should start "parked" until we trigger it to drop.
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;

        if (sensor == null)
            Debug.LogWarning($"{name}: Assign a child trigger collider as 'sensor' on FallingBrick.");
    }

    void OnEnable()
    {
        if (sensor) sensor.isTrigger = true;
        // Subscribe to the sensor�s trigger events via messages
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // If the *brick* collider is set to trigger, ignore.
        // The drop is handled by the child sensor via this safe path:
        if (sensor != null && other == sensor) return;
    }

    // Attach this to the SENSOR child via the built-in Unity message relay:
    void OnDrawGizmosSelected()
    {
        if (sensor)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.25f);
            Gizmos.DrawCube(sensor.bounds.center, sensor.bounds.size);
        }
    }

    // Put this on the sensor child using a small helper
    public void SensorEnter(Collider2D other)
    {
        if (hasDropped || !other.CompareTag(playerTag)) return;
        hasDropped = true;
        Invoke(nameof(BeginFall), dropDelay);
    }

    void BeginFall()
    {
        rb.bodyType = RigidbodyType2D.Dynamic; // let gravity pull it down

        if (resetAfterFall)
            Invoke(nameof(ResetBrick), resetAfterSeconds);
    }

    void ResetBrick()
    {
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        transform.SetPositionAndRotation(startPos, startRot);
        hasDropped = false;
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (!col.collider.CompareTag(playerTag)) return;

        // Damage (if your player has a Health component)
        var health = col.collider.GetComponent<Health>();
        if (health) health.Damage(damage);

        // Knockback the player away from the brick
        var prb = col.rigidbody;
        if (prb)
        {
            float dir = Mathf.Sign(col.transform.position.x - transform.position.x);
            if (dir == 0) dir = 1f;
            prb.linearVelocity = new Vector2(0f, prb.linearVelocity.y);
            prb.AddForce(new Vector2(knockback.x * dir, knockback.y), ForceMode2D.Impulse);
        }
    }
}
