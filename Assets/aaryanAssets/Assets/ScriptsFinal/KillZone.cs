using UnityEngine;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ResetPlayer(other.gameObject);
        }
    }

    private void ResetPlayer(GameObject player)
    {
        // Option 1: Reset to spawn point
        PlayerRespawn respawn = player.GetComponent<PlayerRespawn>();
        if (respawn != null)
        {
            respawn.Respawn();
        }
        // Option 2: Simple position reset
        else
        {
            player.transform.position = Vector3.zero; // Or your start position
            player.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }
    }
}