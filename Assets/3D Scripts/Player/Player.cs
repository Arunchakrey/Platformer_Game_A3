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

        float playerRadius = 0.5f;
        float playerHeight = 2;
        float moveDistance = movementSpeed * Time.deltaTime;
        
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

        isWalking = moveDir.sqrMagnitude > 0.0001f;

        if (!canMove)
        {
            //cannot move towards moveDir

            //Attempt only x
            Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
            if (canMove)
            {
                //Can only move on X
                moveDir = moveDirX;
            } else
            {
                //Cannot move only on the x

                //attempt only z movement
                Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);
                if (canMove)
                {
                    //can move only z
                    moveDir = moveDirZ;
                }
                else
                {
                    //Cannot move in any direction
                }
            }
        }

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
<<<<<<< HEAD
<<<<<<< HEAD
=======
        

    
>>>>>>> 3drenderingaaryan2nd
=======
        
>>>>>>> e69b51185f47690f574f76132b1546a0f798947f
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
