using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildingPlacement : MonoBehaviour
{
    public GameObject[] buildingPrefabs;
    public List<GameObject> ActiveBuildings = new List<GameObject>();

    public GameObject pendingPrefab;
    [SerializeField] private Material[] materials;

    private Vector3 pos;

    private RaycastHit hit;
    [SerializeField] private LayerMask layerMask;

    public float gridSize;
    public float rotationAmount;
    public bool canPlace;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && Input.GetKeyDown(KeyCode.Q))
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
        print(index.ToString());
        pendingPrefab = Instantiate(buildingPrefabs[index], pos, transform.rotation);
        pendingPrefab.tag = "PlaceObject";

        pendingPrefab.AddComponent<Rigidbody>();
        pendingPrefab.GetComponent<Rigidbody>().isKinematic = true;
        pendingPrefab.GetComponent<Rigidbody>().useGravity = false;

        pendingPrefab.AddComponent<BoxCollider>();
        BoxCollider boxCollider = pendingPrefab.GetComponent<BoxCollider>();

        boxCollider.size = new Vector3(.99f, .99f, .99f);
        boxCollider.isTrigger = true;

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
        pendingPrefab.GetComponent<MeshRenderer>().material = materials[2];
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
