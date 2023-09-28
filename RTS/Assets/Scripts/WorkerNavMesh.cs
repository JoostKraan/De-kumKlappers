using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerNavMesh : MonoBehaviour
{
    public float timer = 5f;
    [SerializeField] private bool isAtHarvesterSpot = false;
    [SerializeField] private bool isAtDeliveryPoint = false;
    [SerializeField] private bool Miners = false;
    [SerializeField] private bool TreeHarvesters = false;
    [SerializeField] private Animator animator;
    private MeshRenderer workerMesh;
    private NavMeshAgent navMeshAgent;
    private Transform treeTransform;
    private Transform deliveryTransform;
    private Transform startingTransform; // Store the starting position
    private int woodCollected = 0;

    private void Awake()
    {
        workerMesh = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        treeTransform = GameObject.FindWithTag("treeTransform").GetComponent<Transform>();
        deliveryTransform = GameObject.FindWithTag("deliveryTransform").GetComponent<Transform>();

        if (TreeHarvesters)
        {
            navMeshAgent.destination = treeTransform.position;
            startingTransform = treeTransform; // Set the starting position for tree harvesters
        }
        else if (Miners)
        {
            // Set the starting position for miners
            startingTransform = transform; // You can change this to the miner's starting spot
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
                    navMeshAgent.destination = deliveryTransform.position;
                }
            }
            if (isAtDeliveryPoint)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    navMeshAgent.destination = startingTransform.position;
                }
            }
            yield return null;
        }
    }

    // OnTriggerEnter is called when the Collider enters the trigger
    private void OnTriggerEnter(Collider other)
    {
        workerMesh.enabled = false;
        if (other.gameObject.CompareTag("TreeHarvest"))
        {
            timer = 5f;
            isAtHarvesterSpot = true;
            workerMesh.enabled = false; // Disable the mesh renderer
        }
        if (other.gameObject.CompareTag("DeliveryPoint"))
        {
            timer = 5f;
            isAtDeliveryPoint = true;
        }
    }

    // OnTriggerExit is called when the Collider exits the trigger
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("TreeHarvest") || other.gameObject.CompareTag("DeliveryPoint"))
        {
            isAtHarvesterSpot = false;
            isAtDeliveryPoint = false;
            workerMesh.enabled = true; // Enable the mesh renderer
        }
    }
}
