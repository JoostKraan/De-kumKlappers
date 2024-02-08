using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyUnit : MonoBehaviour
{
    NavMeshAgent myAgent;
    public LayerMask ground;
    public LayerMask enemyBuilding;
    public float attackRange = 10f; // Attack range for the unit
    public GameObject nearestBuilding = null;
    private float nearestDistance = Mathf.Infinity;
    public bool inCombat = false;
    // Damage per attack and time between attacks
    public int damagePerAttack = 10;
    public List<GameObject> sqoud = new List<GameObject>();
    public List<GameObject> playerUnits = new List<GameObject>();
    public GameObject focustUnit;
    public float attackInterval = 5f;
    public EnemyHealth playerHealth;
    private float timeSinceLastAttack = 0f;

    // Flag to indicate if the building is being attacked
    public bool isAttackingBuilding = false;

    void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
        FindNearestEnemyBuilding();
    }

    void Update()
    {
        // Track time since last attack
        timeSinceLastAttack += Time.deltaTime;

        // Attack enemy building if it's time and within range
        if (nearestBuilding != null && nearestDistance <= attackRange && timeSinceLastAttack >= attackInterval)
        {
            AttackEnemyBuilding();
            timeSinceLastAttack = 0f; // Reset the timer
        }
        FindNearestEnemyBuilding();

    }

    void FindNearestEnemyBuilding()
    {
        if (!inCombat)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, 500, enemyBuilding);
            // Reset nearestDistance and nearestBuilding
            nearestDistance = Mathf.Infinity;
            nearestBuilding = null;
            foreach (Collider col in hitColliders)
            {
                // Calculate distance
                float distance = Vector3.Distance(transform.position, col.transform.position);

                // Check if this building is nearer than the previous nearest one
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestBuilding = col.gameObject;
                }
            }
            if (nearestBuilding != null && sqoud.Count >= 5)
            {
                myAgent.destination = nearestBuilding.transform.position;
            }
            else
            {
                myAgent.destination = gameObject.transform.position;
            }
        }
        else
        {
            if (playerUnits.Count > 0)
            {
                int randomIndex = Random.Range(0, playerUnits.Count);
                 focustUnit = playerUnits[randomIndex].gameObject;
                myAgent.destination = focustUnit.transform.position;
                playerHealth = focustUnit.GetComponent<EnemyHealth>();
                AttackPlayerUnit();
            }
            else
            {
                Debug.LogError("No player units available.");
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyUnit"))
        {
            if (sqoud.Contains(other.gameObject)) return;
            sqoud.Add(other.gameObject);
        }
        if (other.CompareTag("PlayerUnit"))
        {
            if (playerUnits.Contains(other.gameObject)) return;
            playerUnits.Add(other.gameObject);
            inCombat = true;
        }
    }
    void AttackPlayerUnit()
    {
        if (playerHealth.isDead == false)
        {
            // Attack the focused player unit if it's available and within attack range
            float distanceToPlayerUnit = Vector3.Distance(transform.position, focustUnit.transform.position);
            if (distanceToPlayerUnit <= attackRange)
            {
                // Check if it's time to attack based on attack interval
                if (timeSinceLastAttack >= attackInterval)
                {
                    // Perform the attack
                  
                    if (playerHealth != null)
                    {
                        playerHealth.TakeDamage(damagePerAttack);
                        Debug.Log("Dealing damage to player unit!");
                    }

                    // Reset the attack timer
                    timeSinceLastAttack = 0f;
                }
            }
            else
            {
                // If the player unit is out of range, move towards it
                myAgent.destination = focustUnit.transform.position;
            }
        }
        else
        {
            int randomIndex = Random.Range(0, playerUnits.Count);
            focustUnit = playerUnits[randomIndex].gameObject;
            myAgent.destination = focustUnit.transform.position;
        }
    }

    public void AttackEnemyBuilding()
    {
        isAttackingBuilding = true; // Set flag to true to prevent further attacks

        // Pause the NavMeshAgent's movement
        myAgent.isStopped = true;

        EnemyBuildingHealth enemyHealth = nearestBuilding.GetComponent<EnemyBuildingHealth>();
        if (enemyHealth != null)
        {
            // Deal damage to the enemy building
            enemyHealth.TakeDamage(damagePerAttack);
            Debug.Log("Dealing damage to enemy building!");

            // Check if the building is destroyed
            if (enemyHealth.health <= 0)
            {
                Debug.Log("Enemy building destroyed!");
                isAttackingBuilding = false;
            }
        }

        // Resume the NavMeshAgent's movement
        myAgent.isStopped = false;
    }
}
