using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;
    public bool isSelected = false;
    public GameObject Arrow;
    public float timeToSpawn;
    public bool isMoving;
    [SerializeField] GameObject destinationPoint;


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
        Destroy(destinationPoint,10f);
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
                    ShowDestination();
                    RemoveObject();
                }
            }
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
   

    public void SetSelected(bool selected)
    {
        isSelected = selected;
    }

    public void MoveTo(Vector3 targetPosition)
    {
        navMeshAgent.SetDestination(targetPosition);
    }

    public void ShowDestination()
    {
        Instantiate(destinationPoint, targetPosition, Quaternion.identity);

    }
}
