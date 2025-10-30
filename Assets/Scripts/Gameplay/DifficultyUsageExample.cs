using UnityEngine;

/// <summary>
/// Example script showing how to use the difficulty system in your game.
/// Attach this to player, enemies, or hazards to apply difficulty modifiers.
/// </summary>
public class DifficultyUsageExample : MonoBehaviour
{
    [Header("Example: Player Health")]
    [SerializeField] private float baseMaxHealth = 100f;
    private float modifiedMaxHealth;

    [Header("Example: Enemy")]
    [SerializeField] private float baseEnemySpeed = 5f;
    private float modifiedEnemySpeed;

    void Start()
    {
        // Example 1: Using DifficultyManager (Enum-based approach)
        if (DifficultyManager.Instance != null)
        {
            modifiedMaxHealth = DifficultyManager.Instance.ApplyHealthModifier(baseMaxHealth);
            modifiedEnemySpeed = DifficultyManager.Instance.ApplyEnemySpeedModifier(baseEnemySpeed);

            Debug.Log($"Player Health: {baseMaxHealth} -> {modifiedMaxHealth}");
            Debug.Log($"Enemy Speed: {baseEnemySpeed} -> {modifiedEnemySpeed}");
        }

        // Example 2: Using DifficultyManagerDataDriven (ScriptableObject approach)
        if (DifficultyManagerDataDriven.Instance != null)
        {
            float dataDriverHealth = DifficultyManagerDataDriven.Instance.ApplyHealthModifier(baseMaxHealth);
            float dataDrivenSpeed = DifficultyManagerDataDriven.Instance.ApplyEnemySpeedModifier(baseEnemySpeed);

            Debug.Log($"[Data-Driven] Player Health: {baseMaxHealth} -> {dataDriverHealth}");
            Debug.Log($"[Data-Driven] Enemy Speed: {baseEnemySpeed} -> {dataDrivenSpeed}");
        }
    }

    // Example: Apply difficulty to player health component
    void ApplyDifficultyToPlayerHealth()
    {
        // Assuming you have a PlayerHealth component
        // PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        // if (playerHealth != null && DifficultyManager.Instance != null)
        // {
        //     float modifiedHealth = DifficultyManager.Instance.ApplyHealthModifier(playerHealth.maxHealth);
        //     playerHealth.maxHealth = modifiedHealth;
        //     playerHealth.currentHealth = modifiedHealth;
        // }
    }

    // Example: Apply difficulty to enemy speed
    void ApplyDifficultyToEnemySpeed()
    {
        // Assuming you have an enemy movement script
        // EnemyMovement enemyMovement = GetComponent<EnemyMovement>();
        // if (enemyMovement != null && DifficultyManager.Instance != null)
        // {
        //     float modifiedSpeed = DifficultyManager.Instance.ApplyEnemySpeedModifier(enemyMovement.moveSpeed);
        //     enemyMovement.moveSpeed = modifiedSpeed;
        // }
    }

    // Example: Apply difficulty to hazard damage
    void ApplyDifficultyToHazard(float baseDamage)
    {
        float modifiedDamage = baseDamage;

        if (DifficultyManager.Instance != null)
        {
            modifiedDamage = DifficultyManager.Instance.ApplyHazardDamageModifier(baseDamage);
        }

        // Use modifiedDamage for your hazard
        Debug.Log($"Hazard Damage: {baseDamage} -> {modifiedDamage}");
    }
}
