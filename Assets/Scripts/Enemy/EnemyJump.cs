using UnityEngine;

public class EnemyJump : MonoBehaviour
{
    public float jumpForce = 4f;
    public Transform player;
    public LayerMask groundLayer;
    private Rigidbody2D rb;
    public bool isGrounded;
    public Animator anim;

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
    }
}
