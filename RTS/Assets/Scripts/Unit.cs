using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Transform leader;  // The leader unit to follow
    public Vector3 offset;    // The offset position relative to the leader
    public float formationSpacing = 2.0f;  // Spacing between units in the formation

    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (leader == null)
        {
            return; // No leader assigned
        }

        // Calculate the target position based on the leader's position and the offset
        Vector3 desiredPosition = leader.position + leader.TransformDirection(offset);

        // Calculate the final position in the formation, considering the spacing
        Vector3 formationPosition = desiredPosition + (transform.position - desiredPosition).normalized * formationSpacing;

        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(formationPosition);
    }
}
