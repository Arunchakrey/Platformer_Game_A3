using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class SwimController : MonoBehaviour
{
    [Header("Swim")]
    [SerializeField] private float accel = 12f;
    [SerializeField] private float maxSpeed = 4.5f;
    [SerializeField] private float waterDrag = 4f;

    [Header("World")]
    [SerializeField] float surfaceY = 0f; // waterline world Y

    private Rigidbody2D body;
    private Vector2 input;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f; // underwater
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Update()
    {
        var k = Keyboard.current;
        input = Vector2.zero;
        if (k != null)
        {
            if (k.wKey.isPressed || k.upArrowKey.isPressed || k.spaceKey.isPressed) input.y += 1;
            if (k.sKey.isPressed || k.downArrowKey.isPressed || k.leftShiftKey.isPressed) input.y -= 1;
            if (k.dKey.isPressed || k.rightArrowKey.isPressed) input.x += 1;
            if (k.aKey.isPressed || k.leftArrowKey.isPressed) input.x -= 1;
            input = Vector2.ClampMagnitude(input, 1f);
        }

        // face swim direction if moving
        if (input.sqrMagnitude > 0.01f)
        {
            var angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }

    private void FixedUpdate()
    {
        // simple acceleration + drag model
        Vector2 desired = input * maxSpeed;
        Vector2 force = (desired - body.linearVelocity) * accel;
        body.AddForce(force);

        // water drap pulls velocity towards 0
        body.linearVelocity = Vector2.MoveTowards(body.linearVelocity, Vector2.zero, waterDrag * Time.fixedDeltaTime);
    }

    public bool IsAtSurface() => transform.position.y >= surfaceY - 0.05f;
    public float SurfaceY => surfaceY;
}
