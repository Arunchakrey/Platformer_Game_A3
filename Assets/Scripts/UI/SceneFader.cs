using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader I { get; private set; }

    [Header("Defaults")]
    [SerializeField] float defaultDuration = 0.5f;
    [SerializeField] Color fadeColor = Color.black;

    CanvasGroup cg;
    Coroutine running;
    bool isFading;
    bool transitionInProgress;

    public static bool InTransition => I != null && (I.isFading || I.transitionInProgress);

    // Always ensure a fader exists before any scene loads
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Bootstrap()
    {
        if (I == null)
        {
            var go = new GameObject("SceneFader");
            go.hideFlags = HideFlags.DontSave;
            go.AddComponent<SceneFader>();
        }
    }

    void Awake()
    {
        if (I && I != this) { Destroy(gameObject); return; }
        I = this;
        DontDestroyOnLoad(gameObject);

        // Build overlay (Canvas + Image + CanvasGroup)
        var cgo = new GameObject("SceneFader_Canvas", typeof(Canvas), typeof(CanvasGroup));
        cgo.transform.SetParent(transform, false);
        var canvas = cgo.GetComponent<Canvas>();
        cg = cgo.GetComponent<CanvasGroup>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 9999;
        cg.blocksRaycasts = true;    // block clicks during fade
        cg.interactable = false;
        cg.alpha = 0f;               // start clear

        var igo = new GameObject("FadeImage", typeof(Image));
        igo.transform.SetParent(cgo.transform, false);
        var img = igo.GetComponent<Image>();
        img.color = fadeColor;
        var rt = img.rectTransform;
        rt.anchorMin = Vector2.zero; rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero; rt.offsetMax = Vector2.zero;

        // Safety: if another load left us opaque, clear quickly after any load
        SceneManager.sceneLoaded += (_, __) =>
        {
            if (!transitionInProgress && cg.alpha > 0.001f)
                StartCoroutine(Fade(1f, 0f, 0.01f));
        };
    }

    void OnDestroy()
    {
        if (I == this) I = null;
        SceneManager.sceneLoaded -= (_, __) => { };
    }

    IEnumerator Fade(float from, float to, float dur)
    {
        isFading = true;
        float t = 0f;
        cg.alpha = from;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(from, to, t / dur);
            yield return null;
        }
        cg.alpha = to;
        cg.blocksRaycasts = cg.alpha > 0.001f;
        isFading = false;
    }

    IEnumerator FadeLoadFade(System.Action load, float dur)
    {
        transitionInProgress = true;

        if (isFading && running != null) yield return running; // finish any fade first

        // Fade OUT
        yield return Fade(0f, 1f, dur);

        // Normalize time/state before loading
        Time.timeScale = 1f;
        GameStateManager.I?.Set(GameState.Playing);

        // Load and wait a frame for new scene UI to exist
        load?.Invoke();
        yield return null;

        // Fade IN
        yield return Fade(1f, 0f, dur);

        transitionInProgress = false;
    }

    // ---------- Static API ----------
    public static void GoTo(string sceneName, float duration = 0.5f)
    {
        Bootstrap();
        if (InTransition) return;
        I.running = I.StartCoroutine(I.FadeLoadFade(() => SceneManager.LoadScene(sceneName),
                                                    duration < 0 ? I.defaultDuration : duration));
    }

    public static void GoTo(int buildIndex, float duration = 0.5f)
    {
        Bootstrap();
        if (InTransition) return;
        I.running = I.StartCoroutine(I.FadeLoadFade(() => SceneManager.LoadScene(buildIndex),
                                                    duration < 0 ? I.defaultDuration : duration));
    }

    public static void FadeOut(float duration = 0.5f) { Bootstrap(); if (!InTransition) I.running = I.StartCoroutine(I.Fade(0f, 1f, duration)); }
    public static void FadeIn (float duration = 0.5f) { Bootstrap(); if (!InTransition) I.running = I.StartCoroutine(I.Fade(1f, 0f, duration)); }
}
