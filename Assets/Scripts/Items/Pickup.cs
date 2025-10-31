using System;
using UnityEngine;
public class Pickup : MonoBehaviour, IItem
{
    public static event Action<int> OnSapplingCollect;
    public int worth = 1;
    public void Collect()
    {
        Destroy(gameObject);
        SoundEffectManager.Play("Sappling");
        OnSapplingCollect.Invoke(worth);
    }
}

