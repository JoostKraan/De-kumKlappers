using System.Collections;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

public class RandomNavMeshMovementWithClick : MonoBehaviour
{
    public NavMeshAgent agent;

    private void Start()
    {
        // Initialize the NavMeshAgent
        agent = GetComponent<NavMeshAgent>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Cast a ray from the mouse click position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                agent.destination = hit.point;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a wireframe box in the scene view to visualize the movement bounds
        Gizmos.color = Color.yellow;
    }
}
