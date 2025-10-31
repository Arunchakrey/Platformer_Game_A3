using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public float jumpForce = 8f;
    // public float jumpForce = 4f;
    private float difficultyAdjustedJumpForce;
    public Transform player;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    public bool isGrounded;
    public Animator anim;

    public float jumpCooldown = 2f;
    private float difficultyAdjustedCooldown;
    private float jumpTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponent<Animator>();
        anim = GetComponent<Animator>();

        // Apply difficulty modifier to jump force and cooldown
        if (DifficultyManager.Instance != null)
        {
            float speedMultiplier = DifficultyManager.Instance.GetEnemySpeedMultiplier();
            difficultyAdjustedJumpForce = jumpForce * speedMultiplier;
            // Faster enemies jump more frequently (shorter cooldown)
            difficultyAdjustedCooldown = jumpCooldown / speedMultiplier;
            Debug.Log($"[EnemyJump] Difficulty adjusted - Force: {jumpForce} → {difficultyAdjustedJumpForce}, Cooldown: {jumpCooldown} → {difficultyAdjustedCooldown}");
        }
        else
        {
            difficultyAdjustedJumpForce = jumpForce;
            difficultyAdjustedCooldown = jumpCooldown;
        }
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        jumpTimer -= Time.deltaTime;

        if (isGrounded && jumpTimer < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, difficultyAdjustedJumpForce);
            anim.SetTrigger("Jump");
            jumpTimer = difficultyAdjustedCooldown;
        }

        //set animation parameter
        anim.SetFloat("yVelocity", rb.linearVelocityY);

        float direction = Mathf.Sign(player.position.x - transform.position.x);
    }
}
