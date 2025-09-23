using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private float movementSpeed = 6f, jumpSpeed = 5f;
    [SerializeField] LayerMask groundMask;

    private Rigidbody2D body;
    private Collider2D col;


    // load script once game starts
    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    // checking for user input updates
    private void Update()
    {
        var k = Keyboard.current;
        float inputX = 0f;

        if (k != null)
        {
            if (k.aKey.isPressed || k.leftArrowKey.isPressed) inputX -= 1f;
            if (k.dKey.isPressed || k.rightArrowKey.isPressed) inputX += 1f;
        }

        bool grounded = col && col.IsTouchingLayers(groundMask);

        // Jump only from ground
        if (k != null && (k.spaceKey.wasPressedThisFrame || k.upArrowKey.wasPressedThisFrame) && grounded)
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpSpeed);

        // Horizontal: apply only when grounded; in air, preserve current X
        float vx = grounded ? inputX * movementSpeed : body.linearVelocity.x;
        body.linearVelocity = new Vector2(vx, body.linearVelocity.y);
    }
}
