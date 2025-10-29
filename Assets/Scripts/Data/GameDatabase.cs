using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[CreateAssetMenu(fileName = "GameDatabase", menuName = "Game Data/Database")]
public class GameDatabase : ScriptableObject
{
    public static GameDatabase Instance { get; private set; }

    [Header("Difficulties")]
    public DifficultyData easyDifficulty;
    public DifficultyData normalDifficulty;
    public DifficultyData hardDifficulty;

    [Header("Levels")]
    public LevelData[] levels;

    [Header("Enemies")]
    public EnemyData[] enemies;

    [Header("Hazards")]
    public HazardData[] hazards;

    [Header("Pickups")]
    public PickupData[] pickups;

    [Header("Level Difficulty Configs")]
    public LevelDifficultyData[] levelDifficultyConfigs;

    public void Initialize()
    {
        Instance = this;
    }

    public DifficultyData GetDifficulty(string difficultyName)
    {
        return difficultyName.ToLower() switch
        {
            "easy" => easyDifficulty,
            "hard" => hardDifficulty,
            _ => normalDifficulty
        };
    }

    public LevelData GetLevel(int index)
    {
        return levels.FirstOrDefault(l => l.levelIndex == index);
    }

    public LevelData GetLevel(string sceneName)
    {
        return levels.FirstOrDefault(l => l.sceneName == sceneName);
    }

    public EnemyData GetEnemy(string name)
    {
        return enemies.FirstOrDefault(e => e.enemyName == name);
    }

    public HazardData GetHazard(string name)
    {
        return hazards.FirstOrDefault(h => h.hazardName == name);
    }

    public PickupData GetPickup(string name)
    {
        return pickups.FirstOrDefault(p => p.pickupName == name);
    }

    public PickupData GetPickupByType(PickupType type)
    {
        return pickups.FirstOrDefault(p => p.type == type);
    }

    public LevelDifficultyData GetLevelDifficultyConfig(int levelIndex)
    {
        return levelDifficultyConfigs?.FirstOrDefault(l => l.levelIndex == levelIndex);
    }
}
