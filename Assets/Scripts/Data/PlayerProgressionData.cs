using UnityEngine;

[System.Serializable]
public class PlayerProgressionData
{
    public int totalPoints;
    public int highestLevelUnlocked;
    public string selectedDifficulty = "Normal";
    public float totalPlayTime;
    public int totalDeaths;
    public int totalEnemiesKilled;
    public int totalItemsCollected;

    [System.Serializable]
    public class LevelStats
    {
        public int levelIndex;
        public int bestScore;
        public float bestTime;
        public bool completed;
        public int timesPlayed;
        public int deaths;
    }

    public LevelStats[] levelStats = new LevelStats[10];

    public void Initialize()
    {
        for (int i = 0; i < levelStats.Length; i++)
        {
            if (levelStats[i] == null)
            {
                levelStats[i] = new LevelStats { levelIndex = i };
            }
        }
    }
}
