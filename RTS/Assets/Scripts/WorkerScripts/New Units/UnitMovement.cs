using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask ground;
    public LayerMask enemyBuilding;
    public LayerMask playerUnits; // Layer mask for player units
    public float attackRange = 10f; // Attack range for the unit
    public GameObject nearestBuilding = null;
    private float nearestDistance = Mathf.Infinity;

    // Damage per attack and time between attacks
    public int damagePerAttack = 10;
    public float attackInterval = 5f;
    private float timeSinceLastAttack = 0f;

    // Flag to indicate if the unit is an enemy
    public bool isEnemy;

    // Flag to indicate if the unit is attacking
    public bool isAttacking = false;

    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Track time since last attack
        timeSinceLastAttack += Time.deltaTime;

        // Find nearest player unit and start attacking it if within range
        if (isEnemy && !isAttacking)
        {
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, attackRange, playerUnits);
            if (hitColliders.Length > 0)
            {
                // Attack the nearest player unit
                AttackPlayerUnit(hitColliders[0].gameObject);
            }
            else
            {
                // If no player units in range, find the nearest player unit and move towards it
                FindNearestPlayerUnitAndMove();
            }
        }

        // Attack enemy building if it's time and within range
        if (!isAttacking && nearestBuilding != null && nearestDistance <= attackRange && timeSinceLastAttack >= attackInterval)
        {
            AttackEnemyBuilding();
            timeSinceLastAttack = 0f; // Reset the timer
        }

        // Check if player is attempting to move
        if (!isEnemy && Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                Vector3 destination = hit.point;
                myAgent.SetDestination(destination);
            }
        }
    }

    void FindNearestPlayerUnitAndMove()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Mathf.Infinity, playerUnits);

        // Reset nearestDistance and nearestPlayerUnit
        float nearestPlayerDistance = Mathf.Infinity;
        GameObject nearestPlayerUnit = null;

        foreach (Collider col in hitColliders)
        {
            // Calculate distance
            float distance = Vector3.Distance(transform.position, col.transform.position);

            // Check if this player unit is nearer than the previous nearest one
            if (distance < nearestPlayerDistance)
            {
                nearestPlayerDistance = distance;
                nearestPlayerUnit = col.gameObject;
            }
        }

        // Move towards the nearest player unit if found
        if (nearestPlayerUnit != null)
        {
            myAgent.SetDestination(nearestPlayerUnit.transform.position);
        }
    }

    void FindNearestEnemyBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100, enemyBuilding);

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
    }

    void AttackEnemyBuilding()
    {
        isAttacking = true; // Set flag to true to prevent further attacks

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
                isAttacking = false;
            }
        }

        // Resume the NavMeshAgent's movement
        myAgent.isStopped = false;
    }

    void AttackPlayerUnit(GameObject playerUnit)
    {
        // Check if the player unit has health
        PlayerHealth playerHealth = playerUnit.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            // Deal damage to the player unit
            playerHealth.TakeDamage(damagePerAttack);
            Debug.Log("Dealing damage to player unit: " + playerUnit.name);
        }
    }
}
