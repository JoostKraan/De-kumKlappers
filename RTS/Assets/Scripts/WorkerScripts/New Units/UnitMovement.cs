using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    [Header("Layers")]
    public LayerMask ground;
    public LayerMask enemyBuilding;
    [Header("Toggles")]
    public bool isEnemy;
    public bool isAttackingBuilding = false;
    [Header("Stats")]
    public int damagePerTick = 10;
    public float tickInterval = 5f;
    public float attackRange = 10f;
    public int damagePerAttack = 10;
    public float attackInterval = 5f;
    [Header("Miscellaneous")]
    public GameObject nearestBuilding = null;
    private float nearestDistance = Mathf.Infinity;
    
   
   
    
    
    private float timeSinceLastAttack = 0f;
    RaycastHit enemyHit;
    

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

            if (!isAttackingBuilding && Physics.Raycast(ray, out hit, Mathf.Infinity, enemyBuilding))
            {
               
                Debug.Log("Clicked on an enemy!");

             
                if (nearestDistance <= attackRange)
                {
                    hit.transform.GetChild(0).gameObject.SetActive(true);

                    enemyHit = hit;
                    StartCoroutine(AttackEnemy());
                }
                else
                {
                    Debug.Log("Enemy is out of range!");

               
                    MoveTowardsEnemyBuilding(hit.point);
                }
            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                // Clicked on the ground
                Debug.Log("Clicked on the ground!");
                if (enemyHit.collider != null)
                {
                    enemyHit.collider.transform.GetChild(0).gameObject.SetActive(false);
                }
                StopAttack(); 
                myAgent.SetDestination(hit.point);
            }
        }

        FindNearestEnemyBuilding();

        timeSinceLastAttack += Time.deltaTime;
    }

    void MoveTowardsEnemyBuilding(Vector3 enemyPosition)
    {
       
        myAgent.SetDestination(enemyPosition);
    }


    void StopAttack()
    {
     
        isAttackingBuilding = false;
        StopAllCoroutines(); 
        myAgent.isStopped = false; 
    }


    void FindNearestEnemyBuilding()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100, enemyBuilding);

       
        nearestDistance = Mathf.Infinity;
        nearestBuilding = null;

        foreach (Collider col in hitColliders)
        {
           
            float distance = Vector3.Distance(transform.position, col.transform.position);

            
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestBuilding = col.gameObject;
            }
        }

      
        if (nearestBuilding != null && nearestDistance <= attackRange)
        {
            if (timeSinceLastAttack >= attackInterval)
            {
                AttackEnemyBuildingCoroutine();
                timeSinceLastAttack = 0f; 
            }
        }
    }

    IEnumerator AttackEnemyBuildingCoroutine()
    {
        isAttackingBuilding = true; 

        myAgent.isStopped = true;

        EnemyBuildingHealth enemyHealth = nearestBuilding.GetComponent<EnemyBuildingHealth>();
        if (enemyHealth != null)
        {
         
            enemyHealth.TakeDamage(damagePerAttack);
            Debug.Log("Dealing damage to enemy building!");

            
            if (enemyHealth.health <= 0)
            {
                Debug.Log("Enemy building destroyed!");
                isAttackingBuilding = false;
            }
        }

       
        myAgent.isStopped = false;

        yield return null; 
    }

    IEnumerator AttackEnemy()
    {
        isAttackingBuilding = true;
        myAgent.isStopped = true;
        myAgent.SetDestination(enemyHit.point);
        Debug.Log("Attacking enemy unit!");
        Debug.Log("Object hit: " + enemyHit.collider.gameObject.name);
        EnemyHealth enemyHealth = enemyHit.collider.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            while (enemyHealth.currentHealth > 0)
            {
                enemyHealth.TakeDamage(damagePerTick);
                Debug.Log("Dealing damage to enemy unit!");
                yield return new WaitForSeconds(tickInterval);
            }
            Debug.Log("Enemy unit destroyed!");
        }
        myAgent.isStopped = false;
        isAttackingBuilding = false;
    }
}
