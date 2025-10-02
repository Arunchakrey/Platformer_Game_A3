using UnityEngine;
using TMPro;

public class PointsHUD : MonoBehaviour
{
    [SerializeField] PlayerScore score;
    [SerializeField] TMP_Text label;
    [SerializeField] string format = "Points: {0}";

    void Awake()
    {
        if (!label) label = GetComponent<TMP_Text>();
        if (!score) score = FindAnyObjectByType<PlayerScore>();
        if (!label) Debug.LogError("[PointsHUD] No TMP_Text assigned");
        if (!score) Debug.LogError("[PointsHUD] No PlayerScore found in scene");
    }

    void OnEnable()
    {
        if (score != null) score.OnChanged.AddListener(UpdateLabel);
        // seed current value
        if (score != null) UpdateLabel(score.Total);
    }

    void OnDisable()
    {
        if (score != null) score.OnChanged.RemoveListener(UpdateLabel);
    }

    void UpdateLabel(int total)
    {
        if (!label) return;
        label.text = string.Format(format, total);
        // quick feedback ping
        StopAllCoroutines();
        StartCoroutine(Punch());
        Debug.Log($"[PointsHUD] Display = {total}");
    }

    System.Collections.IEnumerator Punch()
    {
        var rt = (RectTransform)label.transform;
        var baseScale = rt.localScale;
        var target = baseScale * 1.12f;
        float t = 0f;
        while (t < 0.12f){ t += Time.unscaledDeltaTime; rt.localScale = Vector3.Lerp(baseScale, target, t/0.12f); yield return null; }
        t = 0f;
        while (t < 0.12f){ t += Time.unscaledDeltaTime; rt.localScale = Vector3.Lerp(target, baseScale, t/0.12f); yield return null; }
        rt.localScale = baseScale;
    }
}
