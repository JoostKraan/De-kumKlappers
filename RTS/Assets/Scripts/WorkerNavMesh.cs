using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerNavMesh : MonoBehaviour
{
    [SerializeField] private string firstTargetTag = "FirstTag";
    [SerializeField] private string secondTargetTag = "SecondTag";
    [SerializeField] private float waitTime = 2f;
    [SerializeField] bool cooldownRunning = false;
    Animator animator;

    private NavMeshAgent navMeshAgent;
    private Transform[] firstTargetTransforms;
    private Transform[] secondTargetTransforms;
    private int currentDestinationIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        // Find objects with the first tag
        GameObject[] firstTargetObjects = GameObject.FindGameObjectsWithTag(firstTargetTag);
        // Find objects with the second tag
        GameObject[] secondTargetObjects = GameObject.FindGameObjectsWithTag(secondTargetTag);
        // Initialize the arrays with the transforms of the found objects
        firstTargetTransforms = new Transform[firstTargetObjects.Length];
        secondTargetTransforms = new Transform[secondTargetObjects.Length];

        for (int i = 0; i < firstTargetObjects.Length; i++)
        {
            firstTargetTransforms[i] = firstTargetObjects[i].transform;
        }

        for (int i = 0; i < secondTargetObjects.Length; i++)
        {
            secondTargetTransforms[i] = secondTargetObjects[i].transform;
        }
    }

    private void Update()
    {
        // Remove this part if it's not needed
        // It sets the "isRunning" parameter of the animator to false when cooldownRunning is true
        if (cooldownRunning)
        {
            animator.SetBool("isRunning", false);
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
            // Determine which set of targets to use based on the currentDestinationIndex
            Transform[] currentTargets;
            if (currentDestinationIndex % 2 == 0)
            {
                currentTargets = firstTargetTransforms;
            }
            else
            {
                currentTargets = secondTargetTransforms;
            }

            // Check if currentTargets is empty to avoid errors
            if (currentTargets.Length > 0)
            {
                navMeshAgent.destination = currentTargets[currentDestinationIndex % currentTargets.Length].position;
                // Calculate the distance to the target position
                float distance = Vector3.Distance(transform.position, navMeshAgent.destination);
                // If the distance is less than a small threshold, set "isRunning" to false
                if (distance < 0.1f)
                {
                    animator.SetBool("isRunning", false);
                }
                else
                {
                    animator.SetBool("isRunning", true);
                }
                yield return new WaitForSeconds(waitTime);
                currentDestinationIndex++;
            }
            yield return null;
        }
    }
}
