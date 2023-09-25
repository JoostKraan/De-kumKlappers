using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool isSelected = false;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isSelected)
        {
            // Implement visual feedback for selected units (e.g., highlighting).
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        // Implement visual feedback for selected units (e.g., highlighting).
    }

    public void MoveTo(Vector3 targetPosition)
    {
        // Calculate the formation position based on the target position
        Vector3 formationPosition = targetPosition;

        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(formationPosition);
    }
}
