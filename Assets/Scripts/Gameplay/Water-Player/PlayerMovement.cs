using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementWater : MonoBehaviour

{
    // player mechanics
    private float movementSpeed = 3.2f, jumpSpeed = 8f;
    private float accelRate = 60f, decelRate = 40f;

    // double jump
    private int extraAirJump = 1;
    private int airJumpRemaining = 0;

    // player interactions
    [SerializeField] LayerMask groundMask;

    private Rigidbody2D body;
    private Collider2D col;

    // player animation
    private Animator anim;
    private bool grounded;


    // load script once game starts
    private void Awake()
    {
        // grab references for its methods
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
    }

    // checking for user input updates
    private void Update()
    {
        // detecting keypress
        var k = Keyboard.current;
        float inputX = 0f;
        if (k != null)
        {
            if (k.aKey.isPressed || k.leftArrowKey.isPressed)
            {
                inputX -= 1f;
                transform.localScale = new Vector3(-3, 3, 3);
            }
            if (k.dKey.isPressed || k.rightArrowKey.isPressed)
            {
                inputX += 1f;
                transform.localScale = new Vector3(3, 3, 3);
            }
        }

        // checking if player is grounded
        grounded = col && col.IsTouchingLayers(groundMask);
        if (grounded && body.linearVelocity.y <= 0.01f)
        {
            airJumpRemaining = extraAirJump;
        }

        // jump condition
        bool jumpPressed = k != null && (k.spaceKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame);
        if (jumpPressed)
        {
            if (grounded)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
                anim.ResetTrigger("Jump");
                anim.SetTrigger("Jump");
            }
            else if (airJumpRemaining > 0)
            {
                airJumpRemaining--;
                body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);
                anim.ResetTrigger("DoubleJump");
                anim.SetTrigger("DoubleJump");
            }
        }


        // acceleration ground movement; no air control
        float vx = body.linearVelocity.x;
        if (grounded)
        {
            float target = inputX * movementSpeed;
            float rate = (Mathf.Abs(target) > 0.01f) ? accelRate : decelRate; // accelerate vs brake
            vx = Mathf.MoveTowards(vx, target, rate * Time.deltaTime);

            // optional tiny snap-to-zero to prevent drift
            if (Mathf.Abs(target) < 0.01f && Mathf.Abs(vx) < 0.05f) vx = 0f;
        }
        // else: keep vx as-is while in air

        body.linearVelocity = new Vector2(vx, body.linearVelocity.y);

        // set anim parameters
        anim.SetBool("Walk", inputX != 0);
        anim.SetBool("Grounded", grounded);
    }
}
