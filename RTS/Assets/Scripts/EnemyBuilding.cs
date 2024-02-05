using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilding : MonoBehaviour
{
    public GameObject[] buildingPrefabs; // Array of building prefabs
    public Transform buildArea; // Area where the enemy can build
    public float buildingRange = 5f; // Maximum distance from the center of the build area
    private EnemyEconomy enemyEconomy;
    private EnemySpawner spawner;

    private bool isBuilding = false;
    private List<int> availableBuildingIndices = new List<int>();

    // Update is called once per frame
    void Update()
    {
        enemyEconomy = GameObject.FindWithTag("EnemyManager").GetComponent<EnemyEconomy>();
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
                enemyEconomy.RemoveWood(buildingCosts.woodCost);
                enemyEconomy.RemoveStone(buildingCosts.stoneCost);
                enemyEconomy.RemoveIron(buildingCosts.ironCost);
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
        float remainingWood = enemyEconomy.Wood - Bc.woodCost;
        float remainingStone = enemyEconomy.Stone - Bc.stoneCost;
        float remainingIron = enemyEconomy.Iron - Bc.ironCost;

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
