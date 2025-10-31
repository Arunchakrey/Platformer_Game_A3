using UnityEngine;

public class StartLineTrigger : MonoBehaviour
{
    private bool hasTriggered = false;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            DialogueManager.Instance.StartLevelDialogue();
            hasTriggered = true;
        }
    }
}