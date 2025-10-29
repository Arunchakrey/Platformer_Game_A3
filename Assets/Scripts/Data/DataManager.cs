using UnityEngine;
using System.IO;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    [Header("References")]
    public GameDatabase gameDatabase;

    [Header("Progression")]
    private PlayerProgressionData progressionData;

    private string saveFilePath;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        saveFilePath = Path.Combine(Application.persistentDataPath, "playerdata.json");

        if (gameDatabase != null)
        {
            gameDatabase.Initialize();
        }

        LoadProgressionData();
    }

    public PlayerProgressionData GetProgressionData()
    {
        return progressionData;
    }

    public void LoadProgressionData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            progressionData = JsonUtility.FromJson<PlayerProgressionData>(json);
            Debug.Log("Progression data loaded");
        }
        else
        {
            progressionData = new PlayerProgressionData();
            progressionData.Initialize();
            SaveProgressionData();
            Debug.Log("New progression data created");
        }
    }

    public void SaveProgressionData()
    {
        string json = JsonUtility.ToJson(progressionData, true);
        File.WriteAllText(saveFilePath, json);
        Debug.Log($"Progression data saved to: {saveFilePath}");
    }

    public void ResetProgressionData()
    {
        progressionData = new PlayerProgressionData();
        progressionData.Initialize();
        SaveProgressionData();
        Debug.Log("Progression data reset");
    }

    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex > progressionData.highestLevelUnlocked)
        {
            progressionData.highestLevelUnlocked = levelIndex;
            SaveProgressionData();
        }
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        return levelIndex <= progressionData.highestLevelUnlocked;
    }

    public void UpdateLevelScore(int levelIndex, int score)
    {
        if (levelIndex < progressionData.levelStats.Length)
        {
            var stats = progressionData.levelStats[levelIndex];
            if (stats == null)
            {
                stats = new PlayerProgressionData.LevelStats { levelIndex = levelIndex };
                progressionData.levelStats[levelIndex] = stats;
            }

            if (score > stats.bestScore)
            {
                stats.bestScore = score;
            }
            stats.completed = true;
            stats.timesPlayed++;

            SaveProgressionData();
        }
    }

    public void AddPoints(int points)
    {
        progressionData.totalPoints += points;
        SaveProgressionData();
    }

    public void SetDifficulty(string difficulty)
    {
        progressionData.selectedDifficulty = difficulty;
        SaveProgressionData();
    }

    public string GetDifficulty()
    {
        return progressionData.selectedDifficulty;
    }

    void OnApplicationQuit()
    {
        SaveProgressionData();
    }
}
