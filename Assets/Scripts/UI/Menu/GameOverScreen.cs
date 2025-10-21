using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverScreen : MonoBehaviour
{
    public void Restart(int levelId)
    {
        Time.timeScale = 1f;
        string levelName = "Level" + levelId;
        GameStateManager.I?.Set(GameState.Playing);
        SceneManager.LoadSceneAsync(levelName);
    }

    public void ExitButton()
    {
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);

        // Explicitly fix the cursor for menus
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadSceneAsync("Main Menu", LoadSceneMode.Single);
    }
}
