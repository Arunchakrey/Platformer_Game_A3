using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class Health : MonoBehaviour
{
    [SerializeField] int maxHearts = 3;
    public int MaxHearts => maxHearts;
    public int CurrentHearts { get; private set; }

    public UnityEvent<int, int> OnHealthChanged = new(); //(current, max)
    public UnityEvent OnDied = new();

    void Awake()
    {
        CurrentHearts = Mathf.Max(1, maxHearts);
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
    }

    public void Damage(int amount = 1)
    {
        if (CurrentHearts <= 0) return;
        CurrentHearts = Mathf.Max(0, CurrentHearts - Mathf.Max(1, amount));
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
        if (CurrentHearts == 0) OnDied.Invoke();
    }

    public void Heal(int amount = 1)
    {
        CurrentHearts = Mathf.Min(MaxHearts, CurrentHearts + Mathf.Max(1, amount));
        OnHealthChanged.Invoke(CurrentHearts, MaxHearts);
    }
}