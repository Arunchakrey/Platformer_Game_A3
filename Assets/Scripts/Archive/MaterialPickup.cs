using UnityEngine;

public class MaterialPickup : MonoBehaviour
{
    [Tooltip("Player must have tag 'Player' and a CarryStack component.")]
    public string playerTag = "Player";
    public int pointsOnPickup = 0; // set to 4 if you want immediate points

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag(playerTag)) return;
        var carry = c.GetComponent<CarryStack>();
        if (carry == null || !carry.CanPick) return;
        carry.AddOne();
        if (pointsOnPickup != 0) ScoreThisLevel.I?.Add(pointsOnPickup);
        gameObject.SetActive(false); // if pooled; else Destroy(gameObject);
    }
}
