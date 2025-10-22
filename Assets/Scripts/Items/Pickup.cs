using System;
using UnityEngine;
public class Pickup : MonoBehaviour, IItem
{
    public static event Action<int> OnSapplingCollect;
    public int worth = 5;
    public void Collect()
    {
        OnSapplingCollect.Invoke(worth);
        Destroy(gameObject);
    }
}

