using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilding : MonoBehaviour
{
    public GameObject[] buildingPrefabs; // Array of building prefabs
    public Transform buildArea; // Area where the enemy can build
    public float buildingRange = 5f; // Maximum distance from the center of the build area
    private EnemyEconomy closestEnemyEconomy;
    private EnemySpawner spawner;

    private bool isBuilding = false;
    private List<int> availableBuildingIndices = new List<int>();

    // Update is called once per frame
    void Update()
    {
        FindClosestEnemyEconomy();
        if (!isBuilding)
        {
            StartBuilding();
        }
        else
        {
            TryBuild();
        }
    }
    void FindClosestEnemyEconomy()
    {
        GameObject[] enemyManagers = GameObject.FindGameObjectsWithTag("EnemyManager");
        float shortestDistance = Mathf.Infinity;

        foreach (GameObject enemyManagerObj in enemyManagers)
        {
            EnemyEconomy enemyEconomy = enemyManagerObj.GetComponent<EnemyEconomy>();
            if (enemyEconomy != null)
            {
                float distanceToEnemyEconomy = Vector3.Distance(transform.position, enemyManagerObj.transform.position);
                if (distanceToEnemyEconomy < shortestDistance)
                {
                    shortestDistance = distanceToEnemyEconomy;
                    closestEnemyEconomy = enemyEconomy;
                }
            }
        }

        if (closestEnemyEconomy != null)
        {
            // You have found the closest EnemyEconomy, do something with it
            Debug.Log("Closest EnemyEconomy found: " + closestEnemyEconomy.gameObject.name);
        }
        else
        {
            Debug.LogWarning("No EnemyEconomy found in the scene with the 'EnemyManager' tag.");
        }
    }
    void StartBuilding()
    {
        if (availableBuildingIndices.Count == 0)
        {
            // All buildings have been built at least once, reset the available indices
            ResetAvailableBuildingIndices();
        }

        // Randomly choose a building type to construct from the available indices
        int randomIndex = Random.Range(0, availableBuildingIndices.Count);
        int selectedBuildingIndex = availableBuildingIndices[randomIndex];
        GameObject selectedBuildingPrefab = buildingPrefabs[selectedBuildingIndex];

        // Get the BuildingCosts script from the selected building prefab
        BuildCost buildingCosts = selectedBuildingPrefab.GetComponent<BuildCost>();

        // Check if the script is present before proceeding
        if (buildingCosts != null)
        {
            // Check if the AI can afford to build this building
            if (CanAffordBuilding(buildingCosts) == true)
            {
                // Instantiate the selected building prefab within the building range
                Vector3 randomPosition = GetRandomPositionInBuildArea();
                Quaternion randomRotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);

                GameObject spawnedBuilding = Instantiate(selectedBuildingPrefab, randomPosition, randomRotation);

                // Deduct the building costs from the economy
                closestEnemyEconomy.RemoveWood(buildingCosts.woodCost);
                closestEnemyEconomy.RemoveStone(buildingCosts.stoneCost);
                closestEnemyEconomy.RemoveIron(buildingCosts.ironCost);
                buildingRange += 5;
                spawner = spawnedBuilding.gameObject.GetComponent<EnemySpawner>();

                // Remove the selected index from availableBuildingIndices
                availableBuildingIndices.RemoveAt(randomIndex);

                // Set a cooldown or delay before the next building construction
                StartCoroutine(BuildCooldown());
            }
        }
        else
        {
            // Handle the case where the BuildingCosts script is not present on the prefab
            Debug.Log("BuildingCosts script not found on the selected building prefab.");
        }
    }

    private void ResetAvailableBuildingIndices()
    {
        // Reset availableBuildingIndices to include all building indices
        availableBuildingIndices.Clear();
        for (int i = 0; i < buildingPrefabs.Length; i++)
        {
            availableBuildingIndices.Add(i);
        }
    }

    public bool CanAffordBuilding(BuildCost Bc)
    {
        float remainingWood = closestEnemyEconomy.Wood - Bc.woodCost;
        float remainingStone = closestEnemyEconomy.Stone - Bc.stoneCost;
        float remainingIron = closestEnemyEconomy.Iron - Bc.ironCost;

        if (remainingWood >= 0 && remainingStone >= 0 && remainingIron >= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
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
