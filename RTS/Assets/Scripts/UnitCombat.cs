using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitCombat : MonoBehaviour
{
    NavMeshAgent myAgent;
    public float meleeRange = 3f;
    public bool inCombat = false;
    // Damage per attack and time between attacks
    public int damagePerAttack = 10;
    public List<GameObject> enemyUnits = new List<GameObject>();
    public GameObject focusUnit;
    public float attackInterval = 5f;
    private float timeSinceLastAttack = 0f;

    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyUnit"))
        {
            // Add the enemy unit to the list
            enemyUnits.Add(other.gameObject);

            // If not already in combat, start attacking
            if (!inCombat)
            {
                inCombat = true;
                StartCoroutine(CombatRoutine());
            }
        }
    }

    IEnumerator CombatRoutine()
    {
        while (inCombat)
        {
            if (focusUnit != null)
            {
                AttackEnemyUnit();
            }

            // Wait for a short duration before checking for the next attack
            yield return new WaitForSeconds(0.1f);
        }
    }

    void AttackEnemyUnit()
    {
        if (focusUnit != null)
        {
            EnemyHealth enemyHealth = focusUnit.GetComponent<EnemyHealth>();

            if (enemyHealth != null && !enemyHealth.isDead)
            {
                // Attack the focused enemy unit if it's available and within attack range
                float distanceToEnemyUnit = Vector3.Distance(transform.position, focusUnit.transform.position);
                if (distanceToEnemyUnit <= meleeRange)
                {
                    // Check if it's time to attack based on attack interval
                    if (timeSinceLastAttack >= attackInterval)
                    {
                        // Perform the attack
                        enemyHealth.TakeDamage(damagePerAttack);
                        Debug.Log("Dealing damage to enemy unit!");

                        // Reset the attack timer
                        timeSinceLastAttack = 0f;
                    }
                }
                else
                {
                    // If the enemy unit is out of range, move towards it
                    myAgent.isStopped = false;
                    myAgent.SetDestination(focusUnit.transform.position);
                }
            }
            else
            {
                // If the focused unit is dead or null, select a new target
                SelectRandomEnemyUnit();
            }
        }
    }

    void SelectRandomEnemyUnit()
    {
        if (enemyUnits.Count > 0)
        {
            int randomIndex = Random.Range(0, enemyUnits.Count);
            focusUnit = enemyUnits[randomIndex];
            myAgent.destination = focusUnit.transform.position;
        }
        else
        {
            // If no enemy units are available, exit combat mode
            inCombat = false;
        }
    }
}
