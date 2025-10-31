using System;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private int progressAmount;
    public Slider progressSlider;
    private void Start()
    {
        progressAmount = 0;
        progressSlider.value = 0;
        // TODO: Pickup.OnSapplingCollect event doesn't exist in Pickup class
        // Need to implement this event in Pickup.cs if progress tracking is needed
        // Pickup.OnSapplingCollect += IncreaseProgressAmount;
    }

    private void IncreaseProgressAmount(int amount)
    {
        progressAmount += amount;
        progressSlider.value = progressAmount;

        if (progressAmount >= 100)
        {
            //level complete
            Debug.Log("Level Complete");
        }
    }

}
