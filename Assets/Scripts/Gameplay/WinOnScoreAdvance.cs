using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WinOnScoreAdvance : MonoBehaviour
{
    [SerializeField] PlayerScore playerScore;     // auto-find if empty
    [SerializeField] int target = 5;              // win when Total >= target
    [SerializeField] float delay = 0.4f;          // small delay for VFX/SFX
    [SerializeField] string fallbackScene = "LevelSelect";

    bool fired;

    void Awake(){
        if (!playerScore) playerScore = FindAnyObjectByType<PlayerScore>();
        Debug.Log($"[WinSimple] Awake -> playerScore={(playerScore? playerScore.name : "NULL")}, target={target}");
    }

    void OnEnable(){
        Debug.Log($"[WinSimple] OnEnable (enabled={enabled}, gameObject.activeInHierarchy={gameObject.activeInHierarchy})");
        // immediate sync
        if (playerScore) Debug.Log($"[WinSimple] Start Total={playerScore.Total}");
    }

    void Update(){
        // Hotkey: press N to force win
        if (Input.GetKeyDown(KeyCode.N)) { Debug.Log("[WinSimple] N pressed -> ForceAdvance"); StartCoroutine(Advance()); return; }

        if (!playerScore) return;

        // Polling (simple & reliable)
        if (!fired && playerScore.Total >= target){
            Debug.Log($"[WinSimple] Threshold met: {playerScore.Total}/{target} -> advancing");
            fired = true;
            StartCoroutine(Advance());
        }
    }

    [ContextMenu("Force Win Now")]
    public void ForceWinNow(){
        Debug.Log("[WinSimple] ContextMenu -> ForceAdvance");
        StartCoroutine(Advance());
    }

    IEnumerator Advance(){
        // don’t double fire
        if (fired == false) fired = true;

        // short real-time delay for SFX/VFX
        float t = 0f; while (t < delay){ t += Time.unscaledDeltaTime; yield return null; }

        // normalize game state
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);

        int cur = SceneManager.GetActiveScene().buildIndex;
        int next = cur + 1;
        int total = SceneManager.sceneCountInBuildSettings;
        Debug.Log($"[WinSimple] cur={cur} next={next} totalScenes={total}");

        if (next < total){
            Debug.Log($"[WinSimple] Loading next index {next}");
            SceneFader.GoTo(next, 0.3f);
        } else {
            Debug.Log($"[WinSimple] No next scene -> '{fallbackScene}'");
            SceneFader.GoTo(fallbackScene, 0.4f);
        }
    }
}
