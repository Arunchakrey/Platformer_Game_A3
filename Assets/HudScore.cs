using UnityEngine;
using TMPro;

public class HUDScore : MonoBehaviour
{
    public TextMeshProUGUI levelScoreText;
    public TextMeshProUGUI totalText;

    void Update()
    {
        if (levelScoreText)
            levelScoreText.text = $"Score: {ScoreThisLevel.I?.LevelScore ?? 0}";
        if (totalText)
            totalText.text = $"Total: {Progression.Total}";
    }
}
