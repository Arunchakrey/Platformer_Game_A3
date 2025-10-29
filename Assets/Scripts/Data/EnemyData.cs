using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Game Data/Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Identity")]
    public string enemyName;
    public Sprite sprite;
    public RuntimeAnimatorController animatorController;

    [Header("Stats")]
    public int maxHealth = 1;
    public int damage = 1;
    public float moveSpeed = 2f;
    public float jumpForce = 8f;

    [Header("Behavior")]
    public float detectionRange = 5f;
    public float attackRange = 1f;
    public float attackCooldown = 2f;
    public bool canJump = false;
    public bool canFly = false;

    [Header("Drops")]
    public int pointsOnDeath = 10;
    public GameObject[] possibleDrops;
    public float dropChance = 0.3f;
}
