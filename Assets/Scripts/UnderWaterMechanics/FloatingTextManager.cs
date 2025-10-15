using UnityEngine;
#if TMP_PRESENT
using TMPro;
#endif

public class FloatingTextManager : MonoBehaviour
{
    static FloatingTextManager _inst;
    Canvas worldCanvas;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    static void Init(){
        if (_inst) return;
        var go = new GameObject("~FloatingTextManager");
        _inst = go.AddComponent<FloatingTextManager>();
        DontDestroyOnLoad(go);
        _inst.CreateCanvas();
    }

    void CreateCanvas(){
        worldCanvas = new GameObject("WorldCanvas").AddComponent<Canvas>();
        worldCanvas.renderMode = RenderMode.WorldSpace;
        worldCanvas.worldCamera = Camera.main;
        var scaler = worldCanvas.gameObject.AddComponent<UnityEngine.UI.CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10f;
        var cg = worldCanvas.gameObject.AddComponent<CanvasGroup>();
        worldCanvas.transform.SetParent(transform);
        worldCanvas.transform.position = Vector3.zero;
        worldCanvas.sortingLayerName = "FrontDecor"; // optional; make sure this exists, else remove
        worldCanvas.sortingOrder = 100;
        var rect = worldCanvas.GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(10, 10);
    }

    public static void Show(string text, Vector3 worldPos){
        if (!_inst) Init();
        _inst._Show(text, worldPos);
    }

    void _Show(string text, Vector3 worldPos){
        // build a text object on the fly
        GameObject go = new GameObject("FloatingText");
        go.transform.SetParent(worldCanvas.transform, false);
        go.transform.position = worldPos + Vector3.up * 0.1f;

#if TMP_PRESENT
        var txt = go.AddComponent<TextMeshProUGUI>();
        txt.fontSize = 24;
        txt.alignment = TextAlignmentOptions.Center;
        txt.text = text;
        txt.color = Color.white;
#else
        var txt = go.AddComponent<UnityEngine.UI.Text>();
        txt.fontSize = 18;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.text = text;
        txt.color = Color.white;
        txt.raycastTarget = false;
#endif
        var rt = go.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(200, 50);

        go.AddComponent<FloatingTextAuto>(); // handles rise + fade + destroy
    }
}

public class FloatingTextAuto : MonoBehaviour
{
    public float riseSpeed = 1.2f;
    public float lifetime = 0.9f;
    public float fadeAfter = 0.2f;

    float t;
#if TMP_PRESENT
    TMP_Text label;
#else
    UnityEngine.UI.Text label;
#endif

    void Awake(){
#if TMP_PRESENT
        label = GetComponent<TMP_Text>();
#else
        label = GetComponent<UnityEngine.UI.Text>();
#endif
    }

    void Update(){
        t += Time.deltaTime;
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        float a = 1f;
        if (t > fadeAfter) a = Mathf.InverseLerp(lifetime, fadeAfter, t);
        if (label){
            var c = label.color; c.a = a; label.color = c;
        }

        if (t >= lifetime) Destroy(gameObject);
    }
}
