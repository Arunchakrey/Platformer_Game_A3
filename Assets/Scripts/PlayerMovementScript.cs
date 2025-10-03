using Unity.VisualScripting;
using UnityEditor.Callbacks;
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
    private bool canSpeedBoost = true;   // tracks if boost is available
    [SerializeField] private float speedBoostDuration = 2f; // how long the boost lasts
    [SerializeField] private float speedBoostCooldown = 3f; // cooldown after boost ends
    public float speedMultiplier =1f;
    public float increaseSpeedMultiplier = 1.5f;


    [Header("Jumping")]
    [SerializeField] private float jumpForce = 12f;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.4f, 0.05f);
    public LayerMask groundLayer; //for checking if anything we are touching is ground

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    public ParticleSystem speedFX;

    public Animator anim;//consider adding serialized field

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
        if (!body) body = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component attached to the player
        if (!anim) anim = GetComponent<Animator>();
        body.freezeRotation = true;
    }

    private void Update()
    {
        //update visuals
        if (horizontalMovement > 0.01f)
        {
            transform.localScale = new Vector3(3, 3, 3); // Face right
            speedFX.transform.localScale = new Vector3(1,1,1);
        }
        else if (horizontalMovement < -0.01f)
        {
            transform.localScale = new Vector3(-3, 3, 3); // Face left
            speedFX.transform.localScale = new Vector3(-1,1,1);
        }

        //Set animator parameters
        anim.SetFloat("yVelocity", body.linearVelocityY);
        anim.SetFloat("magnitude", body.linearVelocity.magnitude);
        isGrounded();
    }

    private void FixedUpdate()
    {
        //Physics goes to fixed updated
        body.linearVelocity = new Vector2(horizontalMovement * speed * speedMultiplier, body.linearVelocity.y); // Set the horizontal velocity based on player input
        Gravity();
    }

    public void SpeedBoost(InputAction.CallbackContext context)
    {
        if (context.started && canSpeedBoost)
        {
            StartCoroutine(SpeedBoostCoroutine(increaseSpeedMultiplier));
        }
    }

    // void StartSpeedBoost(float multiplier)
    // {
    //     StartCoroutine(SpeedBoostCoroutine(mulitplier));
    // }

    private IEnumerator SpeedBoostCoroutine(float multiplier)
{
    canSpeedBoost = false; // lock ability
    speedMultiplier = multiplier;
    speedFX.Play();
    // boost duration
    yield return new WaitForSeconds(speedBoostDuration);
    speedFX.Stop();
    // reset speed
    speedMultiplier = 1f;
    // cooldown before can boost again
    yield return new WaitForSeconds(speedBoostCooldown);
    canSpeedBoost = true; // unlock ability
}

    public void Move(InputAction.CallbackContext context)
    {
        // returns either 1 or -1 depending on key stroke
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded())
        {
            if (context.started)
            {
                Debug.Log("jump!");
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
                anim.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                //cuts jump short if player release jump key early
                if (body.linearVelocity.y > 0f)
                {
                    body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce * 0.5f);
                    anim.SetTrigger("jump");
                }
            }
        }
    }

    private bool isGrounded() //Check if grounded
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            return true;
        }
        return false;
    }
    private void OnDrawGizmosSelected() //draw the ground checker
    {
        if (groundCheckPos == null) return;
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

}
