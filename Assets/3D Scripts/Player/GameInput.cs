using UnityEngine;

public class GameInput : MonoBehaviour

{
    private @_3DPlayerInputActions playerInputActions;
    public void Awake()
    {
        playerInputActions = new @_3DPlayerInputActions();
        playerInputActions.Player.Enable();
    }
    public Vector2 GetMovementVecotorNormalized()
    {
        Vector2 inputVector = playerInputActions.Player.Movement.ReadValue<Vector2>();

        inputVector = inputVector.normalized;

        Debug.Log(inputVector);

        return inputVector;
    }
}