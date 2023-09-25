using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Selection : MonoBehaviour
{
    public GameObject selectedObject;
    public TextMeshProUGUI objNameText;
    private BuildingPlacement buildingPlacement;
    public GameObject objUI;

    // Start is called before the first frame update
    void Start()
    {
        buildingPlacement = GameObject.Find("BuildingManager").GetComponent<BuildingPlacement>(); 
    }

    // Update is called once per frame
    void Update()
    {
       if (Input.GetMouseButtonDown(0))
       {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;
           if (Physics.Raycast(ray, out hit, 1000))
           {
                if (hit.collider.gameObject.CompareTag("PlaceObject"))
                {
                    Select(hit.collider.gameObject);
                }
                else Deselect();
            }
       }
       if (Input.GetMouseButtonDown(1))
        {
            Deselect();
        }
    }

    private void Select(GameObject obj)
    {
        if (obj == selectedObject) { Deselect(); return; };
        if (selectedObject != null) Deselect();
        Outline outline = obj.GetComponent<Outline>();
        if (outline == null) obj.AddComponent<Outline>();
        else outline.enabled = true;
        objNameText.text = obj.name;
        selectedObject = obj;
        objUI.SetActive(true);
    }

    private void Deselect()
    {
        if (selectedObject == null) { print("Trying to delect when no object is active"); return; };
        selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
        objNameText.text = "Select Building";
        objUI.SetActive(false);
    }

    public void Move()
    {
        buildingPlacement.pendingPrefab = selectedObject;

        if (buildingPlacement.ActiveBuildings.Contains(selectedObject))
        {
            buildingPlacement.ActiveBuildings.Remove(selectedObject);
        }
    }

    public void Delete()
    {
        GameObject objToDestroy = selectedObject;
        Deselect();

        if (buildingPlacement.ActiveBuildings.Contains(objToDestroy))
        {
            buildingPlacement.ActiveBuildings.Remove(objToDestroy);
        }

        Destroy(objToDestroy);
    }
}
