using UnityEngine;

[CreateAssetMenu(fileName = "NewPickup", menuName = "Game Data/Pickup")]
public class PickupData : ScriptableObject
{
    [Header("Identity")]
    public string pickupName;
    public Sprite sprite;
    public PickupType type;

    [Header("Values")]
    public int pointsValue = 1;
    public int healthRestore = 0;
    public int materialCount = 1;

    [Header("Effects")]
    public GameObject collectEffect;
    public AudioClip collectSound;
    public bool destroyOnCollect = true;
    public float respawnTime = 0f;
}

public enum PickupType
{
    Points,
    Health,
    Material,
    Gem,
    Sapling,
    Pollution,
    Oxygen
}
