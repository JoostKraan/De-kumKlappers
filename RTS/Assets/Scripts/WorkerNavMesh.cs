using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerNavMesh : MonoBehaviour
{
    [Header("Toggles")]
    [SerializeField] private bool isGoingToHavestingPoint = false;
    [SerializeField] private bool isGoingToDeliveryPoint = false;
    [Header("Roles")]
    [SerializeField] private bool Miners = false;
    [SerializeField] private bool TreeHarvesters;
    [Header("Variables")]
    [SerializeField] private float timer = 5f;
    private bool isCountingDown = false;
    [SerializeField] private GameObject Mesh;
    [SerializeField] private int woodCollected = 0;
    [SerializeField] private Animator animator;
    private Gamemanager gamemanager;
    private MeshRenderer workerMesh;
    private NavMeshAgent navMeshAgent;
    public Transform treeHarvestingPoint;
    public Transform treeDeliveryPoint;
    private Transform minerHarvestingPoint;
    private Transform miningDeliveryPoint;

    public List<GameObject> treeToHarvest; // List of objects to check for proximity
    public List<GameObject> stoneToHarvest;
    public Transform referencePoint; // Reference point for distance calculation
    public GameObject closestTree;
    public GameObject closestStone;

    private void Start()
    {
        isGoingToDeliveryPoint = false;
        isGoingToHavestingPoint = true;
        gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<Gamemanager>();
        workerMesh = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        referencePoint = GetComponent<Transform>();

        GameObject[] treesWithTag = GameObject.FindGameObjectsWithTag("treeHarvestingPoint");
        treeToHarvest.AddRange(treesWithTag);
        GameObject[] stonesWithTag = GameObject.FindGameObjectsWithTag("minerHarvestingPoint");
        stoneToHarvest.AddRange(stonesWithTag);

        
    }
    private void Update()
    {
        if(TreeHarvesters)
        {
            closestTree = FindClosestTree();
            treeDeliveryPoint = GameObject.FindWithTag("treeDeliveryPoint").transform;
        }
        if(Miners)
        {
            closestStone = FindClosestStone();
            miningDeliveryPoint = GameObject.FindWithTag("miningDeliveryPoint").transform;
        }
        MoveBetweenPoints();
        if(isCountingDown)
        {
            if(isGoingToHavestingPoint)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    isGoingToHavestingPoint = false;
                    isGoingToDeliveryPoint = true;
                    Mesh.SetActive(true);
                }
            }
        }
        if(isGoingToDeliveryPoint)
        {
            isCountingDown = false;
        }
    }    
    private void MoveBetweenPoints()
    {
        if(isGoingToHavestingPoint)
        {
            if (TreeHarvesters)
            {
                navMeshAgent.destination = closestTree.transform.position;
            }
            if(Miners)
            {
                navMeshAgent.destination = closestStone.transform.position;
            }
        }
        if(isGoingToDeliveryPoint)
        {
            if (TreeHarvesters)
            {
                navMeshAgent.destination = treeDeliveryPoint.transform.position;
            }
            if (Miners)
            {
                navMeshAgent.destination = miningDeliveryPoint.transform.position;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (TreeHarvesters)
        {
            if (collision.collider.CompareTag("treeHarvestingPoint"))
            {
                print("colidded with forrest");
                timer = 5f;
                isCountingDown = true;
                Mesh.SetActive(false);
            }
            if (collision.collider.CompareTag("treeDeliveryPoint"))
            {
                gamemanager.wood += 5;
                isGoingToDeliveryPoint = false;
                isGoingToHavestingPoint = true;
            }
        }
        if (Miners)
        {
            if (collision.collider.CompareTag("minerHarvestingPoint"))
            {
                timer = 5f;
                isCountingDown = true;
                Mesh.SetActive(false);
            }
            if (collision.collider.CompareTag("miningDeliveryPoint"))
            {
                gamemanager.stone += 5;
                isGoingToDeliveryPoint = false;
                isGoingToHavestingPoint = true;
            }
        }
    }
    GameObject FindClosestTree()
    {
        GameObject closestTree = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject tree in treeToHarvest)
        {
            if (tree != null)
            {
                float distance = Vector3.Distance(referencePoint.position, tree.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTree = tree;
                }
            }
        }

        return closestTree;
    }
    GameObject FindClosestStone()
    {
        GameObject closestStone = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject stone in stoneToHarvest)
        {
            if (stone != null)
            {
                float distance = Vector3.Distance(referencePoint.position, stone.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestStone = stone;
                }
            }
        }

        return closestStone;
    }
}
