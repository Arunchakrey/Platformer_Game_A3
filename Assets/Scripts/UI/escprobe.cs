using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class EscProbe : MonoBehaviour
{
    void Update()
    {
        bool hit = false;

        // Old Input Manager path
        if (Input.GetKeyDown(KeyCode.Escape)) { Debug.Log("[EscProbe] Old Input: Escape"); hit = true; }

        // New Input System direct polling
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        { Debug.Log("[EscProbe] New Input: Keyboard.escape"); hit = true; }
        if (Gamepad.current != null && Gamepad.current.startButton.wasPressedThisFrame)
        { Debug.Log("[EscProbe] New Input: Gamepad.start"); hit = true; }
#endif

        if (hit) return;
    }

    // IMGUI fallback (fires even if Input systems are weird, as long as Game view has focus)
    void OnGUI()
    {
        Event e = Event.current;
        if (e != null && e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape)
            Debug.Log("[EscProbe] IMGUI: Escape");
    }
}
