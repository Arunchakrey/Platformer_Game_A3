using UnityEngine;

public class MenuCursorBootstrap : MonoBehaviour
{
    void Awake()
    {
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
