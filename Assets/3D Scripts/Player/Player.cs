using NUnit.Framework;
using Unity.VisualScripting.InputSystem;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private float movementSpeed = 7f;
    [SerializeField] private GameInput gameInput;
    private bool isWalking;

    private void Update()
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

    public bool IsWalking()
    {
        return isWalking;
    }


}
