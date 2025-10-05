using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string firstLevel = "Level1";

    public void PlayGame()
    {
        Time.timeScale = 1f;                   // always unpause before loading
        GameStateManager.I?.Set(GameState.Playing);
        SceneManager.LoadScene(firstLevel, LoadSceneMode.Single); // unloads the menu scene
    }
}
