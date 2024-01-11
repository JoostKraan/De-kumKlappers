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
    public Transform myDeliveryPoint;
    public GameObject myHarvestingSpot;
    [SerializeField] private Animator animator;
    private Gamemanager gamemanager;
    private MeshRenderer workerMesh;
    private NavMeshAgent navMeshAgent;

    public List<GameObject> treeToHarvest; // List of objects to check for proximity
    public List<GameObject> stoneToHarvest;
    public List<GameObject> ironToHarvest;
    public Transform referencePoint; // Reference point for distance calculation
    public GameObject closestTree;
    public GameObject closestStone;
    public GameObject closetIron;

    public float distanceThreshold = 5.0f;
    private void Start()
    {
        isGoingToDeliveryPoint = false;
        isGoingToHavestingPoint = true;
        Transform currentPosition = this.gameObject.transform;
        myDeliveryPoint = currentPosition;
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
        //find harvesting and delivery points
        if (TreeHarvesters)
        {
            closestTree = FindClosestTree();
            myHarvestingSpot = closestTree;
        }
        if (Miners)
        {
            closestStone = FindClosestStone();
            myHarvestingSpot = closestStone;
        }
        if (ironMiner)
        {
            closetIron = FindClosestIron();
            myHarvestingSpot = closetIron;
        }

        MoveBetweenPoints();
        if (isCountingDown)
        {
            animator.SetBool("idle", false);
            animator.SetBool("Walking", false);
            if (Miners || ironMiner)
            {
                animator.SetBool("Mining", true);

                Transform childTransform = myHarvestingSpot.transform.Find("MiningLocation");
                gameObject.transform.position = childTransform.position;
            }
            else if (TreeHarvesters)
            {
                animator.SetBool("Chopping", true);
            }
        }

        if (isGoingToDeliveryPoint)
        {
            isCountingDown = false;
        }

        //harvesting point distance check
        float distanceFromH = Vector3.Distance(gameObject.transform.position, myHarvestingSpot.transform.position);
        if (distanceFromH < distanceThreshold)
        {
            if (TreeHarvesters)
            {
                //timer = 5f;
                timer-= Time.deltaTime;
                if(timer <=0)
                {
                    isGoingToHavestingPoint = false;
                    isGoingToDeliveryPoint = true;
                }
            }
            if (Miners)
            {
                //timer = 5f;
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    isGoingToHavestingPoint = false;
                    isGoingToDeliveryPoint = true;
                }
            }
            if (ironMiner)
            {
                //timer = 5f;
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    isGoingToHavestingPoint = false;
                    isGoingToDeliveryPoint = true;
                }
            }
        }

        //delivery point distance check
        float distanceFromD = Vector3.Distance(gameObject.transform.position, myDeliveryPoint.transform.position);
        if (distanceFromD < distanceThreshold)
        {
            if (ironMiner)
            {
                gamemanager.iron += 5;
                isGoingToDeliveryPoint = false;
                isGoingToHavestingPoint = true;
            }
            if (Miners)
            {
                gamemanager.stone += 5;
                isGoingToDeliveryPoint = false;
                isGoingToHavestingPoint = true;              
            }
            if (TreeHarvesters)
            {
                gamemanager.wood += 5;
                isGoingToDeliveryPoint = false;
                isGoingToHavestingPoint = true;
            }
        }
    }

        //walking function
        private void MoveBetweenPoints()
        {
            if (isGoingToHavestingPoint)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
                if (TreeHarvesters)
                {
                    navMeshAgent.destination = myHarvestingSpot.transform.position;
                }
                if (Miners)
                {
                    navMeshAgent.destination = myHarvestingSpot.transform.position;
                }
                if (ironMiner)
                {
                    navMeshAgent.destination = myHarvestingSpot.transform.position;

                    //ironToHarvest.Remove(closetIron);
                }
            }
            if (isGoingToDeliveryPoint)
            {
                animator.SetBool("Idle", false);
                animator.SetBool("Walking", true);
                if (TreeHarvesters)
                {
                    navMeshAgent.destination = myDeliveryPoint.transform.position;
                }
                if (Miners)
                {
                    navMeshAgent.destination = myDeliveryPoint.transform.position;
                }
                if (ironMiner)
                {
                    navMeshAgent.destination = myDeliveryPoint.transform.position;
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
