using UnityEngine;

public class EnemyWalk : MonoBehaviour
{
    public Transform player;
    public float chaseSpeed = 2f;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    public bool isGrounded;
    public Animator anim;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //is Grouded
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, 1f, groundLayer);

        //Player Direction
        float direction = Mathf.Sign(player.position.x - transform.position.x);

        //Player above detection
        bool isPlayerAbove = Physics2D.Raycast(transform.position, Vector2.up, 3f, 1 << player.gameObject.layer);

        if (isGrounded)
        {
            //chase player
            rb.linearVelocity = new Vector2(direction * chaseSpeed, rb.linearVelocityY);
        }

        if (player.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (player.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
        
        //set animation parameter
        float horizontalSpeed = Mathf.Abs(rb.linearVelocityX);
        anim.SetFloat("speed", horizontalSpeed); // dampTime=0.1f feels smooth
    }

    void FixedUpdate()
    {
        // if (isGrounded)
        // {
        //     Vector2 direction = (player.position - transform.position).normalized;
        // }
    }
}
