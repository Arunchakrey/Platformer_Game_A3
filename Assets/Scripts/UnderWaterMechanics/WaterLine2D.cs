using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WaterlineBar2D : MonoBehaviour
{
    [Header("Placement")]
    [SerializeField] Camera cam;
    [SerializeField] float surfaceY = 7f;   // match OxygenSystem surfaceY
    [SerializeField] float verticalOffset = 0f;

    [Header("Appearance")]
    [SerializeField] float thicknessUnits = 0.10f; // world units (PPU=32 → ~3.2px)
    [SerializeField] string sortingLayer = "FrontDecor";
    [SerializeField] int sortingOrder = 50;
    [SerializeField] Color color = new(0.56f,0.83f,1f,1f);

    [Header("Subtle motion (optional)")]
    [SerializeField] float bobAmplitude = 0.03f;   // vertical bob
    [SerializeField] float bobSpeed = 0.6f;

    SpriteRenderer sr;

    void Awake(){
        if (!cam) cam = Camera.main;
        sr = GetComponent<SpriteRenderer>();
        sr.sortingLayerName = sortingLayer;
        sr.sortingOrder = sortingOrder;
        sr.color = color;
    }

    void LateUpdate(){
        if (!cam) return;

        float baseY = surfaceY + verticalOffset;
        float bob = bobAmplitude > 0f ? Mathf.Sin(Time.time * Mathf.PI * 2f * bobSpeed) * bobAmplitude : 0f;

        // camera extents (orthographic)
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;

        // scale X to span screen + padding
        float pad = 2f;
        transform.position = new Vector3(cam.transform.position.x, baseY + bob, 0f);
        transform.localScale = new Vector3((halfW + pad) * 2f, thicknessUnits, 1f);
    }
}
