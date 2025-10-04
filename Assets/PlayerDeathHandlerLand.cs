using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerDeathHandlerLand : MonoBehaviour
{
    [Header("Refs (auto if empty)")]
    [SerializeField] private PlayerHealthScript health;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;

    [Header("Disable on death")]
    [SerializeField] private Behaviour[] disableOnDeath; // movement, animator, etc.

    [Header("Hide visuals on death")]
    [SerializeField] private SpriteRenderer[] renderersToHide; // auto if empty

    [Header("Hide HUD groups on death (NOT the GameOver canvas/panel!)")]
    [SerializeField] private GameObject[] uiRootsToHide;

    [Header("Optional: explicitly show this panel on death")]
    [SerializeField] private GameObject gameOverPanelToShow; // assign your Panel_GameOver

    void Awake()
    {
        if (!health)      health = GetComponent<PlayerHealthScript>();
        if (!rb)          rb = GetComponent<Rigidbody2D>();
        if (!playerInput) playerInput = GetComponent<PlayerInput>();
        if (renderersToHide == null || renderersToHide.Length == 0)
            renderersToHide = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void OnEnable(){ if (health) health.OnDied.AddListener(HandleDeath); }
    void OnDisable(){ if (health) health.OnDied.RemoveListener(HandleDeath); }

    void HandleDeath()
    {
        // input + physics off
        if (playerInput) playerInput.enabled = false;
        if (rb){ rb.linearVelocity = Vector2.zero; rb.simulated = false; }

        // disable behaviours
        foreach (var b in disableOnDeath) if (b) b.enabled = false;

        // hide sprites (player body)
        foreach (var sr in renderersToHide) if (sr) sr.enabled = false;

        // hide ONLY gameplay HUD groups; do NOT hide the canvas that contains GameOver panel
        foreach (var go in uiRootsToHide)
            if (go && (gameOverPanelToShow == null || !IsAncestorOf(go.transform, gameOverPanelToShow.transform)))
                go.SetActive(false);

        // set state once
        GameStateManager.I?.Set(GameState.GameOver);
        Debug.Log("[DeathHandlerLand] GameOver requested");

        // explicitly show GameOver panel (optional but robust)
        if (gameOverPanelToShow) {
            gameOverPanelToShow.SetActive(true);
            var cg = gameOverPanelToShow.GetComponent<CanvasGroup>();
            if (cg){ cg.alpha = 1f; cg.interactable = true; cg.blocksRaycasts = true; }
        }
    }

    bool IsAncestorOf(Transform potentialAncestor, Transform node)
    {
        var t = node;
        while (t != null){ if (t == potentialAncestor) return true; t = t.parent; }
        return false;
    }
}
