using UnityEngine;
using System.Reflection;

[RequireComponent(typeof(Enemy))]
public class EnemyDifficultyAdapter : MonoBehaviour
{
    [Header("Base Values")]
    public float baseSpeed = 5f;
    public float baseJumpForce = 10f;

    [Header("References")]
    public Enemy enemyScript;
    public Rigidbody2D rb;

    [Header("Debug")]
    public bool debugMode = false;

    private float actualSpeed;
    private float actualJumpForce;

    void Awake()
    {
        if (enemyScript == null) enemyScript = GetComponent<Enemy>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        ApplyDifficultySettings();
    }

    void Start()
    {
        ApplyDifficultySettings();
    }

    void ApplyDifficultySettings()
    {
        if (DifficultyManager.Instance != null)
        {
            DifficultySettings settings = DifficultyManager.Instance.GetCurrentSettings();
            actualSpeed = baseSpeed * settings.enemySpeedMultiplier;
            actualJumpForce = baseJumpForce * settings.enemyJumpForceMultiplier;

            if (debugMode)
            {
                Debug.Log($"{name}: Applied difficulty - Speed: {actualSpeed} ({settings.enemySpeedMultiplier}x), Jump: {actualJumpForce} ({settings.enemyJumpForceMultiplier}x)");
            }

            UpdateEnemyScript();
        }
        else
        {
            actualSpeed = baseSpeed;
            actualJumpForce = baseJumpForce;

            if (debugMode)
            {
                Debug.LogWarning($"{name}: DifficultyManager not found, using base values");
            }
        }
    }

    void UpdateEnemyScript()
    {
        if (enemyScript == null) return;

        System.Type enemyType = enemyScript.GetType();

        // Update speed field (walking enemies)
        FieldInfo speedField = enemyType.GetField("speed", BindingFlags.Public | BindingFlags.Instance);
        if (speedField != null && speedField.FieldType == typeof(float))
        {
            speedField.SetValue(enemyScript, actualSpeed);
            if (debugMode) Debug.Log($"{name}: Set 'speed' field to {actualSpeed}");
        }

        // Update chaseSpeed field (walking enemies)
        FieldInfo chaseSpeedField = enemyType.GetField("chaseSpeed", BindingFlags.Public | BindingFlags.Instance);
        if (chaseSpeedField != null && chaseSpeedField.FieldType == typeof(float))
        {
            chaseSpeedField.SetValue(enemyScript, actualSpeed);
            if (debugMode) Debug.Log($"{name}: Set 'chaseSpeed' field to {actualSpeed}");
        }

        // Update jumpForce field (jumping enemies)
        FieldInfo jumpField = enemyType.GetField("jumpForce", BindingFlags.Public | BindingFlags.Instance);
        if (jumpField != null && jumpField.FieldType == typeof(float))
        {
            jumpField.SetValue(enemyScript, actualJumpForce);
            if (debugMode) Debug.Log($"{name}: Set 'jumpForce' field to {actualJumpForce}");
        }
    }

    public float GetSpeed() => actualSpeed;
    public float GetJumpForce() => actualJumpForce;
    public void RefreshDifficulty() => ApplyDifficultySettings();
}
