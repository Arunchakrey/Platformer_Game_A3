using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    private Rigidbody2D body;
    [SerializeField] private float speed;
    private Animator anim;
    void Start()
    {

    }

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        anim = GetComponent<Animator>();
        body.freezeRotation = true;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y); // Set the horizontal velocity based on player input

        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Jump");
            body.linearVelocity = new Vector2(body.linearVelocity.x, speed); // Set the vertical velocity to make the player jump
        }

        if (horizontalInput > 0.01f)
        {
            transform.localScale = new Vector3(3, 3, 3); // Face right
        }
        else if (horizontalInput < -0.01f)
        {
            transform.localScale = new Vector3(-3, 3, 3); // Face left
        }

        //Set animator parameters
        anim.SetBool("run", horizontalInput != 0);
        
    }

}
