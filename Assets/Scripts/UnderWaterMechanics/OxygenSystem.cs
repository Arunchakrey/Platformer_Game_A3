using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.Events;

public class OxygenSystem : MonoBehaviour
{
    [SerializeField] float maxOxygen = 10f;
    [SerializeField] float drainPerSec = 1f;
    [SerializeField] float refillPerSec = 3f;
    [SerializeField] float surfaceY = 0f; // sea-level

    float oxy;
    bool dead;
    public UnityEvent OnDrowned = new();

    public float CurrentOxygen => oxy;
    public float MaxOxygen => maxOxygen;
    public float Normalized => maxOxygen > 0 ? oxy / maxOxygen : 0f;

    void Awake()
    {
        oxy = maxOxygen;
    }

    void Update()
    {
        bool atSurface = transform.position.y >= surfaceY - 0.05f;
        float rate = atSurface ? +refillPerSec : -drainPerSec;
        oxy = Mathf.Clamp(oxy + rate * Time.deltaTime, 0f, maxOxygen);

        if (!dead && oxy <= 0f)
        {
            dead = true;
            OnDrowned.Invoke();
        }
    }
}