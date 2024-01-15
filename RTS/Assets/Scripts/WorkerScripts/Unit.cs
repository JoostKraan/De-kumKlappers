using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    private LineRenderer lineRenderer;
    public bool isSelected = false;
    public GameObject Arrow;
    public float timeToSpawn;
    public bool isMoving;
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2;
        lineRenderer.enabled = false;
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (isSelected)
        {
            Arrow.SetActive(true);
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    targetPosition = hit.point;
                    isMoving = true;
                    MoveTo(hit.point);
                    ShowLine();
                }
            }
            if (isMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5.0f);
                if (transform.position == targetPosition)
                {
                    isMoving = false;
                    lineRenderer.enabled = false;
                }
            }
        }
        else
        {
            Arrow.SetActive(false);
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

    public void ShowLine()
    {
        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, targetPosition);


        lineRenderer.enabled = true;
    }
}
