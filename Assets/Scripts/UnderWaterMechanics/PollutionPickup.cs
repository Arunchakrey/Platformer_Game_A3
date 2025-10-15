using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PollutionPickup : MonoBehaviour
{
    [SerializeField] PollutionItem data;
    [SerializeField] ParticleSystem vfxInChild;

    Vector3 startPos;
    bool collected;
    SpriteRenderer sr;
    Collider2D col;

    void Awake(){
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
        sr = GetComponent<SpriteRenderer>();
        if (!vfxInChild) vfxInChild = GetComponentInChildren<ParticleSystem>(true);
        startPos = transform.position;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;

        // Prefer the player’s root PlayerScore; fallback to scene find if null
        var score = other.GetComponentInParent<PlayerScore>();
        if (!score) score = FindAnyObjectByType<PlayerScore>();

        if (!score){
            Debug.LogWarning($"[Pickup] No PlayerScore found. Trigger by '{other.name}'. " +
                             "Ensure PlayerScore is on the player ROOT object.");
            return;
        }

        collected = true;
        int pts = data ? data.points : 1;
        score.Add(Mathf.Max(1, pts));              // <-- actually increments score
        Debug.Log($"[Pickup] +{pts} from {name} by {other.name}");

        if (data && data.pickupSfx) AudioSource.PlayClipAtPoint(data.pickupSfx, transform.position, 0.8f);
        if (vfxInChild){
            vfxInChild.transform.SetParent(null, true);
            vfxInChild.Play(true);
            Destroy(vfxInChild.gameObject, vfxInChild.main.duration + vfxInChild.main.startLifetime.constantMax + 0.2f);
        }

        sr.enabled = false;
        col.enabled = false;
        Destroy(gameObject, 0.05f);
    }
}
