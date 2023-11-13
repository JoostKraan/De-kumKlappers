using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerNavMesh : MonoBehaviour
{
    [Header("Toggles")]
    [SerializeField] private bool isAtHarvesterSpot = false;
    [SerializeField] private bool isAtDeliveryPoint = false;
    [SerializeField] private bool isCollectingWood;
    [Header("Roles")]
    [SerializeField] private bool Miners = false;
    [SerializeField] private bool TreeHarvesters;
    [Header("Variables")]
    [SerializeField] private float timer = 5f;
    [SerializeField] private float woodCollectTimer = 0f;
    [SerializeField] private GameObject skinnedMeshRender;
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

    private void Awake()
    {
        //gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<Gamemanager>();
    }
    private void Start()
    {
        workerMesh = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        referencePoint = GetComponent<Transform>();

        GameObject[] treesWithTag = GameObject.FindGameObjectsWithTag("treeHarvestingPoint");
        treeToHarvest.AddRange(treesWithTag);
        GameObject[] stonesWithTag = GameObject.FindGameObjectsWithTag("minerHarvestingPoint");
        stoneToHarvest.AddRange(stonesWithTag);

        StartCoroutine(MoveBetweenPoints());
    }
    private void Update()
    {
        if(TreeHarvesters)
        {
            closestTree = FindClosestTree();
            treeHarvestingPoint = closestTree.transform;
            navMeshAgent.destination = treeHarvestingPoint.position;
            treeDeliveryPoint = GameObject.FindWithTag("treeDeliveryPoint").transform;
        }
        if(Miners)
        {
            closestStone = FindClosestStone();
            minerHarvestingPoint = closestStone.transform;
            navMeshAgent.destination = minerHarvestingPoint.position;
            miningDeliveryPoint = GameObject.FindWithTag("miningDeliveryPoint").transform;
        }
        if (isCollectingWood)
        {
            woodCollectTimer += Time.deltaTime;
            if (woodCollectTimer >= 5f) 
            {
                woodCollected++;
                woodCollectTimer = 0f; 
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
                        navMeshAgent.destination = treeDeliveryPoint.position;
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
                        navMeshAgent.destination = treeHarvestingPoint.position;
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
        skinnedMeshRender.SetActive(true);
        if (Miners && other.CompareTag("minerHarvestingPoint"))
        {
            timer = 5f;
            isAtHarvesterSpot = true;
            skinnedMeshRender.SetActive(false);
        }
        if (Miners && other.CompareTag("miningDeliveryPoint"))
        {
            timer = 5f;
            isAtHarvesterSpot = false;
            isAtDeliveryPoint = true;
        }

        if (TreeHarvesters && other.CompareTag("treeHarvestingPoint"))
        {
            timer = 5f;
            isCollectingWood = true;
            isAtHarvesterSpot = true;
            skinnedMeshRender.SetActive(false);
        }
        if (TreeHarvesters && other.CompareTag("treeDeliveryPoint"))
        {
            timer = 5f;
            isAtHarvesterSpot = false;
            isAtDeliveryPoint = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        //if ((Miners && (other.CompareTag("minerHarvestingPoint") || other.CompareTag("miningDeliveryPoint"))) ||
        //    (TreeHarvesters && (other.CompareTag("treeHarvestingPoint") || other.CompareTag("treeDeliveryPoint"))))
        //{
        //    woodCollectTimer = 0f;
        //    isCollectingWood = false;
        //    isAtHarvesterSpot = false;
        //    isAtDeliveryPoint = false;
        //    skinnedMeshRender.SetActive(false);
        //}
        if (TreeHarvesters && other.CompareTag("treeHarvestingPoint"))
        {
            isCollectingWood = false;
            isAtHarvesterSpot = false;
            skinnedMeshRender.SetActive(true);
        }
        if (Miners && other.CompareTag("stoneHarvestingPoint"))
        {
            isCollectingWood = false;
            isAtHarvesterSpot = false;
            skinnedMeshRender.SetActive(true);
        }
    }
}
