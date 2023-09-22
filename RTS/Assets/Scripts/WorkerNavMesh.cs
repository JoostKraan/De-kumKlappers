using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerNavMesh : MonoBehaviour
{
    [SerializeField] private bool Miners = false;
    [SerializeField] private bool TreeHarvesters = false;
    [SerializeField] Animator animator;

    private NavMeshAgent navMeshAgent;
    private Transform[] targetTransforms;
    private int currentDestinationIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Automatically assign tags based on boolean variables
        if (Miners)
        {
            targetTransforms = FindObjectsWithTag("Miner");
        }
        else if (TreeHarvesters)
        {
            targetTransforms = FindObjectsWithTag("TreeHarvest");
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
            // Check if targetTransforms is empty to avoid errors
            if (targetTransforms != null && targetTransforms.Length > 0)
            {
                navMeshAgent.destination = targetTransforms[currentDestinationIndex].position;
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
                yield return null; // Remove the WaitForSeconds to remove the cooldown
                currentDestinationIndex = (currentDestinationIndex + 1) % targetTransforms.Length;
            }
            yield return null;
        }
    }

    // Helper function to find objects with a specific tag
    private Transform[] FindObjectsWithTag(string tag)
    {
        GameObject[] targetObjects = GameObject.FindGameObjectsWithTag(tag);
        Transform[] targetTransforms = new Transform[targetObjects.Length];

        for (int i = 0; i < targetObjects.Length; i++)
        {
            targetTransforms[i] = targetObjects[i].transform;
        }

        return targetTransforms;
    }
}
