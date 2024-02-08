using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    public GameObject workerPrefab;
    public Transform spawnLocation;
    public EnemyEconomy closestEnemyEconomy;
    private BuildCost buildCost;
    public List<GameObject> WorkerList = new List<GameObject>();
    public int timesSpawned;
    public float timer;

    private void Start()
    {
        buildCost = GetComponent<BuildCost>();
        timer = 3f;
        FindClosestEnemyEconomy();

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


