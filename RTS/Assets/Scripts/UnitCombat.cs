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
    public EnemyHealth enemyHealth;
    public float attackInterval = 5f;
    private bool canAttack = true;

    public Animator animator;

    private void Start()
    {
        myAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyUnit"))
        {
            if (enemyUnits.Contains(other.gameObject)) return;
            enemyUnits.Add(other.gameObject);
            focusUnit = other.gameObject;
            enemyHealth = focusUnit.GetComponent<EnemyHealth>();
            // If not already in combat, start attacking
            if (!inCombat)
            {
                inCombat = true;
                StartCoroutine(CombatRoutine());
            }
        }
        if (other.gameObject.CompareTag("Marker"))
        {
            animator.SetBool("Walking", false);
            animator.SetBool("Fighting", false);
            animator.SetBool("Idle", true);
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
            yield return null;
        }
    }

    void AttackEnemyUnit()
    {
        if (focusUnit != null)
        {
            if (enemyHealth != null && !enemyHealth.isDead)
            {
                // Attack the focused enemy unit if it's available and within attack range
                float distanceToEnemyUnit = Vector3.Distance(transform.position, focusUnit.transform.position);
                if (distanceToEnemyUnit <= meleeRange && canAttack)
                {
                    animator.SetBool("Idle", false);
                    animator.SetBool("Walking", false);
                    animator.SetBool("Fighting", true);

                    // Perform the attack
                    enemyHealth.TakeDamage(damagePerAttack);
                    Debug.Log("Dealing damage to enemy unit!");

                    // Start the attack cooldown
                    StartCoroutine(AttackCooldown());
                }
                else
                {
                    StartCoroutine(AttackCooldown());
                }
            }
            else
            {
                // If the focused unit is dead or null, select a new target
                SelectRandomEnemyUnit();
            }
        }
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackInterval);
        canAttack = true;
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
