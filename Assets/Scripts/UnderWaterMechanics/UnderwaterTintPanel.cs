using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class UnderwaterTintPanel : MonoBehaviour
{
    [SerializeField] Camera cam;
    [SerializeField] float surfaceY = 6.3f;   // match your OxygenSystem/SwimController
    [SerializeField] float depth = 200f;
    [SerializeField] float extraWidth = 4f;

    SpriteRenderer sr;

    void Awake(){ if (!cam) cam = Camera.main; sr = GetComponent<SpriteRenderer>(); }

    void LateUpdate(){
        if (!cam) return;
        float halfH = cam.orthographicSize;
        float halfW = halfH * cam.aspect;
        float width = (halfW + extraWidth) * 2f;

        // Place so the panel's TOP edge lines up with surfaceY (pivot assumed center)
        transform.position = new Vector3(cam.transform.position.x, surfaceY - depth * 0.5f, 10f);
        transform.localScale = new Vector3(width, depth, 1f);
    }
}
