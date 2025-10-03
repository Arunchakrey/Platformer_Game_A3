using UnityEngine;
using UnityEngine.UI;
#if TMP_PRESENT
using TMPro;
#endif

[RequireComponent(typeof(CanvasGroup))]
public class OxygenHUD : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] OxygenSystem oxygen;         // auto-finds if left empty
    [SerializeField] Slider slider;               // auto-finds in children
#if TMP_PRESENT
    [SerializeField] TMP_Text label;              // optional, auto-finds in children
#endif

    [Header("Visibility")]
    [SerializeField] bool fadeWhenNotSubmerged = true;
    [SerializeField] float fadeSpeed = 6f;

    [Header("Low O2 Flash")]
    [Range(0f,1f)] public float warnThreshold     = 0.30f;   // start amber
    [Range(0f,1f)] public float criticalThreshold = 0.15f;   // start red
    public float pulseSpeedWarn     = 3.0f;
    public float pulseSpeedCritical = 6.0f;
    [Range(0f,1f)] public float pulseIntensity   = 0.45f;
    public bool scalePunch = true;
    public float punchAmount = 1.08f;

    [Header("Colors")]
    public Color normalColor   = new Color(0.25f, 0.86f, 1f, 1f); // cyan/teal
    public Color warnColor     = new Color(1f, 0.74f, 0.10f, 1f); // amber
    public Color criticalColor = new Color(1f, 0.18f, 0.18f, 1f); // red

    CanvasGroup group;
    Image fillImage;
    float targetAlpha = 1f;
    RectTransform rt;
    Vector3 baseScale;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        if (!oxygen) oxygen = FindAnyObjectByType<OxygenSystem>();
        if (!slider) slider = GetComponentInChildren<Slider>(true);
#if TMP_PRESENT
        if (!label)  label  = GetComponentInChildren<TMP_Text>(true);
#endif
        if (slider){ slider.minValue = 0; slider.maxValue = 1; slider.wholeNumbers = false; }
        if (slider && slider.fillRect) fillImage = slider.fillRect.GetComponent<Image>();

        rt = (slider ? slider.transform : transform) as RectTransform;
        baseScale = rt ? rt.localScale : Vector3.one;

        if (fadeWhenNotSubmerged){
            group.alpha = 0f; group.interactable = group.blocksRaycasts = false;
        }
    }

    void OnDisable()
    {
        // restore visuals
        if (fillImage) fillImage.color = normalColor;
        if (rt) rt.localScale = baseScale;
    }

    void Update()
    {
        if (!slider) return;

        // 1) Value update
        float norm = oxygen ? oxygen.Normalized : slider.value;
        slider.value = norm;
#if TMP_PRESENT
        if (label) label.text = $"O₂ {Mathf.RoundToInt(norm * 100)}%";
#endif

        // 2) Visibility (fade when submerged)
        if (fadeWhenNotSubmerged && oxygen){
            targetAlpha = oxygen.IsSubmerged ? 1f : 0f;
            group.alpha = Mathf.MoveTowards(group.alpha, targetAlpha, fadeSpeed * Time.unscaledDeltaTime);
            bool visible = group.alpha > 0.001f;
            group.interactable = group.blocksRaycasts = visible;
        }

        // 3) Low-O2 flashing (color + optional scale punch)
        if (fillImage)
        {
            if (norm <= criticalThreshold){
                Pulse(criticalColor, normalColor, pulseSpeedCritical, norm);
            }
            else if (norm <= warnThreshold){
                Pulse(warnColor, normalColor, pulseSpeedWarn, norm);
            }
            else{
                fillImage.color = normalColor;
                if (scalePunch && rt) rt.localScale = baseScale;
            }
        }
    }

    void Pulse(Color hi, Color lo, float speed, float norm)
    {
        // 0..1 pulsing value
        float s = 0.5f * (1f + Mathf.Sin(Time.unscaledTime * speed));
        float t = Mathf.Lerp(1f - pulseIntensity, 1f, s); // soften brightness swing
        fillImage.color = Color.Lerp(lo, hi, t);

        if (scalePunch && rt)
        {
            // stronger punch as O2 drops
            float severity = Mathf.InverseLerp(1f, 0f, norm); // 0→1 as O2 gets low
            float amt = Mathf.Lerp(1f, punchAmount, 0.5f + 0.5f * s) * Mathf.Lerp(0.95f, 1f, severity);
            rt.localScale = baseScale * amt;
        }
    }
}
