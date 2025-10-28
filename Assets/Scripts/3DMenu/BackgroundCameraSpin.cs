using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform target; // assign BG_IslandRoot
    public float radius = 9f;
    public float height = 2.5f;
    public float speed = 8f;
    private float angle;

    void Start()
    {
        if (!target) return;
        Vector3 dir = (transform.position - target.position);
        angle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;
    }

    // Update is called once per frame
    void Update()
    {
        if (!target) return;
        angle += speed * Time.deltaTime;
        float rad = angle + Mathf.Deg2Rad;
        Vector3 pos = target.position + new Vector3(Mathf.Cos(rad) * radius, height, Mathf.Sin(rad) * radius);
        transform.position = pos;
        transform.LookAt(target.position + Vector3.up * 0.8f);
    }
}
