using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
public class FallingBrick : MonoBehaviour
{
    [Header("Drop Trigger")]
    public Collider2D sensor;            // a child trigger that detects the player underneath
    public string playerTag = "Player";

    [Header("Difficulty-Controlled Values (Base)")]
    [Tooltip("Base drop delay (will be overridden by difficulty settings)")]
    public float dropDelayBase = 0.5f;

    [Tooltip("Base damage amount (will be overridden by difficulty settings)")]
    public int damageBase = 1;

    [Tooltip("Base gravity scale (will be overridden by difficulty settings)")]
    public float gravityScaleBase = 5f;

    [Header("Damage & Knockback")]
    public Vector2 knockback = new Vector2(6f, 4f);  // x pushes away, y pops up

    [Header("Debug")]
    public bool debugMode = false;

    [Header("Reset")]
    public bool resetAfterFall = true;
    public float resetAfterSeconds = 2.0f;

    Rigidbody2D rb;
    Vector3 startPos;
    Quaternion startRot;
    bool hasDropped;

    // Runtime difficulty-adjusted values
    private float actualDropDelay;
    private int actualDamage;
    private float actualGravityScale;

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

        ApplyDifficultySettings();
    }

    void Start()
    {
        ApplyDifficultySettings();
    }

    void ApplyDifficultySettings()
    {
        if (DifficultyManager.Instance != null)
        {
            actualDropDelay = DifficultyManager.Instance.GetBrickDropDelay();
            actualDamage = DifficultyManager.Instance.GetDamageAmount();
            actualGravityScale = DifficultyManager.Instance.GetBrickFallSpeed();

            if (debugMode)
            {
                Debug.Log($"{name}: Applied difficulty settings - Delay: {actualDropDelay}s, Damage: {actualDamage}, Gravity: {actualGravityScale}");
            }
        }
        else
        {
            actualDropDelay = dropDelayBase;
            actualDamage = damageBase;
            actualGravityScale = gravityScaleBase;

            if (debugMode) Debug.LogWarning($"{name}: DifficultyManager not found, using base values");
        }
    }

    void OnEnable()
    {
        if (sensor) sensor.isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (sensor != null && other == sensor) return;
    }

    void OnDrawGizmosSelected()
    {
        if (sensor)
        {
            Gizmos.color = new Color(1f, 0.5f, 0f, 0.25f);
            Gizmos.DrawCube(sensor.bounds.center, sensor.bounds.size);
        }
    }

    public void SensorEnter(Collider2D other)
    {
        if (hasDropped || !other.CompareTag(playerTag)) return;

        if (debugMode)
        {
            Debug.Log($"{name}: Player detected under brick, dropping in {actualDropDelay}s");
        }

        hasDropped = true;
        Invoke(nameof(BeginFall), actualDropDelay);
    }

    void BeginFall()
    {
        if (debugMode) Debug.Log($"{name}: Beginning fall with gravity scale {actualGravityScale}");

        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = actualGravityScale;

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
        if (!hasDropped) return;

        var health = col.collider.GetComponent<Health>();
        if (health)
        {
            health.Damage(actualDamage);
            if (debugMode) Debug.Log($"{name}: Dealt {actualDamage} damage to player");
        }

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
