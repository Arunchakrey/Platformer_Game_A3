using UnityEngine;

public class DifficultyManagerDataDriven : MonoBehaviour
{
    public static DifficultyManagerDataDriven Instance { get; private set; }

    [Header("Data Reference")]
    public GameDatabase gameDatabase;

    private DifficultyData currentDifficultyData;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadDifficulty();
    }

    void LoadDifficulty()
    {
        string difficultyName = DataManager.Instance != null
            ? DataManager.Instance.GetDifficulty()
            : "Normal";

        SetDifficulty(difficultyName);
    }

    public void SetDifficulty(string difficultyName)
    {
        if (gameDatabase != null)
        {
            currentDifficultyData = gameDatabase.GetDifficulty(difficultyName);
            Debug.Log($"Difficulty set to: {difficultyName}");

            if (DataManager.Instance != null)
            {
                DataManager.Instance.SetDifficulty(difficultyName);
            }
        }
    }

    public DifficultyData GetCurrentDifficulty()
    {
        return currentDifficultyData;
    }

    public int GetPlayerMaxHealth() => currentDifficultyData?.playerMaxHealth ?? 5;
    public float GetPlayerSpeedMultiplier() => currentDifficultyData?.playerSpeedMultiplier ?? 1f;
    public float GetEnemySpeedMultiplier() => currentDifficultyData?.enemySpeedMultiplier ?? 1f;
    public float GetEnemyJumpForceMultiplier() => currentDifficultyData?.enemyJumpForceMultiplier ?? 1f;
    public float GetBrickFallSpeed() => currentDifficultyData?.brickGravityScale ?? 5f;
    public float GetBrickDropDelay() => currentDifficultyData?.brickDropDelay ?? 0.5f;
    public int GetDamageAmount() => currentDifficultyData?.damageAmount ?? 1;
    public int GetPointsMultiplier() => currentDifficultyData?.pointsMultiplier ?? 1;
}
