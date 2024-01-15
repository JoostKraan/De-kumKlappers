using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour
{
    EconomyManager economymanager;
    Gamemanager gamemanager;

    public GameObject[] buildingPrefabs;
    public List<GameObject> ActiveBuildings = new List<GameObject>();

    public GameObject pendingPrefab;
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] private Material[] materials;

    private Vector3 pos;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    public float gridSize;
    public float rotationAmount;
    public bool canPlace;

    private void Start()
    {
        economymanager = GameObject.FindObjectOfType<EconomyManager>();
        gamemanager = GameObject.FindObjectOfType<Gamemanager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
        {
            CancelPlacement();
        }

        if (pendingPrefab != null)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                pendingPrefab.transform.position = new Vector3(
                    RoundToNearestGrid(pos.x),
                    RoundToNearestGrid(pos.y),
                    RoundToNearestGrid(pos.z)
                );
            } else
            {
                pendingPrefab.transform.position = pos;
            }

            if (Input.GetMouseButtonDown(0) && canPlace)
            {
                PlaceObject();
            }
            if (Input.GetKeyDown(KeyCode.R)) 
            {
                RotateObject();    
            }
            UpdateMaterials();
        }

        if (navMesh)
        {
            navMesh.BuildNavMesh();
        }
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, 1000, layerMask))
        {
            pos = hit.point;
        }
    }

    public void SelectObject(int index)
    {
        // Assuming EconomyManager and GameManager are assigned in the Inspector or initialized elsewhere
            // Deduct the cost of the building from the player's resources
        if(index == 0 && economymanager.canAffordforester)
        {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
            gamemanager.wood -= economymanager.foresterWoodCost;
            gamemanager.stone -= economymanager.foresterStoneCost;
            gamemanager.iron -= economymanager.foresterIronCost;
        }
        if (index == 1 && economymanager.canAffordMiner)
        {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
            gamemanager.wood -= economymanager.minerWoodCost;
            gamemanager.stone -= economymanager.minerStoneCost;
            gamemanager.iron -= economymanager.minerIronCost;
        }
        if (index == 2 && economymanager.canAffordTrainer)
        {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
            gamemanager.wood -= economymanager.trainerWoodCost;
            gamemanager.stone -= economymanager.trainerStoneCost;
            gamemanager.iron -= economymanager.trainerIronCost;
        }
        if (index == 3 && economymanager.canAffordTrainer)
        {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
            gamemanager.wood -= economymanager.trainerWoodCost;
            gamemanager.stone -= economymanager.trainerStoneCost;
            gamemanager.iron -= economymanager.trainerIronCost;
        }

        // Instantiate the building            
        pendingPrefab.tag = "PlaceObject";

            // Add physics components
            pendingPrefab.AddComponent<Rigidbody>();
            Rigidbody rigidbody = pendingPrefab.GetComponent<Rigidbody>();
            rigidbody.isKinematic = true;
            rigidbody.useGravity = false;

            // Add a box collider
            pendingPrefab.AddComponent<BoxCollider>();
            BoxCollider boxCollider = pendingPrefab.GetComponent<BoxCollider>();
            boxCollider.size = new Vector3(.99f, .99f, .99f);
            boxCollider.isTrigger = true;

            // Add the CheckPlacement script
            pendingPrefab.AddComponent<CheckPlacement>();
    }

    void UpdateMaterials()
    {
        if (pendingPrefab == null) return;
        if (canPlace)
        {
            pendingPrefab.GetComponent<MeshRenderer>().material = materials[0];
        } else
        {
            pendingPrefab.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    public void PlaceObject()
    {
        // pendingPrefab.GetComponent<MeshRenderer>().material = materials[2];
        ActiveBuildings.Add(pendingPrefab);
        pendingPrefab = null;
    }

    public void CancelPlacement()
    {
        pendingPrefab = null;
    }

    public void RotateObject()
    {
        pendingPrefab.transform.Rotate(Vector3.up, rotationAmount);
    }

    float RoundToNearestGrid(float pos)
    {
        float xDiff = pos % gridSize;
        pos -= xDiff;
        if (xDiff > (gridSize / 2))
        {
            pos += gridSize;
        }
        return pos;
    }
}
