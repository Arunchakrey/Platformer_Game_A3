using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    public string easyDescription = "More health, slower enemies, easier hazards";
    public string normalDescription = "Balanced gameplay for average players";
    public string hardDescription = "Less health, faster enemies, deadly hazards";

    private string currentDifficulty = "Normal";

    void Start()
    {
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
