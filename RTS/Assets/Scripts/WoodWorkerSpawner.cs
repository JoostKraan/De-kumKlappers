using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodWorkerSpawner : MonoBehaviour
{
    public Selection selection;
    public GameObject worker;
    public Transform spawnLocation;

    public int timesSpawned;
    public float timer;
    void Update()
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
    public void SpawnWorker()
    {
        if(timer <= 0 && selection.selectedObject == null)
        {
            timesSpawned++;
            Instantiate(worker, spawnLocation.position, Quaternion.identity);
            timer = 3f;
        }       
    }
}
