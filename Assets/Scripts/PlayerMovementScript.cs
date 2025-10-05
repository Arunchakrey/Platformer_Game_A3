<<<<<<< HEAD
﻿using UnityEngine;
// If you later wire the new Input System, you can re-enable this:
// using UnityEngine.InputSystem;
=======
using Unity.VisualScripting;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;
>>>>>>> 06c448357a009321dc8654cdf0205bc1a303a5dd

public class PlayerMovementScript : MonoBehaviour
{
    [Header("Component")]
    public Rigidbody2D body;
    float horizontalMovement;

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
    public LayerMask groundLayer; // assign this in Inspector (e.g., Ground or Default)

    [Header("Gravity")]
    public float baseGravity = 2f;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

<<<<<<< HEAD
    public Animator anim; // optional
=======
    public ParticleSystem speedFX;

    public Animator anim;//consider adding serialized field
>>>>>>> 06c448357a009321dc8654cdf0205bc1a303a5dd

    void Awake()
    {
        if (!body) body = GetComponent<Rigidbody2D>();
        if (!anim) anim = GetComponent<Animator>();
        body.freezeRotation = true;

        // If no ground layer set, fall back to "everything" so you aren't always airborne
        if (groundLayer.value == 0) groundLayer = Physics2D.AllLayers;
    }

    void Update()
    {
        // --- FALLBACK INPUT (works without PlayerInput) ---
        // If you haven't wired the new Input System, this will read Arrow/WASD
        if (Mathf.Approximately(horizontalMovement, 0f))
            horizontalMovement = Input.GetAxisRaw("Horizontal");

        // Jump fallback
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded())
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpForce);
        if (Input.GetKeyUp(KeyCode.Space) && body.linearVelocity.y > 0f)
            body.linearVelocity = new Vector2(body.linearVelocity.x, body.linearVelocity.y * 0.5f);

        // Face direction (keep your visual scale idea, but you can use 1/−1 too)
        if (horizontalMovement > 0.01f) transform.localScale = new Vector3(3, 3, 3);
        else if (horizontalMovement < -0.01f) transform.localScale = new Vector3(-3, 3, 3);

        // Animator params (only if present)
        if (anim)
        {
            TrySetFloat(anim, "yVelocity", body.linearVelocity.y);
            TrySetFloat(anim, "magnitude", body.linearVelocity.magnitude);
        }
    }

    void FixedUpdate()
    {
        // Horizontal move
        body.linearVelocity = new Vector2(horizontalMovement * speed, body.linearVelocity.y);

        // Custom gravity / terminal velocity
        if (body.linearVelocity.y < 0f)
        {
            body.gravityScale = baseGravity * fallSpeedMultiplier;
            body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Max(body.linearVelocity.y, -maxFallSpeed));
        }
        else
        {
            body.gravityScale = baseGravity;
        }

        // Reset for next frame if using fallback input (so held keys still update via GetAxisRaw)
        horizontalMovement = 0f;
    }

    // New Input System hooks (optional — only used if you add a PlayerInput component)
    /*
    public void Move(InputAction.CallbackContext ctx)
    {
<<<<<<< HEAD
        horizontalMovement = ctx.ReadValue<Vector2>().x;
=======
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
>>>>>>> 06c448357a009321dc8654cdf0205bc1a303a5dd
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
<<<<<<< HEAD
        if (ctx.started && isGrounded())
            body.velocity = new Vector2(body.velocity.x, jumpForce);
        else if (ctx.canceled && body.velocity.y > 0f)
            body.velocity = new Vector2(body.velocity.x, body.velocity.y * 0.5f);
    }
    */

    bool isGrounded()
    {
        if (!groundCheckPos) return false;
        return Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0f, groundLayer);
    }

    void OnDrawGizmosSelected()
=======
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
>>>>>>> 06c448357a009321dc8654cdf0205bc1a303a5dd
    {
        if (!groundCheckPos) return;
        Gizmos.color = Color.white;
        Gizmos.DrawCube(groundCheckPos.position, groundCheckSize);
    }

    // Helper so missing animator params don't spam warnings
    static void TrySetFloat(Animator a, string param, float value)
    {
        foreach (var p in a.parameters)
            if (p.name == param && p.type == AnimatorControllerParameterType.Float)
            { a.SetFloat(param, value); return; }
    }
}