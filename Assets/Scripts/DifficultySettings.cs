using UnityEngine;

[System.Serializable]
public class DifficultySettings
{
    [Header("Player")]
    public int playerMaxHealth = 5;
    public float playerSpeedMultiplier = 1f; // reserved for future use

    [Header("Enemies")]
    public float enemySpeedMultiplier = 1f;
    public float enemyJumpForceMultiplier = 1f;

    [Header("Hazards")]
    public float brickGravityScale = 5f;
    public float brickDropDelay = 0.5f;
    public int damageAmount = 1;

    [Header("Collectibles")]
    public int pointsMultiplier = 1; // reserved for future use
    public float respawnTimeMultiplier = 1f; // reserved for future use
}
