using UnityEngine;
using TMPro;

public class HeartsHUD_Text : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] TMP_Text label;
    [SerializeField] bool showSymbols = true;

    void Awake()
    {
        if (!health) health = FindAnyObjectByType<Health>();
        if (!label) label = GetComponent<TMP_Text>();
        if (health)
        {
            health.OnHealthChanged.AddListener(UpdateLabel);
            UpdateLabel(health.CurrentHearts, health.MaxHearts);
        }
    }

    void OnDestroy()
    {
        if (health) health.OnHealthChanged.RemoveListener(UpdateLabel);
    }

    void UpdateLabel(int cur, int max)
    {
        if (!label) return;
        label.text = showSymbols
            ? new string('♥', cur) + new string(' ', Mathf.Max(0, max - cur))
            : $"Hearts: {cur}/{max}";
    }
}