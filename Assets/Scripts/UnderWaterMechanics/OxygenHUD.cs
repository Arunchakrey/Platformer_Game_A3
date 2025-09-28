using UnityEngine;
using UnityEngine.UI;

public class OxygenHUD_Events : MonoBehaviour
{
    [SerializeField] OxygenSystem oxygen;
    [SerializeField] Slider slider;

    void Awake()
    {
        if (!slider) slider = GetComponent<Slider>();
        if (!oxygen) oxygen = FindAnyObjectByType<OxygenSystem>();
        if (slider)
        {
            slider.minValue = 0; slider.maxValue = 1; slider.wholeNumbers = false;
        }
    }

    void Update()
    {
        if (oxygen && slider) slider.value = oxygen.Normalized;
    }
}
