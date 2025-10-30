using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Manages difficulty settings using an enum-based approach.
/// Provides difficulty modifiers for health, damage, enemy speed, etc.
/// </summary>
public class DifficultyManager : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Normal,
        Hard
    }

    // Event fired when difficulty changes
    public static UnityEvent<Difficulty> OnDifficultyChanged = new UnityEvent<Difficulty>();

    private static DifficultyManager _instance;
    public static DifficultyManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DifficultyManager");
                _instance = go.AddComponent<DifficultyManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    [Header("Current Difficulty")]
    [SerializeField] private Difficulty currentDifficulty = Difficulty.Normal;

    [Header("Easy Mode Settings")]
    [SerializeField] private float easyHealthMultiplier = 1.5f;
    [SerializeField] private float easyDamageMultiplier = 0.75f;
    [SerializeField] private float easyEnemySpeedMultiplier = 0.8f;
    [SerializeField] private float easyHazardDamageMultiplier = 0.5f;

    [Header("Normal Mode Settings")]
    [SerializeField] private float normalHealthMultiplier = 1f;
    [SerializeField] private float normalDamageMultiplier = 1f;
    [SerializeField] private float normalEnemySpeedMultiplier = 1f;
    [SerializeField] private float normalHazardDamageMultiplier = 1f;

    [Header("Hard Mode Settings")]
    [SerializeField] private float hardHealthMultiplier = 0.75f;
    [SerializeField] private float hardDamageMultiplier = 1.5f;
    [SerializeField] private float hardEnemySpeedMultiplier = 1.3f;
    [SerializeField] private float hardHazardDamageMultiplier = 1.5f;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Load difficulty from DataManager if available
        LoadDifficultyFromDataManager();
    }

    void OnEnable()
    {
        // Reload difficulty when scene changes
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(UnityEngine.SceneManagement.Scene scene, UnityEngine.SceneManagement.LoadSceneMode mode)
    {
        // Reload difficulty from DataManager when any scene loads
        LoadDifficultyFromDataManager();
        Debug.Log($"[DifficultyManager] Scene loaded: {scene.name}, Difficulty reloaded: {currentDifficulty}");
    }

    private void LoadDifficultyFromDataManager()
    {
        if (DataManager.Instance != null)
        {
            string savedDifficulty = DataManager.Instance.GetDifficulty();
            currentDifficulty = savedDifficulty switch
            {
                "Easy" => Difficulty.Easy,
                "Hard" => Difficulty.Hard,
                _ => Difficulty.Normal
            };
            Debug.Log($"[DifficultyManager] Loaded difficulty from DataManager: {currentDifficulty}");
        }
    }

    #region Difficulty Management

    /// <summary>
    /// Gets the current difficulty setting.
    /// </summary>
    public Difficulty GetDifficulty()
    {
        return currentDifficulty;
    }

    /// <summary>
    /// Sets the current difficulty and saves it.
    /// </summary>
    public void SetDifficulty(Difficulty difficulty)
    {
        currentDifficulty = difficulty;

        if (DataManager.Instance != null)
        {
            string difficultyString = difficulty.ToString();
            DataManager.Instance.SetDifficulty(difficultyString);
        }

        Debug.Log($"DifficultyManager: Difficulty set to {difficulty}");

        // Notify all listeners that difficulty has changed
        OnDifficultyChanged?.Invoke(difficulty);
    }

    #endregion

    #region Multiplier Getters

    /// <summary>
    /// Gets the player health multiplier based on current difficulty.
    /// </summary>
    public float GetHealthMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyHealthMultiplier,
            Difficulty.Hard => hardHealthMultiplier,
            _ => normalHealthMultiplier
        };
    }

    /// <summary>
    /// Gets the damage taken multiplier based on current difficulty.
    /// </summary>
    public float GetDamageTakenMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyDamageMultiplier,
            Difficulty.Hard => hardDamageMultiplier,
            _ => normalDamageMultiplier
        };
    }

    /// <summary>
    /// Gets the enemy speed multiplier based on current difficulty.
    /// </summary>
    public float GetEnemySpeedMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyEnemySpeedMultiplier,
            Difficulty.Hard => hardEnemySpeedMultiplier,
            _ => normalEnemySpeedMultiplier
        };
    }

    /// <summary>
    /// Gets the hazard damage multiplier based on current difficulty.
    /// </summary>
    public float GetHazardDamageMultiplier()
    {
        return currentDifficulty switch
        {
            Difficulty.Easy => easyHazardDamageMultiplier,
            Difficulty.Hard => hardHazardDamageMultiplier,
            _ => normalHazardDamageMultiplier
        };
    }

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

    #endregion
}
