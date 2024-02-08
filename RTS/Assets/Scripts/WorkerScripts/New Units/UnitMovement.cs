using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class UnitMovement : MonoBehaviour
{
    Camera myCam;
    NavMeshAgent myAgent;
    public LayerMask ground;
    public LayerMask enemyBuilding;

    public bool isEnemy;
   
    RaycastHit enemyHit;

  
    public int damagePerTick = 10;
    public float tickInterval = 5f;

    
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
               
                enemyHit = hit;
                StartCoroutine(AttackEnemyBuilding());
            }
        }
    }

    IEnumerator AttackEnemyBuilding()
    {
        isAttackingBuilding = true; 

    
        myAgent.isStopped = true;

     
        myAgent.SetDestination(enemyHit.point);

        Debug.Log("Attacking enemy building!");

       
        Debug.Log("Object hit: " + enemyHit.collider.gameObject.name);

       
        EnemyBuildingHealth enemyHealth = enemyHit.collider.GetComponent<EnemyBuildingHealth>();
        if (enemyHealth != null)
        {
           
            while (enemyHealth.health > 0)
            {
                
                enemyHealth.TakeDamage(damagePerTick);

                Debug.Log("Dealing damage to enemy building!");

                
                yield return new WaitForSeconds(tickInterval);
            }

           
            Debug.Log("Enemy building destroyed!");
        }

   
        myAgent.isStopped = false;

        isAttackingBuilding = false; 
    }
}
