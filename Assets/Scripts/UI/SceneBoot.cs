using UnityEngine;

public class SceneBoot : MonoBehaviour
{
    void Start()
    {
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
