using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEditor.Search;
using UnityEngine;

public class MapTransition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundry;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;

    enum Direction { Up, Down, Left, Right }

    private void Awake()
    {
        confiner = FindAnyObjectByType<CinemachineConfiner2D>();
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         confiner.BoundingShape2D = mapBoundry;

    //     }
    // }

    // private void UpdatePlayerPosition(gameObject Player)
    // {
        
    // }
}
