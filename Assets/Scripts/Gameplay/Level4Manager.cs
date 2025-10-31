using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Manages Level 4 game logic including win condition, brick spawning, and exit door.
/// </summary>
public class Level4Manager : MonoBehaviour
{
    [Header("Level Completion")]
    [Tooltip("GameObject representing the exit door")]
    [SerializeField] private GameObject exitDoor;

    [Tooltip("Sprite for closed door")]
    [SerializeField] private Sprite closedDoorSprite;

    [Tooltip("Sprite for open door")]
    [SerializeField] private Sprite openDoorSprite;

    [Tooltip("Scene to load on level completion")]
    [SerializeField] private string nextLevelScene = "MainMenu";

    [Tooltip("Delay before loading next scene")]
    [SerializeField] private float levelCompleteDelay = 2f;

    [Header("Brick Spawning")]
    [Tooltip("Spawn points for falling bricks")]
    [SerializeField] private Transform[] brickSpawnPoints;

    [Tooltip("Brick prefab to spawn")]
    [SerializeField] private GameObject brickPrefab;

    [Tooltip("Time between brick spawns")]
    [SerializeField] private float brickSpawnInterval = 3f;

    [Tooltip("Random variation in spawn timing")]
    [SerializeField] private float spawnIntervalVariation = 1f;

    [Tooltip("Enable automatic brick spawning")]
    [SerializeField] private bool enableBrickSpawning = true;

    [Header("Audio")]
    [SerializeField] private AudioClip doorOpenSound;
    [SerializeField] private AudioClip levelCompleteSound;

    private bool buildingComplete = false;
    private bool levelComplete = false;
    private float nextBrickSpawnTime;
    private SpriteRenderer doorRenderer;

    void Start()
    {
        // Setup exit door
        if (exitDoor != null)
        {
            doorRenderer = exitDoor.GetComponent<SpriteRenderer>();
            if (doorRenderer != null && closedDoorSprite != null)
            {
                doorRenderer.sprite = closedDoorSprite;
            }

            // Disable door trigger until building is complete
            Collider2D doorCollider = exitDoor.GetComponent<Collider2D>();
            if (doorCollider != null)
            {
                doorCollider.enabled = false;
            }
        }

        // Initialize brick spawning
        if (enableBrickSpawning)
        {
            ScheduleNextBrickSpawn();
        }

        Debug.Log("[Level4Manager] Level 4 initialized.");
    }

    void Update()
    {
        // Handle brick spawning
        if (enableBrickSpawning && !buildingComplete && Time.time >= nextBrickSpawnTime)
        {
            SpawnBrick();
            ScheduleNextBrickSpawn();
        }
    }

    /// <summary>
    /// Called when the building is completed.
    /// </summary>
    public void OnBuildingComplete()
    {
        if (buildingComplete)
            return;

        buildingComplete = true;
        Debug.Log("[Level4Manager] Building completed! Opening exit door.");

        // Stop brick spawning
        enableBrickSpawning = false;

        // Open the exit door
        OpenExitDoor();
    }

    private void OpenExitDoor()
    {
        if (exitDoor != null)
        {
            // Change door sprite
            if (doorRenderer != null && openDoorSprite != null)
            {
                doorRenderer.sprite = openDoorSprite;
            }

            // Enable door trigger
            Collider2D doorCollider = exitDoor.GetComponent<Collider2D>();
            if (doorCollider != null)
            {
                doorCollider.enabled = true;
                doorCollider.isTrigger = true;
            }

            // Play door open sound
            if (doorOpenSound != null && SoundEffectManager.Instance != null)
            {
                SoundEffectManager.Instance.PlaySound(doorOpenSound);
            }

            Debug.Log("[Level4Manager] Exit door opened!");
        }
    }

    private void SpawnBrick()
    {
        if (brickPrefab == null || brickSpawnPoints == null || brickSpawnPoints.Length == 0)
        {
            Debug.LogWarning("[Level4Manager] Cannot spawn brick: Missing prefab or spawn points.");
            return;
        }

        // Pick random spawn point
        Transform spawnPoint = brickSpawnPoints[Random.Range(0, brickSpawnPoints.Length)];

        // Spawn brick
        GameObject brick = Instantiate(brickPrefab, spawnPoint.position, Quaternion.identity);
        Debug.Log($"[Level4Manager] Spawned brick at {spawnPoint.position}");
    }

    private void ScheduleNextBrickSpawn()
    {
        float randomVariation = Random.Range(-spawnIntervalVariation, spawnIntervalVariation);
        nextBrickSpawnTime = Time.time + brickSpawnInterval + randomVariation;
    }

    /// <summary>
    /// Called when player enters the exit door.
    /// </summary>
    public void OnPlayerExitLevel()
    {
        if (!buildingComplete || levelComplete)
            return;

        levelComplete = true;
        Debug.Log("[Level4Manager] Player exited level. Loading next scene...");

        // Play level complete sound
        if (levelCompleteSound != null && SoundEffectManager.Instance != null)
        {
            SoundEffectManager.Instance.PlaySound(levelCompleteSound);
        }

        StartCoroutine(LoadNextSceneAfterDelay());
    }

    private IEnumerator LoadNextSceneAfterDelay()
    {
        yield return new WaitForSeconds(levelCompleteDelay);

        // Load next scene
        if (!string.IsNullOrEmpty(nextLevelScene))
        {
            SceneManager.LoadScene(nextLevelScene);
        }
        else
        {
            Debug.LogWarning("[Level4Manager] Next scene name is not set!");
        }
    }

    /// <summary>
    /// Public method to manually spawn a brick (can be called from Unity Events).
    /// </summary>
    public void ManualSpawnBrick()
    {
        SpawnBrick();
    }

    /// <summary>
    /// Toggle brick spawning on/off.
    /// </summary>
    public void SetBrickSpawning(bool enabled)
    {
        enableBrickSpawning = enabled;
        if (enabled)
        {
            ScheduleNextBrickSpawn();
        }
    }

    void OnDrawGizmos()
    {
        // Visualize brick spawn points
        if (brickSpawnPoints != null)
        {
            Gizmos.color = Color.red;
            foreach (Transform spawnPoint in brickSpawnPoints)
            {
                if (spawnPoint != null)
                {
                    Gizmos.DrawWireSphere(spawnPoint.position, 0.5f);
                    Gizmos.DrawLine(spawnPoint.position, spawnPoint.position + Vector3.down * 2f);
                }
            }
        }
    }
}
