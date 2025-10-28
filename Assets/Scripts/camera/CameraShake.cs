using System.Collections;
using UnityEngine;
using Unity.Cinemachine;

public class CameraShake : MonoBehaviour
{
    [SerializeField] private CinemachineCamera vcam;   // drag your CMCam here
    private CinemachineBasicMultiChannelPerlin noise;
    private Coroutine routine;

    void Awake()
    {
        if (!vcam) vcam = FindObjectOfType<CinemachineCamera>();
        noise = vcam.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitude = 2f, float frequency = 3f, float duration = 0.25f)
    {
        if (routine != null) StopCoroutine(routine);
        routine = StartCoroutine(DoShake(amplitude, frequency, duration));
    }

    private IEnumerator DoShake(float amp, float freq, float dur)
    {
        noise.AmplitudeGain = amp;
        noise.FrequencyGain = freq;

        float t = 0f;
        while (t < dur) { t += Time.unscaledDeltaTime; yield return null; }

        noise.AmplitudeGain = 0f;
        noise.FrequencyGain = 0f;
        routine = null;
    }
}
