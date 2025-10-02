using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    public int Total { get; private set; }

    // Make sure the event always exists
    public UnityEvent<int> OnChanged = new UnityEvent<int>();

    void Awake()
    {
        // Send initial value once so HUD starts correct
        OnChanged.Invoke(Total);
    }

    public void Add(int amount)
    {
        int a = Mathf.Max(0, amount);
        if (a == 0) return;
        Total += a;
        OnChanged.Invoke(Total);
        Debug.Log($"[PlayerScore] +{a} → {Total}");
    }
}
