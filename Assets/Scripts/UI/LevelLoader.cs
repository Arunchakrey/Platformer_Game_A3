using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelLoader
{
    public static void LoadNext(string fallback = "LevelSelect", float fade = 0.5f)
    {
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);

        int cur = SceneManager.GetActiveScene().buildIndex;
        int next = cur + 1;
        int total = SceneManager.sceneCountInBuildSettings;

        if (next < total)
        {
            if (SceneFader.InTransition) return;
            if (SceneFader.I != null) SceneFader.GoTo(next, fade);
            else SceneManager.LoadScene(next);
        }
        else
        {
            if (SceneFader.InTransition) return;
            if (SceneFader.I != null) SceneFader.GoTo(fallback, fade);
            else SceneManager.LoadScene(fallback);
        }
    }
}
