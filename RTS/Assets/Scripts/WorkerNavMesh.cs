using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorkerNavMesh : MonoBehaviour
{
    [SerializeField] private Transform[] movePositionTransforms;
    [SerializeField] private float waitTime = 2f;
    bool cooldownRunning = false;
    Animator animator;
    
    private NavMeshAgent navMeshAgent;
    private int currentDestinationIndex = 0;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
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
            navMeshAgent.destination = movePositionTransforms[currentDestinationIndex].position;
            yield return new WaitForSeconds(waitTime);
            currentDestinationIndex = (currentDestinationIndex + 1) % movePositionTransforms.Length;
        }
    }
}