using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class CausticsScroll : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] Vector2 speed = new(0.12f, 0.05f);
    [SerializeField] Vector2 tiling = new(4f, 4f); // how many repeats across the screen
    [SerializeField, Range(0f,1f)] float alpha = 0.22f;
    [SerializeField] float extraWidth = 2f, extraHeight = 2f;

    RawImage img;
    RectTransform rt;

    void Awake(){
        if (!cam) cam = Camera.main;
        img = GetComponent<RawImage>();
        rt = GetComponent<RectTransform>();
        var c = img.color; c.a = alpha; img.color = c;
        // Set initial UV repeat
        img.uvRect = new Rect(img.uvRect.position, tiling);
    }

    void LateUpdate(){
        if (!cam) return;

        // Stretch to camera size (world-space canvas assumed)
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        float w = (halfW + extraWidth) * 2f;
        float h = (halfH + extraHeight) * 2f;

        // Make the canvas follow camera if needed
        transform.parent.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 9f);
        rt.sizeDelta = new Vector2(w, h);

        // Scroll UV
        var uv = img.uvRect;
        uv.position += speed * Time.deltaTime;
        img.uvRect = uv;
    }
}
