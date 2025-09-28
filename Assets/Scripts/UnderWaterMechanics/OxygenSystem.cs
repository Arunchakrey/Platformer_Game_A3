using UnityEngine;
using UnityEngine.Events;

public class OxygenSystem : MonoBehaviour
{
    [Header("O2")]
    [SerializeField] float maxOxygen = 10f;
    [SerializeField] float drainPerSec = 1f, refillPerSec = 3f;

    [Header("Surface (Y height)")]
    [SerializeField] float surfaceY = 0f;

    float oxy;
    public float CurrentOxygen => oxy;
    public float MaxOxygen => maxOxygen;
    public float Normalized => maxOxygen > 0 ? oxy / maxOxygen : 0f;

    // NEW: visibility signal
    public bool IsSubmerged { get; private set; }
    public UnityEvent<bool> OnSubmergedChanged = new();          // true = underwater

    void Awake()
    {
        oxy = maxOxygen;
        // initialize submerged state
        bool underwater = transform.position.y < surfaceY - 0.05f;
        IsSubmerged = underwater;
        OnSubmergedChanged.Invoke(IsSubmerged);
    }

    void Update()
    {
        bool atSurface = transform.position.y >= surfaceY - 0.05f;
        float rate = atSurface ? +refillPerSec : -drainPerSec;
        oxy = Mathf.Clamp(oxy + rate * Time.deltaTime, 0f, maxOxygen);

        // detect surface crossing
        bool newSubmerged = !atSurface;
        if (newSubmerged != IsSubmerged)
        {
            IsSubmerged = newSubmerged;
            OnSubmergedChanged.Invoke(IsSubmerged);
        }
    }
}
