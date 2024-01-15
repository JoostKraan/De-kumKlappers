using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWorkerSpawner : MonoBehaviour
{
    public BuildingPlacement buildingPlacement;
    public GameObject worker;
    public Transform spawnLocation;

    public int timesSpawned;
    public float timer;

    private void Start()
    {
            buildingPlacement = GameObject.Find("BuildingManager").GetComponent<BuildingPlacement>();
        timer = 3f;
    }
    void Update()
    {
        if(buildingPlacement.pendingPrefab == null)
        {
            timer -= Time.deltaTime;
            if (timesSpawned == 0)
            {
                SpawnWorker();
            }
            if (timesSpawned == 1)
            {
                SpawnWorker();
            }
            if (timesSpawned == 2)
            {
                SpawnWorker();
            }
        }        
    }
    public void SpawnWorker()
    {
        if(timer <= 0 && buildingPlacement.pendingPrefab == null)
        {
            timesSpawned++;
            Instantiate(worker, spawnLocation.position, Quaternion.identity);
            timer = 3f;
        }
    }
}