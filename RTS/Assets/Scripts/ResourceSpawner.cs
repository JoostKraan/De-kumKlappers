using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] resourcePrefabs;
    public GameObject orePrefab;
    public float spawnChance;
    public float oreSpawnChance;
    public GameObject treePrefab;
    public float treeSpawnChance;

    [Header("Raycast Settings")]
    public float distanceBetweenChecks;
    public float oreDistanceBetweenChecks;
    public float heightCheck = 10f, rangecheck = 30f;
    public Vector2 positivePosition, negativePosition;

    [Header("Layers")]
    public LayerMask layerMask;
    public LayerMask forrestSpawningLayerMask;
    public LayerMask stoneSpawningLayerMask;

    private void Awake()
    {
        SpawnResources();
    }

    void SpawnResources()
    {
        for (float x = negativePosition.x; x < positivePosition.x; x += distanceBetweenChecks)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += distanceBetweenChecks)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightCheck, z), Vector3.down, out hit, rangecheck, layerMask))
                {
                    if (spawnChance > Random.Range(0, 101))
                    {
                        // Randomly select a prefab from the array
                        GameObject randomPrefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)];

                        Instantiate(randomPrefab, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                    }
                }
            }
        }
        for (float x = negativePosition.x; x < positivePosition.x; x += oreDistanceBetweenChecks)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += oreDistanceBetweenChecks)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightCheck, z), Vector3.down, out hit, rangecheck, stoneSpawningLayerMask))
                {
                    if (oreSpawnChance > Random.Range(0, 101))
                    {
                        Quaternion rotation = Quaternion.Euler(new Vector3(-90, 0, 0)); // Fixed rotation of 90 degrees on the Y-axis
                        Instantiate(orePrefab, hit.point, rotation, transform);
                    }
                }
            }
        }
        for (float x = negativePosition.x; x < positivePosition.x; x += oreDistanceBetweenChecks)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += oreDistanceBetweenChecks)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightCheck, z), Vector3.down, out hit, rangecheck, forrestSpawningLayerMask))
                {
                    if (treeSpawnChance > Random.Range(0, 101))
                    {
                        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                        Instantiate(treePrefab, hit.point, rotation, transform);
                    }
                }
            }
        }
    }
}
