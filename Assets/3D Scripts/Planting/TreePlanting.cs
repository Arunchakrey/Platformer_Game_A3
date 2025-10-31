using UnityEngine;

public class TreePlanting : MonoBehaviour
{
    [Header("References")]
    public GameObject treePrefab;
    public float plantingDistance = 2f; // How close the player needs to be

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
                // Check if a tree already exists at this spot
                if (spot.transform.childCount == 0)
                {
                    // Plant tree
                    GameObject newTree = Instantiate(treePrefab, spot.transform.position, Quaternion.identity);
                    newTree.transform.localScale = Vector3.one * 2.0f;
                    newTree.transform.parent = spot.transform; // Attach tree to the spot
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
