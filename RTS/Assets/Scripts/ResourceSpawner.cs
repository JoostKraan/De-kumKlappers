using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] resourcePrefabs; // Array of different resource prefabs
    public float spawnChance;

    [Header("Raycast Settings")]
    public float distanceBetweenChecks;
    public float heightCheck = 10f, rangecheck = 30f;
    public LayerMask layerMask;
    public Vector2 positivePosition, negativePosition;

    private void Start()
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
    }
}
