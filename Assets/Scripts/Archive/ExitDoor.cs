using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoor : MonoBehaviour
{
    [SerializeField] string nextScene = "LevelSelect"; // or direct next level
    public GameObject lockIcon;
    bool unlocked;

    void Start() { if (lockIcon) lockIcon.SetActive(!unlocked); }

    public void Unlock() { unlocked = true; if (lockIcon) lockIcon.SetActive(false); }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!unlocked || !c.CompareTag("Player")) return;
        ScoreThisLevel.I?.BankToTotal();
        SceneManager.LoadScene(nextScene);
    }
}
