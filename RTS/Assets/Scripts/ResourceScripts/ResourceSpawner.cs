using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    public GameObject[] resourcePrefabs;
    public DebrisClass[] DebrisPrefabs;
    public GameObject orePrefab;
    public GameObject treePrefab;
    public GameObject ironPrefab;
    public float spawnChance;
    public float oreSpawnChance;

    public float treeSpawnChance;
    public float ironSpawnChance;

    [Header("Raycast Settings")]    
    public float distanceBetweenChecks;
    public float oreDistanceBetweenChecks;
    public float ironoreDistanceBetweenChecks;
    public float heightCheck = 10f, rangecheck = 30f;
    public Vector2 positivePosition, negativePosition;

    [Header("Layers")]
    public LayerMask layerMask;
    public LayerMask forrestSpawningLayerMask;
    public LayerMask stoneSpawningLayerMask;
    public LayerMask ironSpawningLayerMask;

    private void Awake()
    {
        SpawnResources();
    }

    public DebrisClass getValFromWeightTable()
    {
        int sum = 0;
        for (int i = 0; i < DebrisPrefabs.Length; i++)
        {
            sum += DebrisPrefabs[i].Weight;
        }

        sum = Random.Range(0, sum);

        for (int i = 0; i < DebrisPrefabs.Length; i++) {
            sum -= DebrisPrefabs[i].Weight;

            if (sum <= 0) {
                return DebrisPrefabs[i];
            }
        }

        return null;
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
                        float randomScale = Random.Range(1.0f, 2.5f);

                        // Randomly select a prefab from the array
                        // GameObject randomPrefab = resourcePrefabs[Random.Range(0, resourcePrefabs.Length)]; [OLD]
                        DebrisClass randomPrefab = getValFromWeightTable();

                        Instantiate(randomPrefab.DebrisObject, hit.point, Quaternion.Euler(new Vector3(0, Random.Range(0, 360), 0)), transform);
                        randomPrefab.DebrisObject.transform.localScale = new Vector3(randomScale, randomScale, randomScale);
                    }
                }
            }
        }

        //stone spawning
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

        //Forest spawning
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

        //iron spawning
        for (float x = negativePosition.x; x < positivePosition.x; x += ironoreDistanceBetweenChecks)
        {
            for (float z = negativePosition.y; z < positivePosition.y; z += ironoreDistanceBetweenChecks)
            {
                RaycastHit hit;
                if (Physics.Raycast(new Vector3(x, heightCheck, z), Vector3.down, out hit, rangecheck, ironSpawningLayerMask))
                {
                    if (ironSpawnChance > Random.Range(0, 101))
                    {
                        Quaternion rotation = Quaternion.Euler(new Vector3(-90, 0, 0));
                        Instantiate(ironPrefab, hit.point, rotation, transform);
                    }
                }
            }
        }
    }
}
