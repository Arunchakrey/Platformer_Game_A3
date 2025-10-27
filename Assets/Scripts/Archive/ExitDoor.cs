using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    [Header("Destination")]
    [Tooltip("Scene to load when entering (e.g., 'Main Menu', 'Level1', 'End')")]
    public string nextScene = "Main Menu";

    [Header("Visual")]
    public GameObject lockIcon;

    [Header("Debug")]
    public bool debugMode = false;

    private bool unlocked;
    private bool isTransitioning;

    void Start()
    {
        if (lockIcon) lockIcon.SetActive(!unlocked);
    }

    public void Unlock()
    {
        unlocked = true;
        if (lockIcon) lockIcon.SetActive(false);
        if (debugMode) Debug.Log($"ExitDoor: Unlocked! Next scene: {nextScene}");
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!unlocked || !c.CompareTag("Player") || isTransitioning) return;

        // Validate scene name
        if (string.IsNullOrEmpty(nextScene))
        {
            Debug.LogError("ExitDoor: Next scene is not set! Please set the 'Next Scene' field in the Inspector.");
            return;
        }

        // Check if scene exists in build settings
        if (UnityEngine.SceneManagement.SceneUtility.GetBuildIndexByScenePath(nextScene) == -1)
        {
            // Try with "Levels/" prefix (common path)
            string scenePath = $"Assets/Scenes/Levels/{nextScene}.unity";
            if (UnityEngine.SceneManagement.SceneUtility.GetBuildIndexByScenePath(scenePath) == -1)
            {
                Debug.LogError($"ExitDoor: Scene '{nextScene}' not found in build settings! Available scenes: Main Menu, Level1, Level2, Level3, End");
                Debug.LogWarning("ExitDoor: Falling back to 'Main Menu'");
                nextScene = "Main Menu";
            }
        }

        isTransitioning = true;

        // Bank score to progression system
        if (ScoreThisLevel.I != null)
        {
            ScoreThisLevel.I.BankToTotal();
            if (debugMode) Debug.Log("ExitDoor: Score banked to progression");
        }

        // Use SceneFader if available, otherwise direct load
        if (SceneFader.Instance != null)
        {
            if (debugMode) Debug.Log($"ExitDoor: Fading to {nextScene}");
            SceneFader.GoTo(nextScene);
        }
        else
        {
            if (debugMode) Debug.Log($"ExitDoor: Direct loading {nextScene} (no SceneFader)");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextScene);
        }
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.CompareTag("Player"))
        {
            isTransitioning = false;
        }
    }
}
