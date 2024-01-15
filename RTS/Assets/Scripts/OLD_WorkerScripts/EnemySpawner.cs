using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject workerPrefab;
    public Transform spawnLocation;

    public int timesSpawned;
    public float timer;

    private void Start()
    {
        timer = 3f;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timesSpawned < 3 && timer <= 0)
        {
            SpawnWorker();
        }
    }

    public void SpawnWorker()
    {
        timesSpawned++;
        Instantiate(workerPrefab, spawnLocation.position, Quaternion.identity);
        timer = 3f;
    }
}


