using UnityEngine;
using UnityEngine.Events;

public class PlayerScore : MonoBehaviour
{
    public int Total { get; private set; }
    public UnityEvent<int> OnChanged; // emits new total

    public void Add(int amount){
        Total += Mathf.Max(0, amount);
        OnChanged?.Invoke(Total);
    }
}
