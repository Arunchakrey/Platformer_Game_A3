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
    [SerializeField] BubbleEmitter bubble; // drag from child in Inspector (BubbleEmitter)


    [Header("World")]
    [SerializeField] float surfaceY = 0f; // waterline world Y
    [SerializeField] float surfaceSpring = 10f; // pulls you to the surface height
    [SerializeField] float surfaceDamping = 2f; // damps bobbing
    [SerializeField] float allowHeadAbove = 1f; // small allowanve above water (visual)


    [Header("Facing")]
    [SerializeField] bool faceSwimDirection = true; // rotate to aim/velocity
    private Vector2 facingDir = Vector2.right;
    private float facingRotationOffset = 0f;
    // visual facing (never rotate 180)
    [SerializeField] float diagonalTilt = 35f;   // how much to tilt on down-diagonals
    Vector3 baseScale, baseScaleFlipped;



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
        baseScale = transform.localScale;
        baseScaleFlipped = new Vector3(-Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        baseScale = new Vector3(Mathf.Abs(baseScale.x), baseScale.y, baseScale.z);
        if (!bubble) bubble = GetComponentInChildren<BubbleEmitter>(true);
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

        // face aim/velocity for visuals, but snap to 4 directions
        if (faceSwimDirection)
        {
            Vector2 rawDir = IsSubmerged
                ? (body.linearVelocity.sqrMagnitude > 0.01f ? body.linearVelocity : aim)
                : new Vector2(Mathf.Sign(body.linearVelocity.x == 0 ? 1f : body.linearVelocity.x), 0f); // surface: L/R

            var facing = QuantizeFacingDir(rawDir);
            ApplyFacing(facing);
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

    enum Facing4 { Right, Left, DownRight, DownLeft }

    Facing4 QuantizeFacingDir(Vector2 raw)
    {
        // decide horizontal by x sign; switch to diagonals only if meaningfully downward
        const float downThreshold = -0.25f;

        if (raw.y <= downThreshold)               // downwards intent
            return (raw.x >= 0f) ? Facing4.DownRight : Facing4.DownLeft;
        else                                      // horizontal only
            return (raw.x >= 0f) ? Facing4.Right   : Facing4.Left;
    }

    void ApplyFacing(Facing4 f)
    {
        switch (f)
        {
            case Facing4.Right:
                transform.localScale = baseScale;
                transform.rotation = Quaternion.Euler(0, 0, 0f + facingRotationOffset);
                break;
            case Facing4.Left:
                transform.localScale = baseScaleFlipped;                // flip X instead of 180° rotate
                transform.rotation = Quaternion.Euler(0, 0, 0f + facingRotationOffset);
                break;
            case Facing4.DownRight:
                transform.localScale = baseScale;
                transform.rotation = Quaternion.Euler(0, 0, -diagonalTilt + facingRotationOffset);
                break;
            case Facing4.DownLeft:
                transform.localScale = baseScaleFlipped;               // flip X
                transform.rotation = Quaternion.Euler(0, 0, +diagonalTilt + facingRotationOffset);
                break;
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
        bubble?.OnStroke();

    }

    public float SurfaceY => surfaceY;
}
