using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource musicSource;
    public AudioClip backgroundMusic;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("MusicManager Started");

        if (musicSource != null)
        {
            Debug.Log($"AudioSource found: {musicSource.name}");
        }
        else
        {
            Debug.LogError("No AudioSource assigned!");
            return;
        }

        if (backgroundMusic != null)
        {
            Debug.Log($"Background music: {backgroundMusic.name}");
            musicSource.clip = backgroundMusic;
            musicSource.loop = true;
            musicSource.Play();
            Debug.Log("Music should be playing now!");
        }
        else
        {
            Debug.LogError("No background music assigned!");
        }
    }
}