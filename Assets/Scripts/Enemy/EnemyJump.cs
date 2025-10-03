using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public float jumpForce = 8f;
    public Transform player;
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
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        float direction = Mathf.Sign(player.position.x - transform.position.x);

        jumpTimer -= Time.deltaTime;

        if (isGrounded && jumpTimer < 0)
        {
            //chase player
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
            jumpTimer = jumpCooldown;
        }

        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(2.5f, 2.5f, 1);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(-2.5f, 2.5f, 1);
        }
        
        //set animation parameter
        anim.SetFloat("yVelocity", rb.linearVelocityY); // dampTime=0.1f feels smooth
    }
}
