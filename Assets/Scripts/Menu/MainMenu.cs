using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartLevel2()
    {
        SceneManager.LoadSceneAsync("Level2");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
