using UnityEngine;

[CreateAssetMenu(fileName = "NewDifficulty", menuName = "Game Data/Difficulty")]
public class DifficultyData : ScriptableObject
{
    [Header("Player")]
    public int playerMaxHealth = 5;
    public float playerSpeedMultiplier = 1f;
    public float playerJumpMultiplier = 1f;

    [Header("Enemies")]
    public float enemySpeedMultiplier = 1f;
    public float enemyJumpForceMultiplier = 1f;
    public float enemyDamageMultiplier = 1f;

    [Header("Hazards")]
    public float brickGravityScale = 5f;
    public float brickDropDelay = 0.5f;
    public int damageAmount = 1;
    public float trapDamageMultiplier = 1f;

    [Header("Collectibles")]
    public int pointsMultiplier = 1;
    public float respawnTimeMultiplier = 1f;
    public float collectibleValueMultiplier = 1f;

    [Header("Level")]
    public float timeToCompleteMultiplier = 1f;
    public int scoreGoalMultiplier = 1;
}
