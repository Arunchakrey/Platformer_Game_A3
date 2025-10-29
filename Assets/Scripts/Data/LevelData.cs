using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "Game Data/Level")]
public class LevelData : ScriptableObject
{
    [Header("Identity")]
    public string levelName;
    public int levelIndex;
    public string sceneName;
    public Sprite levelIcon;

    [Header("Requirements")]
    public int pointsToUnlock = 0;
    public int previousLevelRequired = -1;

    [Header("Goals")]
    public int scoreToWin = 100;
    public float timeLimit = 300f;
    public int materialsRequired = 5;
    public bool hasTimerGoal = false;
    public bool hasScoreGoal = true;
    public bool hasMaterialGoal = false;

    [Header("Player Spawn")]
    public Vector3 spawnPosition;
    public Vector3 checkpointPosition;

    [Header("UI")]
    public string levelDescription;
    public Color themeColor = Color.white;
}
