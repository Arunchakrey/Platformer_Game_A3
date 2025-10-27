using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DifficultyUI : MonoBehaviour
{
    [Header("UI Elements")]
    public TMP_Text currentDifficultyText;
    public Button easyButton;
    public Button normalButton;
    public Button hardButton;

    [Header("Visual Feedback")]
    public Color selectedColor = new Color(0.2f, 0.8f, 0.2f, 1f);
    public Color normalColor = Color.white;

    [Header("Debug")]
    public bool debugMode = false;

    void Start()
    {
        if (easyButton != null) easyButton.onClick.AddListener(() => SetDifficulty(DifficultyManager.Difficulty.Easy));
        if (normalButton != null) normalButton.onClick.AddListener(() => SetDifficulty(DifficultyManager.Difficulty.Normal));
        if (hardButton != null) hardButton.onClick.AddListener(() => SetDifficulty(DifficultyManager.Difficulty.Hard));
        UpdateUI();
    }

    void SetDifficulty(DifficultyManager.Difficulty difficulty)
    {
        if (DifficultyManager.Instance != null)
        {
            DifficultyManager.Instance.SetDifficulty(difficulty);
            UpdateUI();
            if (debugMode) Debug.Log($"DifficultyUI: Difficulty set to {difficulty}");
        }
        else
        {
            Debug.LogError("DifficultyUI: DifficultyManager instance not found!");
        }
    }

    void UpdateUI()
    {
        if (DifficultyManager.Instance == null)
        {
            if (debugMode) Debug.LogWarning("DifficultyUI: Cannot update UI - DifficultyManager not found");
            return;
        }

        DifficultyManager.Difficulty currentDifficulty = DifficultyManager.Instance.currentDifficulty;

        if (currentDifficultyText != null)
        {
            currentDifficultyText.text = $"Current Difficulty: {currentDifficulty}";
        }

        UpdateButtonColor(easyButton, currentDifficulty == DifficultyManager.Difficulty.Easy);
        UpdateButtonColor(normalButton, currentDifficulty == DifficultyManager.Difficulty.Normal);
        UpdateButtonColor(hardButton, currentDifficulty == DifficultyManager.Difficulty.Hard);
    }

    void UpdateButtonColor(Button button, bool isSelected)
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
        }
    }

    public void RefreshUI() => UpdateUI();

    // Optional methods for displaying difficulty descriptions
    public void ShowEasyDescription()
    {
        if (currentDifficultyText != null)
            currentDifficultyText.text = "Easy: More health, slower enemies, easier hazards";
    }

    public void ShowNormalDescription()
    {
        if (currentDifficultyText != null)
            currentDifficultyText.text = "Normal: Balanced gameplay for average players";
    }

    public void ShowHardDescription()
    {
        if (currentDifficultyText != null)
            currentDifficultyText.text = "Hard: Less health, faster enemies, deadly hazards";
    }

    public void ShowCurrentDifficulty() => UpdateUI();
}
