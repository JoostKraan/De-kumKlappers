using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public bool isSelected = false;
    public GameObject Pijltje;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isSelected)
        {
            Pijltje.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                // Raycast to get the target position from the mouse click
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    // Move the unit to the clicked position
                    MoveTo(hit.point);
                }
            }
        }
        else
        {
            Pijltje.SetActive(false);
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;
        // Implement visual feedback for selected units (e.g., highlighting).
    }

    public void MoveTo(Vector3 targetPosition)
    {
        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(targetPosition);
    }
}
