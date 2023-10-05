using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerNavMesh : MonoBehaviour
{
    [Header("Toggles")]
    [SerializeField] private bool isAtHarvesterSpot = false;
    [SerializeField] private bool isAtDeliveryPoint = false;
    [Header("Roles")]
    [SerializeField] private bool Miners = false;
    [SerializeField] private bool TreeHarvesters;
    [Header("Variables")]
    [SerializeField] private float timer = 5f;
    [SerializeField] private int woodCollected = 0;
    [SerializeField] private Animator animator;
    private MeshRenderer workerMesh;
    private NavMeshAgent navMeshAgent;
    private Transform treeTransform;
    private Transform deliveryTransform;
    private Transform minerHarvestingPoint; 
    private Transform miningDeliveryPoint;  

    private void Awake()
    {
        workerMesh = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        treeTransform = GameObject.FindWithTag("treeTransform").GetComponent<Transform>();
        deliveryTransform = GameObject.FindWithTag("deliveryTransform").GetComponent<Transform>();

        // Add references to miner-specific points
        minerHarvestingPoint = GameObject.FindWithTag("minerHarvestingPoint").GetComponent<Transform>();
        miningDeliveryPoint = GameObject.FindWithTag("miningDeliveryPoint").GetComponent<Transform>();

        if (TreeHarvesters)
        {
            navMeshAgent.destination = treeTransform.position;
        }
        else if (Miners)
        {
            navMeshAgent.destination = minerHarvestingPoint.position;
        }
    }

    private void Start()
    {
        StartCoroutine(MoveBetweenPoints());
    }

    private IEnumerator MoveBetweenPoints()
    {
        while (true)
        {
            if (isAtHarvesterSpot)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TreeHarvesters)
                    {
                        navMeshAgent.destination = deliveryTransform.position;
                    }
                    else if (Miners)
                    {
                        navMeshAgent.destination = miningDeliveryPoint.position;
                    }
                }
            }
            if (isAtDeliveryPoint)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TreeHarvesters)
                    {
                        navMeshAgent.destination = treeTransform.position;
                    }
                    else if (Miners)
                    {
                        navMeshAgent.destination = minerHarvestingPoint.position;
                    }
                }
            }
            yield return null;
        }
    }

   
    private void OnTriggerEnter(Collider other)
    {
        workerMesh.enabled = false;
        if (Miners && other.CompareTag("minerHarvestingPoint"))
        {
            timer = 5f;
            isAtHarvesterSpot = true;
            workerMesh.enabled = false; 
        }
        if (Miners && other.CompareTag("miningDeliveryPoint"))
        {
            timer = 5f;
            isAtDeliveryPoint = true;
        }

        if (TreeHarvesters && other.CompareTag("treeTransform"))
        {
            timer = 5f;
            isAtHarvesterSpot = true;
            workerMesh.enabled = false; 
        }
         if (TreeHarvesters && other.CompareTag("deliveryTransform"))
        {
            timer = 5f;
            isAtDeliveryPoint = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if ((Miners && (other.CompareTag("minerHarvestingPoint") || other.CompareTag("miningDeliveryPoint"))) ||
            (TreeHarvesters && (other.CompareTag("TreeHarvest") || other.CompareTag("DeliveryPoint"))))
        {
            isAtHarvesterSpot = false;
            isAtDeliveryPoint = false; 
        }

        
        workerMesh.enabled = true; 
    }

}
