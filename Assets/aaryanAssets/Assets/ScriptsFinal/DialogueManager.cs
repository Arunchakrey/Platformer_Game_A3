using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public Button continueButton;

    [Header("SDG Dialogue Content")]
    [TextArea(3, 10)]
    public string[] startOfLevelDialogue;

    [TextArea(3, 10)]
    public string[] endOfLevelDialogue;

    [Header("Scene Management")]
    public string menuSceneName = "MainMenu";
    public bool loadMenuAfterEndDialogue = false;

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

        // Auto-populate dialogue if empty in Inspector
        if (startOfLevelDialogue == null || startOfLevelDialogue.Length == 0)
        {
            startOfLevelDialogue = new string[]
            {
                "Welcome to the Reforestation Initiative!",
                "Climate change is causing rising global temperatures, extreme weather events, and sea level rise. The last decade was the warmest in recorded history.",
                "Deforestation accounts for nearly 15% of all greenhouse gas emissions. That's more than all the world's cars, trucks, planes, and ships combined!",
                "Trees are nature's carbon capture technology. A single mature tree can absorb about 48 pounds of carbon dioxide per year and release enough oxygen for two people to breathe.",
                "Your mission: Plant trees across this damaged landscape to restore the forest ecosystem and combat climate change.",
                "Every tree you plant represents real climate action. Plant 5 during this level! Let's make a difference!"
            };
        }

        if (endOfLevelDialogue == null || endOfLevelDialogue.Length == 0)
        {
            endOfLevelDialogue = new string[]
            {
                "Mission Complete! You've successfully reforested the area!",
                "The trees you planted will collectively absorb over 5 tons of CO2 annually as they mature. That's equivalent to taking one car off the road for a year!",
                "Beyond carbon capture, your new forest will: • Prevent soil erosion\n• Provide wildlife habitat\n• Regulate local temperatures\n• Improve air and water quality",
                "Forests are called 'carbon sinks' because they absorb more carbon than they release. Protecting existing forests is just as important as planting new ones.",
                "Sustainable Development Goal 13: Climate Action - Take urgent action to combat climate change and its impacts through measures like reforestation.",
                "Thank you for taking climate action! Small steps like this collectively create massive change for our planet's future."
            };
        }
    }

    public void StartLevelDialogue()
    {
        currentDialogue = startOfLevelDialogue;
        currentLine = 0;
        loadMenuAfterEndDialogue = false; // Don't load menu after start dialogue
        ShowDialogue();
    }

    public void EndLevelDialogue()
    {
        currentDialogue = endOfLevelDialogue;
        currentLine = 0;
        loadMenuAfterEndDialogue = true; // Set flag to load menu after this dialogue
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
        
        // Load menu after end dialogue finishes
        if (loadMenuAfterEndDialogue && !string.IsNullOrEmpty(menuSceneName))
        {
            SceneManager.LoadScene(menuSceneName);
        }
    }
}