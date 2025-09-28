using UnityEngine;

[DisallowMultipleComponent]
public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] Health health;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Behaviour[] disableOnDeath; // e.g., SwimController, PlayerMovement, Animator

    void Awake()
    {
        if (!health) health = GetComponent<Health>();
        if (!rb) rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable() { if (health) health.OnDied.AddListener(HandleDeath); }
    void OnDisable() { if (health) health.OnDied.RemoveListener(HandleDeath); }

    void HandleDeath()
    {
        foreach (var b in disableOnDeath) if (b) b.enabled = false;
        if (rb) { rb.linearVelocity = Vector2.zero; rb.simulated = false; }
        GameStateManager.I.Set(GameState.GameOver);
    }
}
