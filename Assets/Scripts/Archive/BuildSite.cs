using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildSite : MonoBehaviour
{
    [Header("Goal")]
    public int required = 5;

    [Header("UI")]
    public Slider progress;        // 0..1
    public TMP_Text tip;           // "Bring materials here"/"Press E to deliver"
    public TMP_Text exitPrompt;    // OPTIONAL: small text near door like "Exit →" (assign if you create it)

    [Header("Exit")]
    public ExitDoor exitDoor;

    [Header("Scoring")]
    public int pointsPerDelivered = 4;

    int delivered;
    bool inZone;
    CarryStack carry;

    void Start()
    {
        if (progress) { progress.minValue = 0f; progress.maxValue = 1f; progress.value = 0f; }
        UpdateTip();
        if (exitPrompt) exitPrompt.gameObject.SetActive(false);
    }

    void Update()
    {
        if (!inZone || carry == null) return;

        if (Input.GetKeyDown(KeyCode.E) && carry.HasAny)
        {
            int dropped = carry.DropAll();
            delivered += dropped;

            if (pointsPerDelivered > 0)
                ScoreThisLevel.I?.Add(pointsPerDelivered * dropped);

            if (progress) progress.value = Mathf.Clamp01((float)delivered / required);

            if (delivered >= required)
            {
                if (tip) tip.text = "House built! Head to the exit.";
                if (exitDoor) exitDoor.Unlock();
                if (exitPrompt) exitPrompt.gameObject.SetActive(true); // <-- prompt appears
            }
            else
            {
                UpdateTip();
            }
        }
    }

    void UpdateTip()
    {
        if (!tip) return;
        if (carry != null && carry.HasAny) tip.text = "Press E to deliver materials";
        else tip.text = "Bring materials here";
    }

    void OnTriggerEnter2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;
        inZone = true;
        carry = c.GetComponent<CarryStack>();
        UpdateTip();
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (!c.CompareTag("Player")) return;
        if (c.GetComponent<CarryStack>() == carry)
        {
            inZone = false;
            carry = null;
            UpdateTip();
        }
    }
}
