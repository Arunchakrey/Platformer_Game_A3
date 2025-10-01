using UnityEngine;
using UnityEngine.UI;
#if TMP_PRESENT
using TMPro;
#endif

[RequireComponent(typeof(CanvasGroup))]
public class OxygenHUD : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] OxygenSystem oxygen;   // drag your Player here (auto-finds if left empty)
    [SerializeField] Slider slider;         // leave empty if this script is on the Slider
#if TMP_PRESENT
    [SerializeField] TMP_Text label;        // optional TMP label under the bar
#endif

    [Header("Visibility")]
    [SerializeField] bool fadeWhenNotSubmerged = true;
    [SerializeField] float fadeSpeed = 6f;  // higher = snappier fade

    CanvasGroup group;
    float targetAlpha = 1f;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        if (!slider) slider = GetComponentInChildren<Slider>(true);
        if (!oxygen) oxygen = FindAnyObjectByType<OxygenSystem>();
        if (slider) { slider.minValue = 0; slider.maxValue = 1; slider.wholeNumbers = false; }

        // start hidden if we’re going to fade based on submersion
        if (fadeWhenNotSubmerged) { group.alpha = 0f; group.interactable = group.blocksRaycasts = false; }
    }

    void Update()
    {
        if (!oxygen || !slider) return;

        // 1) update bar (polling)
        float norm = oxygen.Normalized;               // 0..1
        slider.value = norm;
#if TMP_PRESENT
        if (label) label.text = $"O₂ {Mathf.RoundToInt(norm * 100)}%";
#endif

        // 2) fade visibility while submerged
        if (fadeWhenNotSubmerged)
        {
            bool submerged = oxygen.IsSubmerged;      // see OxygenSystem addition below
            targetAlpha = submerged ? 1f : 0f;

            group.alpha = Mathf.MoveTowards(group.alpha, targetAlpha, fadeSpeed * Time.deltaTime);
            bool visible = group.alpha > 0.001f;
            group.interactable = group.blocksRaycasts = visible;
        }
    }
}
