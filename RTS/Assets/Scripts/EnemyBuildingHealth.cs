using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuildingHealth : MonoBehaviour
{
    public int health = 100;
    private EnemyEconomy closestEnemyEconomy;
    private EnemySpawner spawner;
    private void Start()
    {
        spawner = GetComponent<EnemySpawner>();
        FindClosestEnemyEconomy();
    }

    public void SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
    }
    private void Update()
    {
        if (health <= 0)
        {
            Destroy(gameObject);
            closestEnemyEconomy.health -= 100;


            if (spawner != null)
            {
                foreach (GameObject worker in spawner.WorkerList)
                {
                    Destroy(worker);
                }
            }
        }
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
}
