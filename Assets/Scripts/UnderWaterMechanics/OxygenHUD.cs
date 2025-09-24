using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;
using UnityEngine.UI;

public class OxygenHUD_Events : MonoBehaviour
{
    [SerializeField] OxygenSystem oxygen;
    [SerializeField] Slider slider;

    void Awake() { if (!slider) slider = GetComponent<Slider>(); if (slider) { slider.minValue = 0; slider.maxValue = 1; } }
    void OnEnable()
    {
        if (!oxygen) oxygen = FindAnyObjectByType<OxygenSystem>();
        if (oxygen)
        {
            oxygen.OnOxygenChanged.AddListener(UpdateBar);
            UpdateBar(oxygen.currentOxygen, oxygen.MaxOxygen); // initial
        }
    }
    void OnDisable() { if (oxygen) oxygen.OnOxygenChanged.RemoveListener(UpdateBar); }
    void UpdateBar(float cur, float max) { slider.value = max > 0 ? cur / max : 0; }
}
