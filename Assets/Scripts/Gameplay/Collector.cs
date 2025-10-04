using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Collector : MonoBehaviour
{
    [SerializeField] int pointsPerItem = 1;
    [SerializeField] PlayerScore playerScore;

    void Awake(){
        // Grab PlayerScore from the player root (parent) or scene
        if (!playerScore)
            playerScore = GetComponentInParent<PlayerScore>() ?? FindAnyObjectByType<PlayerScore>();

        // Safety check: this collider must be a trigger, but we DON'T touch the root collider
        var col = GetComponent<Collider2D>();
        if (!col.isTrigger)
            Debug.LogWarning("[Collector] The PickupTrigger collider should be IsTrigger = true.");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Find IItem on the collider, its parent, or its children
        var item = other.GetComponent<IItem>()
                ?? other.GetComponentInParent<IItem>()
                ?? other.GetComponentInChildren<IItem>();

        if (item == null) return;

        // Award points BEFORE the item destroys itself
        if (playerScore && pointsPerItem > 0)
            playerScore.Add(pointsPerItem);

        item.Collect();
        // Debug.Log($"[Collector] Picked {other.name} (+{pointsPerItem})");
    }
}
