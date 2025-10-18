using UnityEngine;

public class PlayerMovementL4 : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D body;
    float horizontalMovement;

    [Header("Movement")]
    [SerializeField] float speed = 6f;

    [Header("Jumping")]
    [SerializeField] float jumpForce = 12f;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.05f);
    public LayerMask groundLayer; // set in Inspector (Default or Ground)

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    Animator anim;

    void Awake()
    {
        if (!body) body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        body.freezeRotation = true;
        if (groundLayer.value == 0) groundLayer = Physics2D.AllLayers; // fallback
    }

    void Update()
    {
        // Fallback input (works even without PlayerInput)
        float axis = Input.GetAxisRaw("Horizontal");
        if (Mathf.Abs(axis) > 0.01f) horizontalMovement = axis;

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);

        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0f)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * 0.5f);

        // Face direction
        if (horizontalMovement > 0.01f) transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalMovement < -0.01f) transform.localScale = new Vector3(-3, 3, 3);

        // Optional animator params � only if they exist
        if (anim)
        {
            TrySetFloat(anim, "yVelocity", body.linearVelocity.y);
            TrySetFloat(anim, "magnitude", body.linearVelocity.magnitude);
        }
    }

    void FixedUpdate()
    {
        body.linearVelocity = new Vector2(horizontalMovement * speed, body.linearVelocity.y);

        // custom gravity / terminal velocity
        if (body.linearVelocity.y < 0f)
        {
            body.gravityScale = baseGravity * fallSpeedMultiplier;
            body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Max(body.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            body.gravityScale = baseGravity;
        }

        // reset so Update re-reads input each frame
        horizontalMovement = 0f;
    }

    bool IsGrounded()
    {
        if (!groundCheckPos) return false;
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);
    }

    void OnDrawGizmosSelected()
    {
        if (!groundCheckPos) return;
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

    static void TrySetFloat(Animator a, string param, float value)
    {
        foreach (var p in a.parameters)
            if (p.name == param && p.type == AnimatorControllerParameterType.Float)
            { a.SetFloat(param, value); return; }
    }
}
