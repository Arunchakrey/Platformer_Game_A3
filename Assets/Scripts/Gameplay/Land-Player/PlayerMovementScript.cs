using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Component")]
    float horizontalMovement;
    public Rigidbody2D body;

    [Header("Movement")]
    [SerializeField] private float speed = 6f;

    [Header("SpeedBoost")]
    private bool canSpeedBoost = true;
    [SerializeField] private float speedBoostDuration = 2f;
    [SerializeField] private float speedBoostCooldown = 3f;
    public float speedMultiplier = 1f;
    public float increaseSpeedMultiplier = 1.5f;

    [Header("Jumping")]
    [SerializeField] private float jumpForce = 12f;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.05f);
    public LayerMask groundLayer;

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    public ParticleSystem speedFX;
    public Animator anim;

    private void Gravity()
    {
        if (body.linearVelocity.y < 0)
        {
            body.gravityScale = baseGravity * fallSpeedMultiplier;
            body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Max(body.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            body.gravityScale = baseGravity;
        }
    }

    private void Awake()
    {
        if (!body) body = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponent<Animator>();
        body.freezeRotation = true;
    }

    private void Update()
    {
        // Flip visuals
        if (horizontalMovement > 0.01f)
        {
            transform.localScale = new Vector3(3, 3, 3);
            speedFX.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (horizontalMovement < -0.01f)
        {
            transform.localScale = new Vector3(-3, 3, 3);
            speedFX.transform.localScale = new Vector3(-1, 1, 1);
        }

        // Footstep control
        bool grounded = isGrounded();
        bool moving = Mathf.Abs(horizontalMovement) > 0.1f;
        if (grounded && moving)
        {
            // Slightly slower pace for walking
            SoundEffectManager.PlayFootsteps("PlayerWalk", 0.45f, 0.65f);
        }
        else
        {
            SoundEffectManager.StopFootsteps();
        }

        // Animation updates
        anim.SetFloat("yVelocity", body.linearVelocityY);
        anim.SetFloat("magnitude", body.linearVelocity.magnitude);
        isGrounded();
    }

    private void FixedUpdate()
    {
        body.linearVelocity = new Vector2(horizontalMovement * speed * speedMultiplier, body.linearVelocity.y);
        Gravity();
    }

    public void SpeedBoost(InputAction.CallbackContext context)
    {
        if (context.started && canSpeedBoost)
        {
            StartCoroutine(SpeedBoostCoroutine(increaseSpeedMultiplier));
        }
    }

    private IEnumerator SpeedBoostCoroutine(float multiplier)
    {
        canSpeedBoost = false;
        speedMultiplier = multiplier;
        speedFX.Play();
        yield return new WaitForSeconds(speedBoostDuration);
        speedFX.Stop();
        speedMultiplier = 1f;
        yield return new WaitForSeconds(speedBoostCooldown);
        canSpeedBoost = true;
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            if (context.started)
            {
                SoundEffectManager.Play("PlayerJump");
                SoundEffectManager.StopFootsteps(); // stop footsteps when jumping
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
                anim.SetTrigger("jump");
            }
            else if (context.canceled && body.linearVelocity.y > 0f)
            {
                SoundEffectManager.Play("PlayerJump");
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce * 0.5f);
                anim.SetTrigger("jump");
            }
        }
    }

    private bool isGrounded()
    {
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheckPos == null) return;
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }
}
