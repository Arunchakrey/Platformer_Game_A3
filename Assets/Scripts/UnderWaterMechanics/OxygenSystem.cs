using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class OxygenSystem : MonoBehaviour
{
    [SerializeField] float maxOxygen = 10f;
    [SerializeField] float drainPerSec = 1f;
    [SerializeField] float refillPerSec = 3f;

    [SerializeField] SwimController swimmer;
    public UnityEvent<float, float> OnOxygenChanged; // (current, max)
    public UnityEvent OnDrowned;

    float oxy;

    public float currentOxygen => oxy;
    public float MaxOxygen => maxOxygen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (!swimmer)
        {
            swimmer = GetComponent<SwimController>();
        }

        oxy = maxOxygen;
        OnOxygenChanged?.Invoke(oxy, maxOxygen);
    }

    // Update is called once per frame
    void Update()
    {
        bool atSurface = swimmer && swimmer.IsAtSurface();
        float delta = (atSurface ? +refillPerSec : -drainPerSec) * Time.deltaTime;
        oxy = Mathf.Clamp(oxy + delta, 0f, maxOxygen);
        OnOxygenChanged?.Invoke(oxy, maxOxygen);

        if (Time.frameCount % 30 == 0)
        {
            Debug.Log($"O2={oxy:F2}, atSurface={atSurface}, y={transform.position.y:F2}, surfY={swimmer?.SurfaceY}");
        }

        if (oxy <= 0f) OnDrowned?.Invoke();
    }

    public void RefillBurst(float amount)
    {
        oxy = Mathf.Clamp(oxy + amount, 0f, maxOxygen);
        OnOxygenChanged?.Invoke(oxy, maxOxygen);
    }
}
