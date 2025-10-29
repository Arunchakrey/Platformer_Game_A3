using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public static GameController Instance { get; private set; }

    [SerializeField] private Slider progressSlider;
    [SerializeField] private Transform saplingsParent; // optional

    private int totalSaplings;
    private int collected;
    private bool subscribed;

    private void Awake()
    {
        // Singleton guard (prevents double subscriptions)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Count by component (includes nested children). If no parent, scan whole scene.
        totalSaplings = saplingsParent
            ? saplingsParent.GetComponentsInChildren<Pickup>(true).Length
            : FindObjectsOfType<Pickup>(true).Length;

        if (totalSaplings <= 0) totalSaplings = 1;

        // Slider config
        progressSlider.wholeNumbers = true;
        progressSlider.minValue = 0;
        progressSlider.maxValue = totalSaplings;
        progressSlider.value = 0;
        collected = 0;

        Debug.Log($"[GC] Saplings found = {totalSaplings}");
    }

    private void OnEnable()
    {
        if (subscribed) return;
        Pickup.OnSapplingCollect += HandleSaplingPickup;
        subscribed = true;
    }

    private void OnDisable()
    {
        if (!subscribed) return;
        Pickup.OnSapplingCollect -= HandleSaplingPickup;
        subscribed = false;
    }

    private void HandleSaplingPickup(int _amount)
    {
        // Always count one sapling per collect
        collected = Mathf.Min(collected + 1, totalSaplings);
        progressSlider.value = collected;

        if (collected >= totalSaplings)
            Debug.Log("[GC] Level complete!");
    }
}
