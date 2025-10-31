using UnityEngine;

/// <summary>
/// Simple controller to show/hide the difficulty panel.
/// Attach this to any GameObject in the scene (like the Canvas or a MenuController).
/// Reference the DifficultyPanel in the Inspector.
/// </summary>
public class DifficultyPanelController : MonoBehaviour
{
    [Header("Panel Reference")]
    [Tooltip("Drag the DifficultyPanel GameObject here")]
    public GameObject difficultyPanel;

    [Header("Auto Settings")]
    [Tooltip("Hide panel on start (good for main menu)")]
    public bool hideOnStart = true;

    void Start()
    {
        if (hideOnStart && difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
        }
    }

    /// <summary>
    /// Show the difficulty panel - Call this from a button
    /// </summary>
    public void ShowDifficultyPanel()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(true);
            Debug.Log("Difficulty panel shown");
        }
        else
        {
            Debug.LogWarning("DifficultyPanelController: No panel assigned!");
        }
    }

    /// <summary>
    /// Hide the difficulty panel - Call this from a button
    /// </summary>
    public void HideDifficultyPanel()
    {
        if (difficultyPanel != null)
        {
            difficultyPanel.SetActive(false);
            Debug.Log("Difficulty panel hidden");
        }
    }

    /// <summary>
    /// Toggle the difficulty panel - Call this from a button
    /// </summary>
    public void ToggleDifficultyPanel()
    {
        if (difficultyPanel != null)
        {
            bool newState = !difficultyPanel.activeSelf;
            difficultyPanel.SetActive(newState);
            Debug.Log($"Difficulty panel {(newState ? "shown" : "hidden")}");
        }
    }
}
