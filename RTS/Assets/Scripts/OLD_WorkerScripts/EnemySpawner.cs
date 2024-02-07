using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject workerPrefab;
    public Transform spawnLocation;
    private EnemyEconomy econemyManager;
    private BuildCost buildCost;
    public List<GameObject> WorkerList = new List<GameObject>();
    public int timesSpawned;
    public float timer;

    private void Start()
    {
        buildCost = GetComponent<BuildCost>();
        timer = 3f;
        econemyManager = GameObject.FindWithTag("EnemyManager").GetComponent<EnemyEconomy>();

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
        GameObject A =Instantiate(workerPrefab, spawnLocation.position, Quaternion.identity);
        WorkerList.Add(A);
        timer = 3f;
    }
}


