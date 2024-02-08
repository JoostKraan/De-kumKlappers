using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrainingCamp : MonoBehaviour
{
    public List<GameObject> workerPrefabs = new List<GameObject>();
    public Transform spawnLocation;
    public EnemyEconomy closestEnemyEconomy;
    private BuildCost buildCost;
    public List<GameObject> WorkerList = new List<GameObject>();
    public float timer;
    public float startTime;

    private void Start()
    {
        buildCost = GetComponent<BuildCost>();
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

        if (timer < 0)
        {
            SpawnWorker();
        }
    }

    public void SpawnWorker()
    {
        timer = startTime;
        if (closestEnemyEconomy.Iron > 0)
        {

            if (workerPrefabs.Count > 0)
            {
                int random_number = new System.Random().Next(0, workerPrefabs.Count);
                GameObject A = Instantiate(workerPrefabs[random_number], spawnLocation.position, Quaternion.identity);
                WorkerList.Add(A);
                closestEnemyEconomy.Iron -= 10;
              
            }
            else
            {
                Debug.LogError("No worker prefabs available to spawn.");
            }
        }
        else
        {
            //Debug.Log("Not enougf iron");
        }
    }

}




