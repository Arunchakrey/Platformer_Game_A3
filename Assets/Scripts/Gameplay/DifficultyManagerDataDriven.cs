using UnityEngine;
using System;

/// <summary>
/// Data-driven difficulty manager using ScriptableObject configurations.
/// Allows for more flexible and designer-friendly difficulty adjustments.
/// </summary>
public class DifficultyManagerDataDriven : MonoBehaviour
{
    private static DifficultyManagerDataDriven _instance;
    public static DifficultyManagerDataDriven Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DifficultyManagerDataDriven");
                _instance = go.AddComponent<DifficultyManagerDataDriven>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    [Header("Difficulty Configurations")]
    [SerializeField] private DifficultyConfig easyConfig;
    [SerializeField] private DifficultyConfig normalConfig;
    [SerializeField] private DifficultyConfig hardConfig;

    [Header("Current Settings")]
    [SerializeField] private string currentDifficulty = "Normal";
    private DifficultyConfig currentConfig;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Create default configs if none assigned
        InitializeDefaultConfigs();

        // Load saved difficulty
        if (DataManager.Instance != null)
        {
            string savedDifficulty = DataManager.Instance.GetDifficulty();
            SetDifficulty(savedDifficulty);
        }
        else
        {
            SetDifficulty(currentDifficulty);
        }
    }

    void InitializeDefaultConfigs()
    {
        if (easyConfig == null)
        {
            easyConfig = ScriptableObject.CreateInstance<DifficultyConfig>();
            easyConfig.Initialize("Easy", 1.5f, 0.75f, 0.8f, 0.5f, 1.2f);
        }

        if (normalConfig == null)
        {
            normalConfig = ScriptableObject.CreateInstance<DifficultyConfig>();
            normalConfig.Initialize("Normal", 1f, 1f, 1f, 1f, 1f);
        }

        if (hardConfig == null)
        {
            hardConfig = ScriptableObject.CreateInstance<DifficultyConfig>();
            hardConfig.Initialize("Hard", 0.75f, 1.5f, 1.3f, 1.5f, 0.8f);
        }
    }

    #region Difficulty Management

    /// <summary>
    /// Gets the current difficulty name.
    /// </summary>
    public string GetDifficulty()
    {
        return currentDifficulty;
    }

    /// <summary>
    /// Sets the difficulty by name.
    /// </summary>
    public void SetDifficulty(string difficulty)
    {
        currentDifficulty = difficulty;

        currentConfig = difficulty switch
        {
            "Easy" => easyConfig,
            "Hard" => hardConfig,
            _ => normalConfig
        };

        if (DataManager.Instance != null)
        {
            DataManager.Instance.SetDifficulty(difficulty);
        }

        Debug.Log($"DifficultyManagerDataDriven: Difficulty set to {difficulty}");
    }

    /// <summary>
    /// Gets the current difficulty configuration.
    /// </summary>
    public DifficultyConfig GetCurrentConfig()
    {
        return currentConfig ?? normalConfig;
    }

    #endregion

    #region Multiplier Getters

    public float GetHealthMultiplier() => GetCurrentConfig().healthMultiplier;
    public float GetDamageTakenMultiplier() => GetCurrentConfig().damageTakenMultiplier;
    public float GetEnemySpeedMultiplier() => GetCurrentConfig().enemySpeedMultiplier;
    public float GetHazardDamageMultiplier() => GetCurrentConfig().hazardDamageMultiplier;
    public float GetResourceDropMultiplier() => GetCurrentConfig().resourceDropMultiplier;

    #endregion

    #region Utility Methods

    /// <summary>
    /// Applies difficulty modifier to a base health value.
    /// </summary>
    public float ApplyHealthModifier(float baseHealth)
    {
        return baseHealth * GetHealthMultiplier();
    }

    /// <summary>
    /// Applies difficulty modifier to damage taken.
    /// </summary>
    public float ApplyDamageModifier(float baseDamage)
    {
        return baseDamage * GetDamageTakenMultiplier();
    }

    /// <summary>
    /// Applies difficulty modifier to enemy speed.
    /// </summary>
    public float ApplyEnemySpeedModifier(float baseSpeed)
    {
        return baseSpeed * GetEnemySpeedMultiplier();
    }

    /// <summary>
    /// Applies difficulty modifier to hazard damage.
    /// </summary>
    public float ApplyHazardDamageModifier(float baseDamage)
    {
        return baseDamage * GetHazardDamageMultiplier();
    }

    /// <summary>
    /// Applies difficulty modifier to resource drops.
    /// </summary>
    public float ApplyResourceDropModifier(float baseAmount)
    {
        return baseAmount * GetResourceDropMultiplier();
    }

    #endregion
}

/// <summary>
/// ScriptableObject for difficulty configuration data.
/// Can be created as assets in the project for easy tweaking.
/// </summary>
[CreateAssetMenu(fileName = "DifficultyConfig", menuName = "Game/Difficulty Configuration")]
[Serializable]
public class DifficultyConfig : ScriptableObject
{
    [Header("Difficulty Info")]
    public string difficultyName = "Normal";

    [Header("Player Stats")]
    [Tooltip("Multiplier for player health")]
    [Range(0.5f, 2f)]
    public float healthMultiplier = 1f;

    [Tooltip("Multiplier for damage taken by player")]
    [Range(0.5f, 2f)]
    public float damageTakenMultiplier = 1f;

    [Header("Enemy Stats")]
    [Tooltip("Multiplier for enemy movement speed")]
    [Range(0.5f, 2f)]
    public float enemySpeedMultiplier = 1f;

    [Header("Hazards & Environment")]
    [Tooltip("Multiplier for hazard damage")]
    [Range(0.5f, 2f)]
    public float hazardDamageMultiplier = 1f;

    [Header("Resources")]
    [Tooltip("Multiplier for resource/collectible drops")]
    [Range(0.5f, 2f)]
    public float resourceDropMultiplier = 1f;

    /// <summary>
    /// Initialize the config programmatically.
    /// </summary>
    public void Initialize(string name, float health, float damage, float enemySpeed, float hazardDamage, float resources)
    {
        difficultyName = name;
        healthMultiplier = health;
        damageTakenMultiplier = damage;
        enemySpeedMultiplier = enemySpeed;
        hazardDamageMultiplier = hazardDamage;
        resourceDropMultiplier = resources;
    }
}
