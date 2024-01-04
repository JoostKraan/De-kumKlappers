using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScout : MonoBehaviour
{
    private NavMeshAgent agent;
    private Vector3 wanderPoint;
    public List<GameObject> UnitList; // Assuming you have a Unit class
    public float wanderRadius = 10f;
    public float wanderTimer = 5f;
    public float fleeDistance = 5f; // Distance at which the enemy starts fleeing
    public Vector3 startPostion;
    private bool isFleeing = false;

    // Start is called before the first frame update
    void Start()
    {
        startPostion = transform.position;
        UnitList = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        SetRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFleeing)
        {
            Fleeing();
        }
        else
        {
            UnitList.Clear();
            if (!agent.pathPending && agent.remainingDistance < 0.5f)
            {
                SetRandomDestination();
            }
        }
    }

    void SetRandomDestination()
    {
        wanderPoint = RandomNavSphere(transform.position, wanderRadius, -1);
        agent.SetDestination(wanderPoint);
    }

    void Fleeing()
    {
        if (UnitList.Count > 0)
        {
            Vector3 fleeDirection = transform.position - UnitList[0].transform.position;
            Vector3 fleePoint = startPostion;
            agent.SetDestination(fleePoint);
            if (Vector3.Distance(gameObject.transform.position, startPostion) < 0.1f)
            {
                isFleeing = false;
            }
        }
    }

    Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask)
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;
        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Unit"))
        {
            UnitList.Add(other.gameObject);
            isFleeing = true;
        }
    }
}
