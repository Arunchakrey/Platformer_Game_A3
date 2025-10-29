using UnityEngine;

public class LevelDifficultyApplier : MonoBehaviour
{
    [Header("References")]
    public GameDatabase gameDatabase;

    [Header("Current Level")]
    public int levelIndex;
    public LevelDifficultyData levelDifficultyConfig;

    private string currentDifficulty;
    private LevelDifficultySettings currentSettings;

    void Start()
    {
        LoadDifficulty();
        ApplyDifficultyToLevel();
    }

    void LoadDifficulty()
    {
        if (DataManager.Instance != null)
        {
            currentDifficulty = DataManager.Instance.GetDifficulty();
        }
        else
        {
            currentDifficulty = "Normal";
        }

        if (levelDifficultyConfig != null)
        {
            currentSettings = levelDifficultyConfig.GetSettings(currentDifficulty);
        }

        Debug.Log($"Level {levelIndex} loaded with {currentDifficulty} difficulty");
    }

    void ApplyDifficultyToLevel()
    {
        if (currentSettings == null) return;

        ApplyToOxygenSystem();
        ApplyToLevelGoals();
        ApplyToSpawners();
    }

    void ApplyToOxygenSystem()
    {
        var oxygenSystem = FindObjectOfType<OxygenSystem>();
        if (oxygenSystem != null && currentSettings != null)
        {
            oxygenSystem.drainRate = currentSettings.oxygenDrainRate;
            oxygenSystem.maxOxygen = currentSettings.maxOxygen;
            Debug.Log($"Oxygen system configured for {currentDifficulty}");
        }
    }

    void ApplyToLevelGoals()
    {
        var winCondition = FindObjectOfType<WinOnScoreAdvance>();
        if (winCondition != null && currentSettings != null)
        {
            winCondition.scoreTarget = currentSettings.scoreGoal;
            Debug.Log($"Score goal set to {currentSettings.scoreGoal}");
        }

        var buildSite = FindObjectOfType<BuildSite>();
        if (buildSite != null && currentSettings != null)
        {
            buildSite.required = currentSettings.materialsRequired;
            Debug.Log($"Materials required set to {currentSettings.materialsRequired}");
        }
    }

    void ApplyToSpawners()
    {
        if (currentSettings == null) return;
    }

    public LevelDifficultySettings GetCurrentSettings()
    {
        return currentSettings;
    }

    public int GetEnemyCount() => currentSettings?.enemyCount ?? 5;
    public int GetHazardCount() => currentSettings?.hazardCount ?? 3;
    public int GetHealthPickupCount() => currentSettings?.healthPickupCount ?? 2;
    public int GetScoreGoal() => currentSettings?.scoreGoal ?? 100;
    public float GetTimeLimit() => currentSettings?.timeLimit ?? 300f;
    public int GetMaterialsRequired() => currentSettings?.materialsRequired ?? 5;
}
