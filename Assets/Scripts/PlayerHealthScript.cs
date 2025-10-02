using System;
using System.Net.NetworkInformation;
using UnityEngine;
using UnityEngine.InputSystem.Android.LowLevel;

public class PlayerHealthScript : MonoBehaviour
{
    public int MaxHealth = 5;
    private int currentHealth;
    //public HealthUI healthUI;
    private SpriteRenderer spriteRenderer;
    public static event Action OnPlayedDied;

    void Start()
    {
        ResetHealth();

        spriteRenderer = GetComponent<SpriteRenderer>();
        //AndroidGameControllerState.OnReset += ResetHealth;


    }

    private void ResetHealth()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IItem enemy = collision.GetComponent<IItem>();
        // if (enemy)
        // {

        // }
    }
}
