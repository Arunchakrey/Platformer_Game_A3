using System.Runtime.InteropServices;
using UnityEngine;

public class TrapScript : MonoBehaviour
{
    public float bounceForce = 10f;
    public int damage = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            HandlePlayerBounce(collision.gameObject);
            Debug.Log("Same Tag as expected: Player");
        }
    }

    private void HandlePlayerBounce(GameObject player)
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        Animator anim = player.GetComponent<Animator>();
        Debug.Log("Got player Object");

        if (rb)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);

            //apply bounce force
            rb.AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
            Debug.Log("Jump!!!");
            anim.SetTrigger("jump");
            //parameter for animation


        }
    }

}