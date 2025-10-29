using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class DataDrivenEnemy : MonoBehaviour
{
    [Header("Data Reference")]
    public EnemyData enemyData;
    public string enemyDataName;

    [Header("Auto References")]
    private Enemy enemyScript;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    void Awake()
    {
        enemyScript = GetComponent<Enemy>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (string.IsNullOrEmpty(enemyDataName) == false && GameDatabase.Instance != null)
        {
            enemyData = GameDatabase.Instance.GetEnemy(enemyDataName);
        }

        ApplyData();
    }

    void ApplyData()
    {
        if (enemyData == null) return;

        if (enemyScript != null)
        {
            enemyScript.damage = Mathf.RoundToInt(enemyData.damage * GetDifficultyMultiplier());
        }

        if (spriteRenderer != null && enemyData.sprite != null)
        {
            spriteRenderer.sprite = enemyData.sprite;
        }

        if (animator != null && enemyData.animatorController != null)
        {
            animator.runtimeAnimatorController = enemyData.animatorController;
        }

        var walk = GetComponent<EnemyWalk>();
        if (walk != null)
        {
            walk.chaseSpeed = enemyData.moveSpeed * GetDifficultyMultiplier();
        }

        var jump = GetComponent<EnemyJump>();
        if (jump != null)
        {
            jump.jumpForce = enemyData.jumpForce * GetDifficultyMultiplier();
        }
    }

    float GetDifficultyMultiplier()
    {
        if (DifficultyManagerDataDriven.Instance != null)
        {
            return DifficultyManagerDataDriven.Instance.GetEnemySpeedMultiplier();
        }
        return 1f;
    }

    public EnemyData GetData() => enemyData;
}
