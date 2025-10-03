using UnityEngine;

public class EnemyJump : MonoBehaviour
{
<<<<<<< HEAD
    public float jumpForce = 8f;
=======
    public float jumpForce = 4f;
    public Transform player;
>>>>>>> origin/main
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    public bool isGrounded;
    public Animator anim;

<<<<<<< HEAD
    public float jumpCooldown = 2f;
    private float jumpTimer = 0f;

=======
>>>>>>> origin/main
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
<<<<<<< HEAD
        if (!anim) anim = GetComponent<Animator>();
=======
        anim = GetComponent<Animator>();
>>>>>>> origin/main
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);
<<<<<<< HEAD

        jumpTimer -= Time.deltaTime;

        if (isGrounded && jumpTimer < 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            anim.SetTrigger("Jump");
            jumpTimer = jumpCooldown;
        }
        
        //set animation parameter
        anim.SetFloat("yVelocity", rb.linearVelocityY); 
=======
        
        float direction = Mathf.Sign(player.position.x - transform.position.x);
>>>>>>> origin/main
    }
}
