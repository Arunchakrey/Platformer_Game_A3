using UnityEngine;

[CreateAssetMenu(fileName = "NewHazard", menuName = "Game Data/Hazard")]
public class HazardData : ScriptableObject
{
    [Header("Identity")]
    public string hazardName;
    public Sprite sprite;

    [Header("Damage")]
    public int damage = 1;
    public float damageInterval = 1f;
    public bool isInstantKill = false;

    [Header("Physics")]
    public float knockbackForce = 5f;
    public Vector2 knockbackDirection = Vector2.up;
    public float gravityScale = 1f;
    public float fallDelay = 0.5f;

    [Header("Effects")]
    public GameObject hitEffect;
    public AudioClip hitSound;
    public bool destroyOnHit = false;
}
