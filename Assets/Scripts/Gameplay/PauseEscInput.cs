using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseEscInput : MonoBehaviour
{
    [SerializeField] PauseController pause; // drag your PauseController here (or auto-find)

#if ENABLE_INPUT_SYSTEM
    InputAction pauseAction;
#endif

    void Awake()
    {
        if (!pause) pause = FindAnyObjectByType<PauseController>();

        // New Input System binding (Esc + Gamepad Start)
#if ENABLE_INPUT_SYSTEM
        pauseAction = new InputAction(type: InputActionType.Button);
        pauseAction.AddBinding("<Keyboard>/escape");
        pauseAction.AddBinding("<Gamepad>/start");
        pauseAction.performed += _ => TryToggle();
#endif
    }

    void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        pauseAction?.Enable();
#endif
    }

    void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        pauseAction?.Disable();
#endif
    }

    void Update() { Debug.Log("PauseEscInput Update tick"); /* then the code above */ }


    void TryToggle()
    {
        if (pause == null) return;
        if (GameStateManager.I != null && GameStateManager.I.State == GameState.GameOver) return;
        pause.TogglePause();
    }
}
