using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class PauseController : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] GameObject pausePanel;            // assign your panel (keep INACTIVE in editor)

    [Header("Navigation")]
    [SerializeField] string mainMenuScene = "Main Menu";
    [SerializeField] float fadeDuration = 0.5f;

    void Start()
    {
        // Try to auto-find the panel if not assigned
        if (!pausePanel)
        {
            // Try common tag/name patterns
            var tagged = GameObject.FindWithTag("PausePanel");
            if (tagged) pausePanel = tagged;
            else
            {
                var byName = GameObject.Find("Panel_Pause") ?? GameObject.Find("PausePanel");
                if (byName) pausePanel = byName;
            }
        }

        HidePanel(); // ensure hidden on start

        // ensure we start unpaused in gameplay
        if (GameStateManager.I != null && GameStateManager.I.State != GameState.GameOver)
            GameStateManager.I.Set(GameState.Playing);
        else
            Time.timeScale = 1f;
    }

    void Update()
    {
        // ignore Esc if GameOver
        if (GameStateManager.I != null && GameStateManager.I.State == GameState.GameOver) return;

        bool pressed = false;

        // New Input System
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame) pressed = true;
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame) pressed = true;
#endif

        // Old Input System
        if (Input.GetKeyDown(KeyCode.Escape)) pressed = true;

        if (pressed) TogglePause();
    }

    // ---------- Core ----------
    public void TogglePause()
    {
        if (GameStateManager.I != null)
        {
            if (GameStateManager.I.State == GameState.Paused) Resume();
            else if (GameStateManager.I.State == GameState.Playing) Pause();
            return;
        }

        // fallback if GSM not present
        if (Time.timeScale == 0f) Resume();
        else Pause();
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        GameStateManager.I?.Set(GameState.Paused);
        ShowPanel();
    }

    public void Resume()
    {
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);
        HidePanel();
    }

    // ---------- Buttons ----------
    public void RestartCurrent()
    {
        Resume(); // normalize state
        string scene = SceneManager.GetActiveScene().name;
        if (SceneFader.I != null && !SceneFader.InTransition) SceneFader.GoTo(scene, fadeDuration);
        else SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
    }

    // Matches your GameOverScreen signature (optional)
    public void Restart(int levelId)
    {
        Resume();
        string levelName = "Level" + levelId;
        if (SceneFader.I != null && !SceneFader.InTransition) SceneFader.GoTo(levelName, fadeDuration);
        else SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Single);
    }

    public void ExitToMainMenu()
    {
        Resume();
        if (SceneFader.I != null && !SceneFader.InTransition) SceneFader.GoTo(mainMenuScene, fadeDuration);
        else SceneManager.LoadSceneAsync(mainMenuScene, LoadSceneMode.Single);
    }

    // ---------- UI helpers ----------
    void ShowPanel()
    {
        Debug.Log($"[PauseController] ShowPanel → {(pausePanel ? pausePanel.name : "NULL")}");

        if (pausePanel)
        {
            Debug.Log($"[PauseController] ShowPanel → {(pausePanel ? pausePanel.name : "NULL")}");

            // Make sure parent Canvas is enabled
            var canvas = pausePanel.GetComponentInParent<Canvas>(true);
            if (canvas) canvas.enabled = true;

            // Activate the panel GameObject
            pausePanel.SetActive(true);

            // If there is a CanvasGroup, ensure it’s visible & interactive
            var cg = pausePanel.GetComponent<CanvasGroup>();
            if (cg)
            {
                cg.alpha = 1f;
                cg.interactable = true;
                cg.blocksRaycasts = true;
            }
        }

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        AudioListener.pause = true; // optional
    }

    void HidePanel()
    {
        if (pausePanel)
        {
            // If there is a CanvasGroup, hide it
            var cg = pausePanel.GetComponent<CanvasGroup>();
            if (cg)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }

            // Deactivate the panel GameObject
            pausePanel.SetActive(false);
        }

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        AudioListener.pause = false; // optional
    }

}
