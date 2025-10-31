using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishLineTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            DialogueManager.Instance.EndLevelDialogue();
            hasTriggered = true;
        }
    }
}