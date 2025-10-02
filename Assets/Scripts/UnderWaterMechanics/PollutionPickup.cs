using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteRenderer))]
public class PollutionPickup : MonoBehaviour
{
    [SerializeField] PollutionItem data;
    [Header("Idle motion")]
    [SerializeField] bool autoRotate = true;
    [SerializeField] float rotateSpeed = 45f;
    [SerializeField] float bobAmplitude = 0.05f;
    [SerializeField] float bobSpeed = 2f;

    // Optional VFX: put a ParticleSystem as a CHILD named "PickupVFX" (or assign via Inspector)
    [SerializeField] ParticleSystem vfxInChild;

    Vector3 startPos;
    bool collected;
    SpriteRenderer sr;
    Collider2D col;

    void Awake(){
        col = GetComponent<Collider2D>(); col.isTrigger = true;
        sr = GetComponent<SpriteRenderer>();
        if (!vfxInChild) vfxInChild = GetComponentInChildren<ParticleSystem>(true);
        startPos = transform.position;
    }

    void Update(){
        if (autoRotate) transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        if (bobAmplitude > 0f)
            transform.position = startPos + new Vector3(0f, Mathf.Sin(Time.time * bobSpeed) * bobAmplitude, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (collected) return;
        var score = other.GetComponent<PlayerScore>();
        if (!score) return;

        collected = true;
        int pts = data ? data.points : 1;
        score.Add(pts);

        if (data && data.pickupSfx)
            AudioSource.PlayClipAtPoint(data.pickupSfx, transform.position, 0.8f);

        // play child VFX without prefabs
        if (vfxInChild){
            vfxInChild.transform.SetParent(null, true); // detach so we can destroy pickup
            vfxInChild.Play(true);
            Destroy(vfxInChild.gameObject, vfxInChild.main.duration + vfxInChild.main.startLifetime.constantMax + 0.2f);
        }

        // floating +X text (no prefab)
        FloatingTextManager.Show($"+{pts}", transform.position);

        // hide & remove this pickup
        sr.enabled = false;
        col.enabled = false;
        Destroy(gameObject, 0.05f);
    }
}
