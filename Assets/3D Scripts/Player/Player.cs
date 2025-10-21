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

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        if(rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
    }

    private void Update()
    {
        HandleJump();
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 inputVector = gameInput.GetMovementVecotorNormalized();
        Vector3 moveDir = new Vector3(inputVector.x, 0f, inputVector.y);

        isWalking = moveDir.sqrMagnitude > 0.0001f;

        // rotate only if we have input
        if (isWalking)
        {
            float rotateSpeed = 10f;
            Quaternion targetRot = Quaternion.LookRotation(moveDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotateSpeed);
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
