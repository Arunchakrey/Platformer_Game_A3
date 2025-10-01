using UnityEngine;
#if TMP_PRESENT
using TMPro;
#endif

public class ScorePopup : MonoBehaviour
{
#if TMP_PRESENT
    [SerializeField] TMP_Text label;
#else
    [SerializeField] UnityEngine.UI.Text label;
#endif
    [SerializeField] float riseSpeed = 1.2f;
    [SerializeField] float lifetime  = 0.9f;
    [SerializeField] float fadeAfter = 0.2f;

    float t;

    public void Show(string text)
    {
        if (!label) label = GetComponentInChildren<
        #if TMP_PRESENT
            TMP_Text
        #else
            UnityEngine.UI.Text
        #endif
        >(true);

        if (label) label.text = text;
        t = 0f;
    }

    void Update()
    {
        t += Time.deltaTime;
        transform.position += Vector3.up * riseSpeed * Time.deltaTime;

        // fade out near the end
        float a = 1f;
        if (t > fadeAfter) a = Mathf.InverseLerp(lifetime, fadeAfter, t);
        SetAlpha(a);

        if (t >= lifetime) Destroy(gameObject);
    }

    void SetAlpha(float a){
#if TMP_PRESENT
        if (label){ var c = label.color; c.a = a; label.color = c; }
#else
        if (label){ var c = label.color; c.a = a; label.color = c; }
#endif
    }
}
