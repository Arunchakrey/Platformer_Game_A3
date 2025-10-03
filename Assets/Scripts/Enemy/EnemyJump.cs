using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public float jumpForce = 8f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    public bool isGrounded;
    public Animator anim;

    public float jumpCooldown = 2f;
    private float jumpTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        jumpTimer -= Time.deltaTime;

        if (isGrounded && jumpTimer < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
            jumpTimer = jumpCooldown;
        }
        
        //set animation parameter
        anim.SetFloat("yVelocity", rb.linearVelocityY); 
    }
}
