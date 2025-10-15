using UnityEngine;
public class Pickup : MonoBehaviour, IItem
{
    private PickupType type;
    private int value = 1;
    public void Collect()
    {
        // switch (type)
        // {
        //     case PickupType.Health:
        //         other.GetComponent<Health>().Heal(value);
        //         break;
        //     case PickupType.Sapling:
        //         other.GetComponent<PlayerInventory>().AddItem(value);
        //         break;
        //     case PickupType.Gem:
        //         other.GetComponent<PlayerInventory>().AddItem(value);
        //         break;
        // }
        Destroy(gameObject);
    }
}
