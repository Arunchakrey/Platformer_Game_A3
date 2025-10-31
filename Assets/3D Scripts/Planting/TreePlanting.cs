using UnityEngine;

public class TreePlanting : MonoBehaviour
{
    [Header("References")]
    public GameObject treePrefab;
    public float plantingDistance = 2f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            TryPlantTree();
        }
    }

    void TryPlantTree()
    {
        // Find all PlantSpots in the scene
        GameObject[] spots = GameObject.FindGameObjectsWithTag("PlantSpot");

        foreach (GameObject spot in spots)
        {
            float distance = Vector3.Distance(transform.position, spot.transform.position);

            if (distance <= plantingDistance)
            {
                // Check if PlantSpot already has a tree (more than 2 children = has tree)
                if (spot.transform.childCount <= 2) // Only has Quad + TreeAnchor, no trees yet
                {
                    // Find TreeAnchor child for proper positioning
                    Transform treeAnchor = spot.transform.Find("TreeAnchor");
                    Vector3 plantPosition = treeAnchor ? treeAnchor.position : spot.transform.position;
                    
                    // Plant tree
                    GameObject newTree = Instantiate(treePrefab, plantPosition, Quaternion.identity);
                    newTree.transform.localScale = Vector3.one * 2.0f;
                    newTree.transform.parent = spot.transform;
                    Debug.Log("Tree planted at " + spot.name);
                }
                else
                {
                    Debug.Log("Tree already planted here!");
                }

                return; // Stop after planting one tree
            }
        }

        Debug.Log("No planting spot nearby!");
    }
}