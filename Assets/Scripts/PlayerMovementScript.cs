using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementScript : MonoBehaviour
{
    public Rigidbody2D body;
    [Header("Movement")]
    [SerializeField] private float speed = 8f;
    [Header("Jumping")]
    [SerializeField] private float jumpForce = 8f;

    float horizontalMovement;
    private Animator anim;
    void Start()
    {

    }

    private void Awake()
    {
        //body = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        anim = GetComponent<Animator>();
        body.freezeRotation = true;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        body.linearVelocity = new Vector2(horizontalInput * speed, body.linearVelocity.y); // Set the horizontal velocity based on player input

        // if (Input.GetKey(KeyCode.Space))
        // {
        //     Debug.Log("Jump");
        //     body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce); // Set the vertical velocity to make the player jump
        // }

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

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, jumpForce);
        }
        else if (context.canceled)
        {
            body.linearVelocity = new Vector2(body.linearVelocityX, jumpForce * 0.5f );
        }
    }

}
