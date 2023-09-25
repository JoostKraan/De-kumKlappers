using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour
{
    public Vector3 formationOffset;  // Offset for formation positions
    public float formationSpacing = 2.0f;  // Spacing between units in the formation
    public bool Selected;
    public GameObject SelectUI;
    private NavMeshAgent navMeshAgent;
    private Vector3 targetPosition;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (Selected == false)
        {
            SelectUI.SetActive(false);
        }
        else
        {
            SelectUI.SetActive(true);
        }
        if (Input.GetMouseButtonDown(1) && Selected == true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Calculate the target position based on the click position
                targetPosition = hit.point;
                MoveToTargetPosition(targetPosition);
            }
        }
    }

    void MoveToTargetPosition(Vector3 target)
    {
        // Calculate the formation position based on the target position
        Vector3 formationPosition = target + formationOffset;

        // Set the destination for the NavMeshAgent
        navMeshAgent.SetDestination(formationPosition);
    }
}
