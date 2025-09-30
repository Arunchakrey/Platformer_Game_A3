using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PollutionPickup : MonoBehaviour
{
    [SerializeField] PollutionItem data;
    [SerializeField] bool autoRotate = true;
    [SerializeField] float rotateSpeed = 35f;          // deg/sec
    [SerializeField] float bobAmplitude = 0.5f;       // small float
    [SerializeField] float bobSpeed = 2f;

    Vector3 startPos;

    void Awake(){
        var col = GetComponent<Collider2D>();
        col.isTrigger = true;
        startPos = transform.position;
    }

    void Update(){
        // tiny idle motion for visibility
        if (autoRotate) transform.Rotate(0f, 0f, rotateSpeed * Time.deltaTime);
        if (bobAmplitude > 0f)
            transform.position = startPos + new Vector3(0f, Mathf.Sin(Time.time * bobSpeed) * bobAmplitude, 0f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Only the player should collect; you can also check tag == "Player"
        var score = other.GetComponent<PlayerScore>();
        if (!score) return;

        // 1) Add points
        score.Add(data ? data.points : 1);

        // 3) Feedback: SFX (optional)
        if (data && data.pickupSfx)
            AudioSource.PlayClipAtPoint(data.pickupSfx, transform.position, 0.8f);

        // 4) Despawn (pool-friendly)
        gameObject.SetActive(false);
        // or Destroy(gameObject); if you don't use pooling
    }
}
