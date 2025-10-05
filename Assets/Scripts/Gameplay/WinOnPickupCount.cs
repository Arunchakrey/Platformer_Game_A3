using UnityEngine;
using System.Collections;

public class WinOnPickupCount : MonoBehaviour
{
    [SerializeField] int target = 5;           // how many items must be collected to win
    [SerializeField] float delay = 0.4f;       // small delay for sfx/vfx
    [SerializeField] string fallbackScene = "LevelSelect";

    int startCount;
    bool fired;

    void Start()
    {
        // count how many Pickup components exist at level start
        startCount = FindObjectsOfType<Pickup>(true).Length;
        // Debug.Log($"[WinPickups] startCount={startCount}, target={target}");
    }

    void Update()
    {
        if (fired) return;

        // how many remain now
        int remaining = FindObjectsOfType<Pickup>(true).Length;
        int collected = Mathf.Max(0, startCount - remaining);

        // Debug hotkey
        if (Input.GetKeyDown(KeyCode.N)) { StartCoroutine(Advance()); return; }

        if (collected >= target)
        {
            fired = true;
            StartCoroutine(Advance());
        }
    }

    IEnumerator Advance()
    {
        float t = 0f; while (t < delay) { t += Time.unscaledDeltaTime; yield return null; }
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);
        LevelLoader.LoadNext(fallbackScene, 0.5f);
    }
}
