using System.Runtime.InteropServices;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public float damage = 1f;

    private void OnTriggerEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

        }
    }

    private void HandlePlayerTrigger(GameObject player)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        //reset velocity
        rb.linearVelocity = new Vector2(0f, rb.linearVelocityY);

        //reaply velocity
        rb.AddForce(Vector2.left * -5f, ForceMode2D.Impulse);
    }
}
