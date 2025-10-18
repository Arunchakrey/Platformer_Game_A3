using UnityEngine;

public class CarryStack : MonoBehaviour
{
    [Header("Carry Settings")] public int capacity = 3; // how many items at once
    public int count { get; private set; }

    public bool CanPick => count < capacity;
    public void AddOne() { if (count < capacity) count++; }
    public bool HasAny => count > 0;
    public int DropAll() { int c = count; count = 0; return c; }
}
