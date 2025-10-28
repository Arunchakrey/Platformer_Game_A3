using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;

    private static AudioSource audioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private Slider sfxSlider;
    private static Coroutine footstepCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            audioSource = GetComponent<AudioSource>();
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    // Play footsteps
    public static void PlayFootsteps(string soundName, float minDelay = 0.45f, float maxDelay = 0.65f)
    {
        // Only start if not already running
        if (footstepCoroutine == null)
        {
            footstepCoroutine = Instance.StartCoroutine(Instance.FootstepRoutine(soundName, minDelay, maxDelay));
        }
    }

    public static void StopFootsteps()
    {
        if (footstepCoroutine != null)
        {
            Instance.StopCoroutine(footstepCoroutine);
            footstepCoroutine = null;
        }
    }

    void Start()
    {
        // Disable keyboard navigation on the slider
        var nav = sfxSlider.navigation;
        nav.mode = Navigation.Mode.None;
        sfxSlider.navigation = nav;

        sfxSlider.onValueChanged.AddListener(delegate { OnValueChanged(); });
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void OnValueChanged()
    {
        SetVolume(sfxSlider.value);
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator FootstepRoutine(string soundName, float minDelay, float maxDelay)
    {
        while (true)
        {
            AudioClip clip = soundEffectLibrary.GetRandomClip(soundName);
            if (clip != null)
            {
                // Random pitch adds realism
                audioSource.pitch = Random.Range(0.95f, 1.05f);
                audioSource.PlayOneShot(clip);
            }

            // Pause between steps (slower, more natural)
            float wait = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(wait);
        }
    }
}
