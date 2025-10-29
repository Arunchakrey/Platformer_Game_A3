using UnityEngine;
using UnityEngine.UI;
using TMPro;

using UnityEngine;

public class DebugTrigger : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Debug.Log($"DEBUG: Something entered trigger! Object: {other.name}, Tag: {other.tag}");

        if (other.CompareTag("Player"))
        {
            Debug.Log("DEBUG: PLAYER DETECTED IN TRIGGER!");
        }
    }
}
