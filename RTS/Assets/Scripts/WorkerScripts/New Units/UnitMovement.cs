using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask ground;
    public LayerMask enemyBuilding;

    // Store the RaycastHit for enemy building here
    RaycastHit enemyHit;

    // Damage per tick and interval between ticks
    public int damagePerTick = 10;
    public float tickInterval = 5f;

    // Flag to indicate if the building is being attacked
    public bool isAttackingBuilding = false;

    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                myAgent.SetDestination(hit.point);
            }
            if (!isAttackingBuilding && Physics.Raycast(ray, out hit, Mathf.Infinity, enemyBuilding))
            {
                // Store the RaycastHit for enemy building
                enemyHit = hit;
                StartCoroutine(AttackEnemyBuilding());
            }
        }
    }

    IEnumerator AttackEnemyBuilding()
    {
        isAttackingBuilding = true; // Set flag to true to prevent further attacks

        // Pause the NavMeshAgent's movement
        myAgent.isStopped = true;

        // Set destination to the enemy building position
        myAgent.SetDestination(enemyHit.point);

        Debug.Log("Attacking enemy building!");

        // Print the name of the object hit by the raycast
        Debug.Log("Object hit: " + enemyHit.collider.gameObject.name);

        // Assuming the enemy building has a script with a health value
        // You need to access that script to decrease its health
        // For example, if the script is called EnemyBuildingHealth, and it has a public method TakeDamage(int amount)
        // You can do something like this:
        EnemyBuildingHealth enemyHealth = enemyHit.collider.GetComponent<EnemyBuildingHealth>();
        if (enemyHealth != null)
        {
            // Continue dealing damage until the enemy building's health reaches zero
            while (enemyHealth.health > 0)
            {
                // Deal damage per tick
                enemyHealth.TakeDamage(damagePerTick);

                Debug.Log("Dealing damage to enemy building!");

                // Wait for the tick interval before dealing damage again
                yield return new WaitForSeconds(tickInterval);
            }

            // Enemy building destroyed or health reached zero
            Debug.Log("Enemy building destroyed!");
        }

        // Resume the NavMeshAgent's movement
        myAgent.isStopped = false;

        isAttackingBuilding = false; // Reset flag after the building is destroyed
    }
}
