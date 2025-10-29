using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    [Header("SDG Dialogue Content")]
    [TextArea(3, 10)]
    public string[] startOfLevelDialogue = new string[]
    {
        "Welcome! This level focuses on SDG 13: Climate Action.",
        "Your mission reflects the importance of reducing carbon emissions for a sustainable future."
    };

    [TextArea(3, 10)]
    public string[] endOfLevelDialogue = new string[]
    {
        "Great job! You've completed the level!",
        "By taking climate action, you're helping create a better world for future generations."
    };

    private int currentLine = 0;
    private string[] currentDialogue;

    public static DialogueManager Instance;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        continueButton.onClick.AddListener(ContinueDialogue);
        dialoguePanel.SetActive(false);
    }

    public void StartLevelDialogue()
    {
        currentDialogue = startOfLevelDialogue;
        currentLine = 0;
        ShowDialogue();
    }

    public void EndLevelDialogue()
    {
        currentDialogue = endOfLevelDialogue;
        currentLine = 0;
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        dialoguePanel.SetActive(true);
        Time.timeScale = 0f; // Pause game
        DisplayNextLine();
    }

    private void DisplayNextLine()
    {
        if (currentLine < currentDialogue.Length)
        {
            dialogueText.text = currentDialogue[currentLine];
            currentLine++;
        }
        else
        {
            CloseDialogue();
        }
    }

    private void ContinueDialogue()
    {
        DisplayNextLine();
    }

    private void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
        Time.timeScale = 1f; // Resume game
    }
}