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
    [SerializeField] private bool TreeHarvesters = false;
    [SerializeField] private bool ironMiner = false;
    [Header("Variables")]
    [SerializeField] private float timer = 5f;
    private bool isCountingDown = false;
    [SerializeField] private GameObject Mesh;
    [SerializeField] private Animator animator;
    private Gamemanager gamemanager;
    private MeshRenderer workerMesh;
    private NavMeshAgent navMeshAgent;
    public Transform treeHarvestingPoint;
    public Transform treeDeliveryPoint;
    private Transform minerHarvestingPoint;
    private Transform miningDeliveryPoint;
    private Transform ironMiningHarvestingPoint;
    private Transform ironMiningDeliveryPoint;

    public List<GameObject> treeToHarvest; // List of objects to check for proximity
    public List<GameObject> stoneToHarvest;
    public List<GameObject> ironToHarvest;
    public Transform referencePoint; // Reference point for distance calculation
    public GameObject closestTree;
    public GameObject closestStone;
    public GameObject closetIron;
    private Transform spawnPoinr;

    public GameObject myHarvestingSpot;
    private void Start()
    {
        spawnPoinr = gameObject.transform;
        isGoingToDeliveryPoint = false;
        isGoingToHavestingPoint = true;
        gamemanager = GameObject.FindWithTag("Gamemanager").GetComponent<Gamemanager>();
        workerMesh = GetComponent<MeshRenderer>();
        animator = Mesh.GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        referencePoint = GetComponent<Transform>();

        GameObject[] treesWithTag = GameObject.FindGameObjectsWithTag("treeHarvestingPoint");
        treeToHarvest.AddRange(treesWithTag);
        GameObject[] stonesWithTag = GameObject.FindGameObjectsWithTag("minerHarvestingPoint");
        stoneToHarvest.AddRange(stonesWithTag);
        GameObject[] ironWithTag = GameObject.FindGameObjectsWithTag("ironMinerHarvestingPoint");
        ironToHarvest.AddRange(ironWithTag);       
    }
    private void Update()
    {
        if (TreeHarvesters)
        {
            closestTree = FindClosestTree();
            //treeToHarvest.Remove(closestTree);
            treeDeliveryPoint = spawnPoinr.transform;
        }
        if (Miners)
        {
            closestStone = FindClosestStone();
            //stoneToHarvest.Remove(closestStone);
            treeDeliveryPoint = spawnPoinr.transform;
        }
        if (ironMiner)
        {
            closetIron = FindClosestIron();
            myHarvestingSpot = closetIron;
            //ironToHarvest.Remove(closetIron);
            treeDeliveryPoint = spawnPoinr.transform;
        }

        MoveBetweenPoints();
        if(isCountingDown)
        {
            animator.SetBool("idle", false);
            animator.SetBool("Walking", false);
            if(Miners || ironMiner)
            {
                animator.SetBool("Mining", true);
            }
            else if (TreeHarvesters)
            {
                animator.SetBool("Chopping", true);
            }
            if (isGoingToHavestingPoint)
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
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", true);
            if (TreeHarvesters)
            {
                navMeshAgent.destination = closestTree.transform.position;
            }
            if(Miners)
            {
                navMeshAgent.destination = closestStone.transform.position;
            }
            if (ironMiner)
            {
                navMeshAgent.destination = myHarvestingSpot.transform.position;

                //ironToHarvest.Remove(closetIron);
            }
        }
        if(isGoingToDeliveryPoint)
        {
            animator.SetBool("Idle", false);
            animator.SetBool("Walking", true);
            if (TreeHarvesters)
            {
                navMeshAgent.destination = treeDeliveryPoint.transform.position;
            }
            if (Miners)
            {
                navMeshAgent.destination = miningDeliveryPoint.transform.position;
            }
            if (ironMiner)
            {
                navMeshAgent.destination = ironMiningDeliveryPoint.transform.position;
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
        if (ironMiner)
        {
            if (collision.collider.CompareTag("ironMinerHarvestingPoint"))
            {
                timer = 5f;
                isCountingDown = true;
                Mesh.SetActive(false);
            }
            if (collision.collider.CompareTag("ironMiningDeliveryPoint"))
            {
                gamemanager.iron += 5;
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
    GameObject FindClosestIron()
    {
        GameObject closestiron = null;
        float closestDistance = Mathf.Infinity;

        foreach (GameObject iron in ironToHarvest)
        {
            if (iron != null)
            {
                float distance = Vector3.Distance(referencePoint.position, iron.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestiron = iron;
                }
            }
        }

        return closestiron;
    }
}
