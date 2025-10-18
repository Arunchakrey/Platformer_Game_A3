using UnityEngine;
using UnityEngine.InputSystem;

[DisallowMultipleComponent]
public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Behaviour[] disableOnDeath; // SwimController, Animator, etc.

    void Awake()
    {
        if (!health) health = GetComponent<Health>() ?? GetComponentInParent<Health>();
        if (!rb) rb = GetComponent<Rigidbody2D>() ?? GetComponentInParent<Rigidbody2D>();
        Debug.Log($"[DeathHandler] Awake: health={(health ? health.name : "NULL")} rb={(rb ? rb.name : "NULL")}");
    }

    void OnEnable()
    {
        if (health)
        {
            health.OnDied.AddListener(HandleDeath);
            Debug.Log("[DeathHandler] Subscribed to Health.OnDied");
        }
        else
        {
            Debug.LogError("[DeathHandler] No Health found. Put Health on this object or its parent, or assign in Inspector.");
        }
    }

    void OnDisable()
    {
        if (health)
        {
            health.OnDied.RemoveListener(HandleDeath);
            Debug.Log("[DeathHandler] Unsubscribed");
        }
    }

    void HandleDeath()
    {
        Debug.Log("[DeathHandler] GameOver requested");
        foreach (var b in disableOnDeath) if (b) b.enabled = false;
        if (rb) { rb.linearVelocity = Vector2.zero; rb.simulated = false; }
        GameStateManager.I?.Set(GameState.GameOver);
    }

    // TEMP: press K to simulate death and test the chain
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Debug.Log("[DeathHandler] DEBUG K pressed → HandleDeath()");
            HandleDeath();
        }
    }
}
