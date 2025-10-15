using UnityEngine;

public class FallingBrickSensor : MonoBehaviour
{
    public FallingBrick brick; // assign parent

    void Reset() { GetComponent<Collider2D>().isTrigger = true; }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (brick) brick.SensorEnter(other);
    }
}
