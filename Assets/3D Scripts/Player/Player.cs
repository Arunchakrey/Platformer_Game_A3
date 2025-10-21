using NUnit.Framework;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private float jumpForce = 4f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float groundCheckDistance = 0.2f;
    private bool isWalking;
    private bool isGrounded;
    private Rigidbody rb;
    private CapsuleCollider capsule;
    private Vector3 moveDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        rb.interpolation = RigidbodyInterpolation.Interpolate;

        capsule = GetComponentInChildren<CapsuleCollider>();
        if (capsule == null)
        {
            Debug.LogError("Player: No CapsuleCollider found on child.");
        }
    }

    private void Update()
    {
        HandleJump();
        HandleMovement();
    }

    private void FixedUpdate()
    {
        MoveWithPhysics();
    }

    private void MoveWithPhysics()
    {
        if (!isWalking) return;
        if (capsule == null) return;

        // Build a world-space capsule from the child collider
        Vector3 center = capsule.transform.TransformPoint(capsule.center);
        Vector3 up = capsule.transform.up;

        float scaledHeight = capsule.height * capsule.transform.lossyScale.y;
        float scaledRadius = capsule.radius * Mathf.Max(capsule.transform.lossyScale.x, capsule.transform.lossyScale.z);
        float half = Mathf.Max(0f, (scaledHeight * 0.5f) - scaledRadius);

        Vector3 p1 = center + up * half; // top sphere center
        Vector3 p2 = center - up * half; // bottom sphere center

        Vector3 dirNorm = moveDir.normalized;
        float step = movementSpeed * Time.fixedDeltaTime;

        // Blocked if our capsule would hit something in the next step
        bool blocked = Physics.CapsuleCast(
            p1, p2, scaledRadius - 0.01f, dirNorm,
            out _, step + 0.02f, ~0, QueryTriggerInteraction.Ignore
        );

        if (!blocked)
        {
            // Move with Rigidbody so collisions are respected
            rb.MovePosition(rb.position + dirNorm * step);
        }
        // else: we’re blocked; do nothing (you can add slide logic later)
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVecotorNormalized();
        moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        float playerSize = 0.7f;
        bool canMove = Physics.Raycast(transform.position, moveDir, playerSize);

        isWalking = moveDir.sqrMagnitude > 0.0001f;

        // rotate only if we have input
        if (canMove)
        {
            if (isWalking)
            {
                float rotateSpeed = 10f;
                Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
            }
        }
        // move every frame based on input
        transform.position += moveDir * movementSpeed * Time.deltaTime;
    }
    
    private void HandleJump()
    {
        //GroundCheck
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance + 0.1f);

        if (isGrounded && gameInput.IsJumping())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public bool IsWalking()
    {
        return isWalking;
    }


}
