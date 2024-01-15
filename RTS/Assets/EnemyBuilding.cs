using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilding : MonoBehaviour
{
    public GameObject[] buildingPrefabs; // Array of building prefabs
    public Transform buildArea; // Area where the enemy can build
    public float buildingRange = 5f; // Maximum distance from the center of the build area

    private bool isBuilding = false;

    // Update is called once per frame
    void Update()
    {
        if (!isBuilding)
        {
            StartBuilding();
        }
        else
        {
            TryBuild();
        }
    }

    void StartBuilding()
    {
        // Randomly choose a building type to construct
        int randomIndex = Random.Range(0, buildingPrefabs.Length);
        GameObject selectedBuildingPrefab = buildingPrefabs[randomIndex];

        // Instantiate the selected building prefab within the building range
        Vector3 randomPosition = GetRandomPositionInBuildArea();
        Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

        Instantiate(selectedBuildingPrefab, randomPosition, randomRotation);

        // Set a cooldown or delay before the next building construction
        StartCoroutine(BuildCooldown());
    }

    void TryBuild()
    {
        // Check if the build area is clear before starting a new building
        Collider[] colliders = Physics.OverlapBox(buildArea.position, buildArea.localScale / 2);
        if (colliders.Length == 0)
        {
            isBuilding = false;
        }
    }

    Vector3 GetRandomPositionInBuildArea()
    {
        // Get a random position within the specified building range
        Vector2 randomOffset = Random.insideUnitCircle * buildingRange;
        Vector3 randomPosition = buildArea.position + new Vector3(randomOffset.x, 0, randomOffset.y);
        return randomPosition;
    }

    IEnumerator BuildCooldown()
    {
        // Set a cooldown or delay before the next building construction
        isBuilding = true;
        yield return new WaitForSeconds(5f); // You can adjust the cooldown duration
        isBuilding = false;
    }
}
