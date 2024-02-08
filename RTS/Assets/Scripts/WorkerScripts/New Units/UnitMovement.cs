using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask ground;
    public LayerMask enemyBuilding;
    public float attackRange = 10f; // Attack range for the unit
    public GameObject nearestBuilding = null;
    private float nearestDistance = Mathf.Infinity;

    // Damage per attack and time between attacks
    public int damagePerAttack = 10;
    public float attackInterval = 5f;
    private float timeSinceLastAttack = 0f;

    // Flag to indicate if the building is being attacked
    public bool isAttackingBuilding = false;

    //public Animator animator;
    void Start()
    {
        myCam = Camera.main;
        myAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        // Track time since last attack
        timeSinceLastAttack += Time.deltaTime;

        FindNearestEnemyBuilding();
        if (Input.GetMouseButtonDown(1))
        {
          //  animator.SetBool("Idle", false);
          //  animator.SetBool("Fighting", false);
          //  animator.SetBool("Walking", true);
            RaycastHit hit;
            Ray ray = myCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                Vector3 destination = hit.point;
                myAgent.SetDestination(destination);
            }

            // Find nearest enemy building and start attacking it

        }

        // Attack enemy building if it's time and within range
        if (nearestBuilding != null && nearestDistance <= attackRange && timeSinceLastAttack >= attackInterval)
        {
         //   animator.SetBool("Idle", false);
         //  animator.SetBool("Walking", false);
         //   animator.SetBool("Fighting", true);
            AttackEnemyBuilding();
            timeSinceLastAttack = 0f; // Reset the timer
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
