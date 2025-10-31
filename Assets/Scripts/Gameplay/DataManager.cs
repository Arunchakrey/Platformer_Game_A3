using UnityEngine;

/// <summary>
/// Manages game data persistence including difficulty settings, scores, and other player preferences.
/// Uses PlayerPrefs for simple data storage.
/// </summary>
public class DataManager : MonoBehaviour
{
    private static DataManager _instance;
    public static DataManager Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("DataManager");
                _instance = go.AddComponent<DataManager>();
                DontDestroyOnLoad(go);
            }
            return _instance;
        }
    }

    [Header("Default Settings")]
    [SerializeField] private string defaultDifficulty = "Normal";

    private const string DIFFICULTY_KEY = "GameDifficulty";
    private const string HIGH_SCORE_KEY = "HighScore";

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize default difficulty if not set
        if (!PlayerPrefs.HasKey(DIFFICULTY_KEY))
        {
            SetDifficulty(defaultDifficulty);
        }
    }

    #region Difficulty Management

    /// <summary>
    /// Gets the current difficulty setting.
    /// </summary>
    /// <returns>Difficulty string: "Easy", "Normal", or "Hard"</returns>
    public string GetDifficulty()
    {
        return PlayerPrefs.GetString(DIFFICULTY_KEY, defaultDifficulty);
    }

    /// <summary>
    /// Sets and saves the difficulty setting.
    /// </summary>
    /// <param name="difficulty">Difficulty string: "Easy", "Normal", or "Hard"</param>
    public void SetDifficulty(string difficulty)
    {
        PlayerPrefs.SetString(DIFFICULTY_KEY, difficulty);
        PlayerPrefs.Save();
        Debug.Log($"DataManager: Difficulty set to {difficulty}");
    }

    #endregion

    #region Score Management

    /// <summary>
    /// Gets the saved high score.
    /// </summary>
    /// <returns>High score value</returns>
    public int GetHighScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY, 0);
    }

    /// <summary>
    /// Sets and saves the high score if it's higher than the current high score.
    /// </summary>
    /// <param name="score">New score to compare</param>
    /// <returns>True if new high score was set</returns>
    public bool SetHighScore(int score)
    {
        int currentHighScore = GetHighScore();
        if (score > currentHighScore)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY, score);
            PlayerPrefs.Save();
            Debug.Log($"DataManager: New high score: {score}");
            return true;
        }
        return false;
    }

    #endregion

    #region Utility Methods

    /// <summary>
    /// Resets all saved data to defaults.
    /// </summary>
    public void ResetAllData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("DataManager: All data reset to defaults");
    }

    /// <summary>
    /// Saves all current PlayerPrefs data.
    /// </summary>
    public void SaveData()
    {
        PlayerPrefs.Save();
        Debug.Log("DataManager: Data saved");
    }

    #endregion
}
