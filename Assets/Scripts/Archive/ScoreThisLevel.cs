using UnityEngine;

public class ScoreThisLevel : MonoBehaviour
{
    public static ScoreThisLevel I { get; private set; }
    public int LevelScore { get; private set; }

    void Awake() { I = this; }
    public void Add(int v) { LevelScore += v; }
    public void BankToTotal() { Progression.Total += LevelScore; }
    public void ResetLevelScore() { LevelScore = 0; }
}
