using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2SceneGuard : MonoBehaviour
{
    void Start()
    {
        var active = SceneManager.GetActiveScene();

        // Log any PlayerScore that isn't from this scene (i.e., leaked from DDOL)
        foreach (var ps in FindObjectsOfType<PlayerScore>(true))
            Debug.Log($"[L2Guard] PlayerScore: {ps.name} in '{ps.gameObject.scene.name}'");

        // OPTIONAL: if you see a PlayerScore in '(DontDestroyOnLoad)', kill it so Level 2 is clean.
        foreach (var ps in FindObjectsOfType<PlayerScore>(true))
        {
            if (ps.gameObject.scene != active)
            {
                Debug.Log("[L2Guard] Destroy stray PlayerScore from " + ps.gameObject.scene.name);
                Destroy(ps.gameObject);
            }
        }

        // Also helpful to see if any Win scripts leaked over
        foreach (var w in FindObjectsOfType<WinOnScoreAdvance>(true))
            Debug.Log($"[L2Guard] WinScript: {w.name} in '{w.gameObject.scene.name}'");
    }
}
