using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    public bool isSelected = false;
    public GameObject Pijltje;
    public float timeToSpawn;

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
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
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
    }

    public void MoveTo(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }
}
