using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]

public class SwimController : MonoBehaviour
{
    [Header("Stroke Swimming")]
    [SerializeField] private float strokeForce = 19f; // impulse added per stroke
    [SerializeField] private float strokeCooldown = 0.35f; // time allowed between strokes
    [SerializeField] private float maxSpeed = 4.5f; // cap speed after stroke
    [SerializeField] private float waterDrag = 3.5f; // passive slowdown while submerged
    [SerializeField] private float aimDeadzone = 0.15f; // how much input magnitude to update aim (changing direction in water)


    [Header("World")]
    [SerializeField] float surfaceY = 0f; // waterline world Y
    [SerializeField] float surfaceSpring = 10f; // pulls you to the surface height
    [SerializeField] float surfaceDamping = 2f; // damps bobbing
    [SerializeField] float allowHeadAbove = 1f; // small allowanve above water (visual)


    [Header("Facing")]
    [SerializeField] bool faceSwimDirection = true; // rotate to aim/velocity


    private Rigidbody2D body;
    private Vector2 input; // raw move input
    private Vector2 aim = Vector2.right; // last valid aim direction
    private float cooldownTimer;


    public bool IsSubmerged => transform.position.y < surfaceY - 0.001f;


    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        body.gravityScale = 0f; // underwater
        body.interpolation = RigidbodyInterpolation2D.Interpolate;
        body.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
    }

    private void Update()
    {
        // read input
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

        // update aim direction when input is significant
        if (input.sqrMagnitude >= aimDeadzone)
        {
            aim = input.normalized;
        }

        // stroke swimming when submerged
        cooldownTimer -= Time.unscaledDeltaTime;
        bool strokePressed = k != null && k.spaceKey.wasPressedThisFrame;
        if (strokePressed && IsSubmerged && cooldownTimer <= 0f)
        {
            DoStroke();
            cooldownTimer = strokeCooldown;
        }

        // face aim/velocity for visuals
        if (faceSwimDirection)
        {
            Vector2 faceDir = IsSubmerged
                ? (body.linearVelocity.sqrMagnitude > 0.01f ? body.linearVelocity : aim)
                : new Vector2(1, 0); // neutral when on surface

            if (faceDir.sqrMagnitude > 0.0001f)
            {
                float angle = Mathf.Atan2(faceDir.y, faceDir.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle - 90f); //sprite faces up
            }
        }
    }

    private void FixedUpdate()
    {
        if (IsSubmerged)
        {
            // water drag gradually reduces velocity (no continuous thrust)
            body.linearVelocity = Vector2.MoveTowards(body.linearVelocity, Vector2.zero, waterDrag * Time.fixedDeltaTime);

            // clamp max speed
            if (body.linearVelocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                body.linearVelocity = body.linearVelocity.normalized * maxSpeed;
            }
        }
        else
        {
            // floating at surface (simple spring toward surfaceY)
            // small allowance so the head can peek out a bit
            float targetY = surfaceY - allowHeadAbove;
            float offset = targetY - body.position.y;

            // spring F = kx - c*v
            float vy = body.linearVelocity.y;
            float springForce = surfaceSpring * offset - surfaceDamping * vy;

            //apply only vertical correction; let horizontal velo drift normally
            body.AddForce(new Vector2(0f, springForce), ForceMode2D.Force);

            // prevent popping far above wtaer
            if (body.position.y > surfaceY + allowHeadAbove)
            {
                body.position = new Vector2(body.position.x, surfaceY + allowHeadAbove);
                if (body.linearVelocity.y > 0f)
                {
                    body.linearVelocity = new Vector2(body.linearVelocity.x, 0f);
                }
            }
        }
    }

    private void DoStroke()
    {
        // add an impulse along aim; preserve momentum
        Vector2 v = body.linearVelocity + aim * strokeForce;

        // cap to max speed magnitude
        if (v.sqrMagnitude > maxSpeed * maxSpeed)
        {
            v = v.normalized * maxSpeed;
        }

        body.linearVelocity = v;
    }

    public float SurfaceY => surfaceY;
}
