using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerDeathHandler : MonoBehaviour
{
    [Header("Refs (auto if empty)")]
    [SerializeField] private PlayerHealthScript health;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private PlayerInput playerInput;

    [Header("Disable on death")]
    [SerializeField] private Behaviour[] disableOnDeath; // e.g., PlayerMovementScript, Animator, etc.

    [Header("Hide visuals on death")]
    [SerializeField] private SpriteRenderer[] renderersToHide; // auto-pulled if empty

    [Header("Hide UI on death (optional)")]
    [SerializeField] private GameObject[] uiRootsToHide; // assign HUD groups (ability icons, etc.)

    void Awake()
    {
        if (!health) health = GetComponent<PlayerHealthScript>();
        if (!rb) rb = GetComponent<Rigidbody2D>();
        if (!playerInput) playerInput = GetComponent<PlayerInput>();
        if (renderersToHide == null || renderersToHide.Length == 0)
            renderersToHide = GetComponentsInChildren<SpriteRenderer>(true);
    }

    void OnEnable()
    {
        if (health) health.OnDied.AddListener(HandleDeath);
    }
    void OnDisable()
    {
        if (health) health.OnDied.RemoveListener(HandleDeath);
    }

    private void HandleDeath()
    {
        // stop input
        if (playerInput) playerInput.enabled = false;

        // stop physics
        if (rb)
        {
            rb.linearVelocity = Vector2.zero;
            rb.simulated = false;
        }

        // disable behaviours (movement, animator, etc.)
        foreach (var b in disableOnDeath)
            if (b) b.enabled = false;

        // hide player sprites
        foreach (var sr in renderersToHide)
            if (sr) sr.enabled = false;

        // hide UI groups
        foreach (var go in uiRootsToHide)
            if (go) go.SetActive(false);
    }
}
