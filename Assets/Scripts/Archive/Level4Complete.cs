using UnityEngine;

public class Level4Complete : MonoBehaviour
{
    [Header("Completion Settings")]
    public bool showGameOverScreen = true;
    public GameObject gameOverPanel;

    [Header("Score Banking")]
    public bool bankScoreOnComplete = true;

    public void CompleteLevel()
    {
        // Bank the score
        if (bankScoreOnComplete && ScoreThisLevel.I != null)
        {
            ScoreThisLevel.I.BankToTotal();
            Debug.Log("Level4Complete: Score banked to progression system");
        }

        // Change game state to GameOver (which acts as "Level Complete" state)
        if (GameStateManager.I != null)
        {
            GameStateManager.I.SetState(GameStateManager.State.GameOver);
        }

        // Show completion panel
        if (showGameOverScreen && gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        Debug.Log("Level 4 Complete!");
    }

    // Optional: Auto-complete when exit door is unlocked
    public void OnBuildingComplete()
    {
        // You can call this from BuildSite when the building is finished
        // or tie it to the ExitDoor unlock
        Debug.Log("Level4Complete: Building finished!");
    }
}
