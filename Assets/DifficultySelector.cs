using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class DifficultySelector : MonoBehaviour
{
    [Header("UI References")]
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;
    public TMP_Text currentDifficultyText;
    public TMP_Text descriptionText;

    [Header("Visual Feedback")]
    public Color selectedColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    public Color normalColor = Color.white;

    [Header("Descriptions")]
    public string easyDescription = "More health (150%), slower enemies (80%), easier hazards (50% damage)";
    public string normalDescription = "Balanced gameplay - Standard health, speed, and damage";
    public string hardDescription = "Less health (75%), faster enemies (130%), deadly hazards (150% damage)";

    [Header("Panel Control")]
    [Tooltip("Auto-hide panel after selecting difficulty")]
    public bool hideAfterSelection = true;
    [Tooltip("Delay before hiding (seconds)")]
    public float hideDelay = 0.5f;
    [Tooltip("Reload scene after changing difficulty to apply changes (usually not needed - difficulty applies on next scene load)")]
    public bool reloadSceneAfterChange = false;
    [Tooltip("Delay before reloading scene (seconds)")]
    public float reloadDelay = 1.0f;

    private string currentDifficulty = "Normal";
    private GameObject panelObject;

    void Start()
    {
        // Store reference to parent panel for hiding
        panelObject = gameObject;

        if (DataManager.Instance != null)
        {
            currentDifficulty = DataManager.Instance.GetDifficulty();
        }

        if (easyButton != null)
            easyButton.onClick.AddListener(() => SelectDifficulty("Easy"));

        if (normalButton != null)
            normalButton.onClick.AddListener(() => SelectDifficulty("Normal"));

        if (hardButton != null)
            hardButton.onClick.AddListener(() => SelectDifficulty("Hard"));

        UpdateUI();
    }

    public void SelectDifficulty(string difficulty)
    {
        currentDifficulty = difficulty;

        if (DataManager.Instance != null)
        {
            DataManager.Instance.SetDifficulty(difficulty);
        }

        if (DifficultyManagerDataDriven.Instance != null)
        {
            DifficultyManagerDataDriven.Instance.SetDifficulty(difficulty);
        }
        else if (DifficultyManager.Instance != null)
        {
            var diffEnum = difficulty switch
            {
                "Easy" => DifficultyManager.Difficulty.Easy,
                "Hard" => DifficultyManager.Difficulty.Hard,
                _ => DifficultyManager.Difficulty.Normal
            };
            DifficultyManager.Instance.SetDifficulty(diffEnum);
        }

        UpdateUI();
        Debug.Log($"Difficulty selected: {difficulty}");

        // Auto-reload scene if enabled (to apply difficulty changes)
        if (reloadSceneAfterChange && Application.isPlaying)
        {
            StartCoroutine(ReloadSceneAfterDelay());
        }
        // Otherwise, just hide panel if enabled
        else if (hideAfterSelection)
        {
            StartCoroutine(HidePanelAfterDelay());
        }
    }

    private IEnumerator ReloadSceneAfterDelay()
    {
        Debug.Log($"Scene will reload in {reloadDelay} seconds to apply difficulty changes...");
        yield return new WaitForSeconds(reloadDelay);

        Scene currentScene = SceneManager.GetActiveScene();
        Debug.Log($"Reloading scene: {currentScene.name}");
        SceneManager.LoadScene(currentScene.name);
    }

    private IEnumerator HidePanelAfterDelay()
    {
        yield return new WaitForSeconds(hideDelay);
        if (panelObject != null)
        {
            panelObject.SetActive(false);
            Debug.Log("Difficulty panel hidden");
        }
    }

    // Public method to show the panel (can be called from main menu button)
    public void ShowPanel()
    {
        if (panelObject != null)
        {
            panelObject.SetActive(true);
            UpdateUI(); // Refresh to show current selection
        }
    }

    // Public method to hide the panel (can be called from a close button)
    public void HidePanel()
    {
        if (panelObject != null)
        {
            panelObject.SetActive(false);
        }
    }

    void UpdateUI()
    {
        if (currentDifficultyText != null)
        {
            currentDifficultyText.text = $"Difficulty: {currentDifficulty}";
        }

        UpdateButtonVisual(easyButton, currentDifficulty == "Easy");
        UpdateButtonVisual(normalButton, currentDifficulty == "Normal");
        UpdateButtonVisual(hardButton, currentDifficulty == "Hard");

        if (descriptionText != null)
        {
            descriptionText.text = currentDifficulty switch
            {
                "Easy" => easyDescription,
                "Hard" => hardDescription,
                _ => normalDescription
            };
        }
    }

    void UpdateButtonVisual(Button button, bool isSelected)
    {
        if (button == null) return;

        ColorBlock colors = button.colors;
        colors.normalColor = isSelected ? selectedColor : normalColor;
        colors.highlightedColor = isSelected ? selectedColor * 1.2f : normalColor * 0.9f;
        colors.selectedColor = isSelected ? selectedColor : normalColor;
        button.colors = colors;

        TMP_Text buttonText = button.GetComponentInChildren<TMP_Text>();
        if (buttonText != null)
        {
            buttonText.color = isSelected ? Color.white : new Color(0.8f, 0.8f, 0.8f, 1f);
            buttonText.fontStyle = isSelected ? FontStyles.Bold : FontStyles.Normal;
        }
    }

    public void ShowEasyInfo()
    {
        if (descriptionText != null)
            descriptionText.text = easyDescription;
    }

    public void ShowNormalInfo()
    {
        if (descriptionText != null)
            descriptionText.text = normalDescription;
    }

    public void ShowHardInfo()
    {
        if (descriptionText != null)
            descriptionText.text = hardDescription;
    }

    public void ShowCurrentInfo()
    {
        UpdateUI();
    }
}
