using UnityEngine;

public class SimpleCameraFollow : MonoBehaviour
{
    [Header("Follow Settings")]
    public Transform target;
    public Vector3 offset = new Vector3(0, 2, -8);
    public float smoothSpeed = 0.125f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // FORCE Z to be exactly -5 (after smoothing)
        smoothedPosition.z = -8f;

        transform.position = smoothedPosition;
    }
}