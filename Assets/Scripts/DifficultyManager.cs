using UnityEngine;

public class DifficultyManager : MonoBehaviour
{
    public static DifficultyManager Instance { get; private set; }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    [Header("Current Difficulty")]
    public Difficulty currentDifficulty = Difficulty.Normal;

    [Header("Difficulty Presets")]
    public DifficultySettings easySettings = new DifficultySettings
    {
        playerMaxHealth = 7,
        playerSpeedMultiplier = 1f,
        enemySpeedMultiplier = 0.7f,
        enemyJumpForceMultiplier = 0.8f,
        brickGravityScale = 3f,
        brickDropDelay = 0.8f,
        damageAmount = 1,
        pointsMultiplier = 1,
        respawnTimeMultiplier = 1f
    };

    public DifficultySettings normalSettings = new DifficultySettings
    {
        playerMaxHealth = 5,
        playerSpeedMultiplier = 1f,
        enemySpeedMultiplier = 1f,
        enemyJumpForceMultiplier = 1f,
        brickGravityScale = 5f,
        brickDropDelay = 0.5f,
        damageAmount = 1,
        pointsMultiplier = 1,
        respawnTimeMultiplier = 1f
    };

    public DifficultySettings hardSettings = new DifficultySettings
    {
        playerMaxHealth = 3,
        playerSpeedMultiplier = 1f,
        enemySpeedMultiplier = 1.5f,
        enemyJumpForceMultiplier = 1.3f,
        brickGravityScale = 8f,
        brickDropDelay = 0.2f,
        damageAmount = 2,
        pointsMultiplier = 1,
        respawnTimeMultiplier = 1f
    };

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log($"DifficultyManager initialized with difficulty: {currentDifficulty}");
    }

    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;
        Debug.Log($"Difficulty changed to: {difficulty}");
        // Note: Existing game objects won't update until scene reload
    }

    public DifficultySettings GetCurrentSettings()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easySettings,
            Difficulty.Hard => hardSettings,
            _ => normalSettings
        };
    }

    // Helper methods for specific parameters
    public float GetEnemySpeedMultiplier() => GetCurrentSettings().enemySpeedMultiplier;
    public float GetEnemyJumpForceMultiplier() => GetCurrentSettings().enemyJumpForceMultiplier;
    public float GetBrickFallSpeed() => GetCurrentSettings().brickGravityScale;
    public int GetPlayerMaxHealth() => GetCurrentSettings().playerMaxHealth;
    public float GetBrickDropDelay() => GetCurrentSettings().brickDropDelay;
    public int GetDamageAmount() => GetCurrentSettings().damageAmount;
    public float GetPlayerSpeedMultiplier() => GetCurrentSettings().playerSpeedMultiplier;
    public int GetPointsMultiplier() => GetCurrentSettings().pointsMultiplier;
}
