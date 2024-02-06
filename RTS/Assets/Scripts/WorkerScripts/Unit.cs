using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class Unit : MonoBehaviour
{
    public bool Enemy;
    public bool Ally;
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    public bool isSelected = false;
    public GameObject Arrow;
    public float timeToSpawn;
    public bool isMoving;
    [SerializeField] GameObject destinationPoint;
    private GameObject instantiatedDestinationPoint; // Track the instantiated destination point

    private static List<Unit> selectedAllies = new List<Unit>();

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Destination"))
        {
            Debug.Log("Collided");
            Destroy(other.gameObject);
        }
    }

    void RemoveObject()
    {
        if (instantiatedDestinationPoint != null)
        {
            Destroy(instantiatedDestinationPoint);
        }
    }

    void OnDrawGizmos()
    {
        // Draw the ray from the camera to the target position
        if (isSelected)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Camera.main.transform.position, targetPosition);
        }
    }

    void Update()
    {
        if (Ally)
        {
            // Check if Shift is held down
            if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
            {
                // Check for mouse click
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit))
                    {
                        // Check if the clicked object is an ally
                        Unit clickedUnit = hit.collider.GetComponent<Unit>();
                        if (clickedUnit != null && clickedUnit.Ally)
                        {
                            // Toggle selection for the clicked ally
                            clickedUnit.ToggleSelection();
                        }
                    }
                }
            }

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
                        RemoveObject();
                        instantiatedDestinationPoint = Instantiate(destinationPoint, targetPosition, Quaternion.identity);
                    }
                }

                // Draw a red line from the camera to the target position
                Debug.DrawLine(Camera.main.transform.position, targetPosition, Color.red);

                if (isMoving)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 5.0f);
                    if (transform.position == targetPosition)
                    {
                        isMoving = false;
                    }
                }
            }
            else
            {
                Arrow.SetActive(false);
            }
        }
    }

    public void SetSelected(bool selected)
    {
        isSelected = selected;

        if (isSelected)
        {
            Arrow.SetActive(true);
            selectedAllies.Add(this);
        }
        else
        {
            Arrow.SetActive(false);
            selectedAllies.Remove(this);
        }
    }

    public void MoveTo(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    // Additional method to clear the selection
    public static void ClearSelection()
    {
        foreach (Unit unit in selectedAllies)
        {
            unit.SetSelected(false);
        }
        selectedAllies.Clear();
    }

    // Method to toggle selection state
    public void ToggleSelection()
    {
        isSelected = !isSelected;

        if (isSelected)
        {
            Arrow.SetActive(true);
            selectedAllies.Add(this);
        }
        else
        {
            Arrow.SetActive(false);
            selectedAllies.Remove(this);
        }
    }
}
