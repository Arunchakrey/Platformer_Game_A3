using UnityEngine;
public class PauseListener : MonoBehaviour
{
    [SerializeField] GameObject pausePanel;  // set this in Inspector (panel starts INACTIVE)
    void OnEnable() { if (GameStateManager.I != null) GameStateManager.I.OnChanged += Handle; Handle(GameStateManager.I ? GameStateManager.I.State : GameState.Playing); }
    void OnDisable() { if (GameStateManager.I != null) GameStateManager.I.OnChanged -= Handle; }
    void Handle(GameState s)
    {
        bool show = s == GameState.Paused;
        if (pausePanel) pausePanel.SetActive(show);
        Cursor.visible = show; Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
    }
}
