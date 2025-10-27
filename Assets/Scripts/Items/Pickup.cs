using System;
using UnityEngine;
public class Pickup : MonoBehaviour, IItem
{
    public static event Action<int> OnSapplingCollect;
    public int worth = 10;
    public void Collect()
    {
        Destroy(gameObject);
        OnSapplingCollect.Invoke(worth);
    }
}

