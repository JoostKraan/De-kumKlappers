using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuildingHealth : MonoBehaviour
{
    public int health = 100;
    private EnemySpawner spawner;
    private void Start()
    {
        spawner = GetComponent<EnemySpawner>();
    }
    // Method to set the spawner reference
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

            // Check if spawner is not null before accessing WorkerList
            if (spawner != null)
            {
                foreach (GameObject worker in spawner.WorkerList)
                {
                    Destroy(worker);
                }
            }
        }
    }
}
