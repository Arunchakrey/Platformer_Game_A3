using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelDifficulty", menuName = "Game Data/Level Difficulty Config")]
public class LevelDifficultyData : ScriptableObject
{
    [Header("Level Info")]
    public int levelIndex;
    public string levelName;

    [Header("Easy Difficulty")]
    public LevelDifficultySettings easySettings;

    [Header("Normal Difficulty")]
    public LevelDifficultySettings normalSettings;

    [Header("Hard Difficulty")]
    public LevelDifficultySettings hardSettings;

    public LevelDifficultySettings GetSettings(string difficulty)
    {
        return difficulty.ToLower() switch
        {
            "easy" => easySettings,
            "hard" => hardSettings,
            _ => normalSettings
        };
    }
}

[System.Serializable]
public class LevelDifficultySettings
{
    [Header("Enemies")]
    public int enemyCount = 5;
    public float enemySpawnRate = 1f;
    public bool enableEliteEnemies = false;

    [Header("Hazards")]
    public int hazardCount = 3;
    public float hazardDamageMultiplier = 1f;
    public float hazardSpeedMultiplier = 1f;

    [Header("Pickups")]
    public int healthPickupCount = 2;
    public int pointPickupCount = 10;
    public float pickupSpawnRate = 1f;

    [Header("Level Goals")]
    public int scoreGoal = 100;
    public float timeLimit = 300f;
    public int materialsRequired = 5;

    [Header("Environmental")]
    public float gravityMultiplier = 1f;
    public float platformSpeedMultiplier = 1f;
    public bool enableMovingPlatforms = true;

    [Header("Oxygen (Level 2)")]
    public float oxygenDrainRate = 1f;
    public float oxygenRefillRate = 3f;
    public float maxOxygen = 10f;
}
