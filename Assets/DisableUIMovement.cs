using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;

public class DisableUIMoveOnly : MonoBehaviour
{
    IEnumerator Start()
    {
        yield return null; // wait for InputSystemUIInputModule to initialize
        var module = EventSystem.current?.GetComponent<InputSystemUIInputModule>();
        if (module != null)
        {
            module.move.action?.Disable();
        }
    }
}
