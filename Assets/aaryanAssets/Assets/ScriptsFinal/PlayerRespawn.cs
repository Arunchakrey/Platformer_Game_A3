using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public float killY = -10f; // Y position that means "fell off"
    public Vector3 spawnPosition = Vector3.zero;

    void Update()
    {
        if (transform.position.y < killY)
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        // Reset position
        transform.position = spawnPosition;

        // Reset velocity
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}