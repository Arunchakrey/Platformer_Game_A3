// using System.Collections.Generic;
// using System.Linq;
// using UnityEngine;
// using UnityEngine.Tilemaps;

// public class ObjectSpawner : MonoBehaviour
// {
//     public enum ObjectType { Sapling, Enemy }

//     public Tilemap tilemap;
//     public GameObject[] objectPrefabs;
//     private List<Vector3> validSpawnPositions = new List<Vector3>();
//     private List<GameObject> spawnObjects = new List<GameObject>();
//     private bool isSpawning = false;
//     // Start is called once before the first execution of Update after the MonoBehaviour is created
//     void Start()
//     {

//     }

//     // Update is called once per frame
//     void Update()
//     {

//     }

//     private bool PositionHasObject(Vector3 positionToCheck)
//     {
//         return spawnObjects.Any(checkObj => checkObj && Vector3.Distance(checkObj.transform.position, positionToCheck) < 1.0f);
//     }

//     private IEnumerator SpawnObjectsIfNeeded()
//     {

//     }
//     private void GatherValidPositions()
//     {
//         validSpawnPositions.Clear();
//         BoundsInt boundsInt = tilemap.cellBounds;
//         TileBase[] allTiles = tilemap.GetTilesBlock(boundsInt);
//         Vector3 start = tilemap.CellToWorld(new Vector3Int(boundsInt.xMin, boundsInt.yMin, 0));

//         for (int x = 0; x < boundsInt.size.x; x++)
//         {
//             for (int y = 0; y < boundsInt.size.y; y++)
//             {
//                 TileBase tile = allTiles[x + y * boundsInt.size.x];
//                 if (tile != null)
//                 {
//                     Vector3 place = start + new Vector3(x + 0.5f, y + 2f, 0);
//                     validSpawnPositions.Add(place);
//                 }
//             }
//         }

//     }
// }
