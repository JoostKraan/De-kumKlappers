using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour
{
    Gamemanager gamemanager;

    public GameObject[] buildingPrefabs;
    public List<GameObject> ActiveBuildings = new List<GameObject>();

    public GameObject pendingPrefab;
    [SerializeField] private NavMeshSurface navMesh;
    [SerializeField] private Material[] materials;
    [SerializeField] private Material BaseMaterial;

    private Vector3 pos;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    public float gridSize;
    public float rotationAmount;
    public bool canPlace;

    private void Start()
    {
        gamemanager = GameObject.FindObjectOfType<Gamemanager>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.X))
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
        if(index == 0 && gamemanager.wood >= 75) {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
        }
        if (index == 1 && gamemanager.wood >= 150) {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
        }
        if (index == 2 && gamemanager.wood >= 50 && gamemanager.stone >= 25) {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
        }
        if (index == 3 && gamemanager.wood >= 75 && gamemanager.stone >= 50) {
            pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
        }

        // Instantiate the building            
        pendingPrefab.tag = "PlaceObject";

        // Add physics components
        pendingPrefab.AddComponent<Rigidbody>();
        Rigidbody rigidbody = pendingPrefab.GetComponent<Rigidbody>();
        rigidbody.isKinematic = true;
        rigidbody.useGravity = false;

        // Add the CheckPlacement script
        pendingPrefab.AddComponent<CheckPlacement>();
    }

    void UpdateMaterials()
    {
        if (pendingPrefab == null) return;
        if (canPlace) {
            pendingPrefab.GetComponent<MeshRenderer>().material = materials[0];
        } else {
            pendingPrefab.GetComponent<MeshRenderer>().material = materials[1];
        }
    }

    public void PlaceObject()
    {
        ActiveBuildings.Add(pendingPrefab);
        pendingPrefab.GetComponent<MeshRenderer>().material = BaseMaterial;
        pendingPrefab.GetComponent<CheckPlacement>().PlaceBuilding();

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
