// LevelWin.cs  (put this in Level 1 and Level 2 only; NOT in Main Menu)
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class WinOnScoreAdvance : MonoBehaviour
{
    [Header("Bind your level's PlayerScore here (or leave empty to auto-find)")]
    [SerializeField] PlayerScore playerScore;

    [Header("Win condition")]
    [SerializeField] int target = 5;
    [SerializeField] float delay = 0.4f;
    [SerializeField] string fallbackScene = "LevelSelect";
    [SerializeField] float fadeDuration = 0.5f;

    bool fired;
    float retry;

    void Awake()
    {
        TryBind("Awake");
    }

    void Start()
    {
        // make sure we don't enter paused from a prior scene
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);
        // Optional: fade in on scene load
        if (SceneFader.I != null && !SceneFader.InTransition) SceneFader.FadeIn(fadeDuration);
    }

    void Update()
    {
        // --- Hotkey (supports both input systems) ---
        bool forceKey = false;
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.nKey.wasPressedThisFrame) forceKey = true;
#endif
        if (Input.GetKeyDown(KeyCode.N)) forceKey = true;
        if (forceKey) { StartCoroutine(Advance()); return; }

        // Bind late if the player spawns slightly after scene load
        if (!playerScore)
        {
            retry += Time.unscaledDeltaTime;
            if (retry >= 0.25f) { retry = 0f; TryBind("Update"); }
            return;
        }

        if (!fired && playerScore.Total >= target)
        {
            fired = true;
            StartCoroutine(Advance());
        }
    }

    void TryBind(string where)
    {
        if (playerScore) return;
        var active = SceneManager.GetActiveScene();
        var all = FindObjectsOfType<PlayerScore>(true);
        foreach (var ps in all)
        {
            if (ps && ps.gameObject.scene == active) { playerScore = ps; break; }
        }
        if (!playerScore && all.Length > 0) playerScore = all[0]; // last resort
        // Debug.Log($"[LevelWin] TryBind({where}) -> {(playerScore ? playerScore.name : "NULL")}");
    }

    IEnumerator Advance()
    {
        // brief real-time delay for last pickup VFX/SFX
        float t = 0f; while (t < delay) { t += Time.unscaledDeltaTime; yield return null; }

        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);

        int cur = SceneManager.GetActiveScene().buildIndex;
        int next = cur + 1;
        int total = SceneManager.sceneCountInBuildSettings;

        if (next < total)
        {
            // Use SceneFader if available
            if (SceneFader.I != null && !SceneFader.InTransition)
            {
                SceneFader.GoTo(next, fadeDuration);
            }
            else
            {
                SceneManager.LoadScene(next, LoadSceneMode.Single);
            }
        }
        else
        {
            if (SceneFader.I != null && !SceneFader.InTransition)
            {
                SceneFader.GoTo(fallbackScene, fadeDuration);
            }
            else
            {
                SceneManager.LoadScene(fallbackScene, LoadSceneMode.Single);
            }
        }
    }
}
