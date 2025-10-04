using UnityEngine;

public class GameOverListener : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    bool subscribed;

    void Awake()
    {
        if (!gameOverPanel)
        {
            var t = transform.Find("Panel_GameOver");
            if (t) gameOverPanel = t.gameObject;
        }
        if (gameOverPanel) gameOverPanel.SetActive(false); // default hidden
    }

    void OnEnable()
    {
        TrySubscribe();
        // Sync once to current state (works even if we just subscribed)
        if (GameStateManager.I != null) Handle(GameStateManager.I.State);
    }

    void OnDisable()
    {
        TryUnsubscribe();
    }

    void Update()
    {
        // If manager spawned AFTER OnEnable, catch it here and (re)subscribe
        if (!subscribed) TrySubscribe();
    }

    void TrySubscribe()
    {
        if (subscribed) return;
        if (GameStateManager.I == null) return;
        GameStateManager.I.OnChanged += Handle;
        subscribed = true;
    }

    void TryUnsubscribe()
    {
        if (!subscribed) return;
        if (GameStateManager.I != null) GameStateManager.I.OnChanged -= Handle;
        subscribed = false;
    }

    void Handle(GameState s)
    {
        if (!gameOverPanel) return;
        gameOverPanel.SetActive(s == GameState.GameOver);
    }
}
