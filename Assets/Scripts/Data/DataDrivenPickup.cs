using UnityEngine;

public class DataDrivenPickup : MonoBehaviour
{
    [Header("Data Reference")]
    public PickupData pickupData;
    public string pickupDataName;

    [Header("Auto References")]
    private SpriteRenderer spriteRenderer;
    private Collector collector;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (string.IsNullOrEmpty(pickupDataName) == false && GameDatabase.Instance != null)
        {
            pickupData = GameDatabase.Instance.GetPickup(pickupDataName);
        }

        ApplyData();
    }

    void ApplyData()
    {
        if (pickupData == null) return;

        if (spriteRenderer != null && pickupData.sprite != null)
        {
            spriteRenderer.sprite = pickupData.sprite;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") == false) return;
        if (pickupData == null) return;

        if (pickupData.pointsValue > 0)
        {
            int points = Mathf.RoundToInt(pickupData.pointsValue * GetDifficultyMultiplier());
            if (DataManager.Instance != null)
            {
                DataManager.Instance.AddPoints(points);
            }
        }

        if (pickupData.healthRestore > 0)
        {
            var health = other.GetComponent<PlayerHealthScript>();
            if (health != null)
            {
                health.TakeDamage(-pickupData.healthRestore);
            }
        }

        if (pickupData.materialCount > 0)
        {
            var carry = other.GetComponent<CarryStack>();
            if (carry != null)
            {
                for (int i = 0; i < pickupData.materialCount; i++)
                {
                    carry.AddOne();
                }
            }
        }

        if (pickupData.collectEffect != null)
        {
            Instantiate(pickupData.collectEffect, transform.position, Quaternion.identity);
        }

        if (pickupData.destroyOnCollect)
        {
            Destroy(gameObject);
        }
        else if (pickupData.respawnTime > 0)
        {
            gameObject.SetActive(false);
            Invoke(nameof(Respawn), pickupData.respawnTime);
        }
    }

    void Respawn()
    {
        gameObject.SetActive(true);
    }

    float GetDifficultyMultiplier()
    {
        if (DifficultyManagerDataDriven.Instance != null)
        {
            return DifficultyManagerDataDriven.Instance.GetPointsMultiplier();
        }
        return 1f;
    }

    public PickupData GetData() => pickupData;
}
