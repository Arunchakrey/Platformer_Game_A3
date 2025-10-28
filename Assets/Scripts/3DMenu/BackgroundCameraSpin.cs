using UnityEngine;

/// <summary>
/// Minecraft-style menu camera orbit:
/// Orbits gently around a fixed world position (no target object required),
/// with subtle noise sway and vertical bob.
/// Attach this to your Main Camera.
/// </summary>
[ExecuteAlways]
public class MenuCameraOrbitFixed : MonoBehaviour
{
    [Header("Orbit Center (world position)")]
    public Vector3 orbitCenter = new Vector3(4.655f, 1.346f, 1.872f);

    [Header("Orbit Motion")]
    public float radius = 8f;            // distance from center
    public float heightOffset = 2.5f;    // eye height above center
    public float orbitSpeedDegPerSec = 6f;

    [Header("Camera Sway / Bob")]
    public float yawNoiseDegrees = 8f;   // horizontal sway
    public float pitchNoiseDegrees = 4f; // vertical tilt sway
    public float noiseSpeed = 0.25f;     // how fast noise changes
    public float bobAmplitude = 0.2f;
    public float bobSpeed = 0.4f;

    [Header("FOV Pulse (optional)")]
    public bool fovPulse = true;
    public float baseFOV = 55f;
    public float fovPulseAmplitude = 1.5f;
    public float fovPulseSpeed = 0.15f;

    private float angleDeg;
    private Camera cam;

    void OnEnable()
    {
        cam = GetComponent<Camera>();
        if (cam && fovPulse)
            cam.fieldOfView = baseFOV;
    }

    void Update()
    {
        // --- 1) Orbit position around fixed center ---
        angleDeg += orbitSpeedDegPerSec * Time.deltaTime;
        float rad = angleDeg * Mathf.Deg2Rad;

        float bob = Mathf.Sin(Time.time * Mathf.PI * 2f * bobSpeed) * bobAmplitude;
        Vector3 pos = orbitCenter + new Vector3(Mathf.Cos(rad) * radius,
                                                heightOffset + bob,
                                                Mathf.Sin(rad) * radius);
        transform.position = pos;

        // --- 2) Look back at the center (Minecraft-style) ---
        Vector3 dir = (orbitCenter - pos).normalized;
        Quaternion baseRot = Quaternion.LookRotation(dir, Vector3.up);

        // --- 3) Add Perlin noise sway (no roll) ---
        float t = Time.time * noiseSpeed;
        float yawNoise = (Mathf.PerlinNoise(t, 0.0f) - 0.5f) * 2f * yawNoiseDegrees;
        float pitchNoise = (Mathf.PerlinNoise(0.0f, t) - 0.5f) * 2f * pitchNoiseDegrees;
        Quaternion noiseRot = Quaternion.Euler(pitchNoise, yawNoise, 0f);

        transform.rotation = baseRot * noiseRot;

        // --- 4) Gentle FOV pulse ---
        if (fovPulse && cam)
            cam.fieldOfView = baseFOV + Mathf.Sin(Time.time * Mathf.PI * 2f * fovPulseSpeed) * fovPulseAmplitude;
    }
}
