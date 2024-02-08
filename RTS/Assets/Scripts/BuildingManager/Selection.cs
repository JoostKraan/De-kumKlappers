using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class Selection : MonoBehaviour
{
    public GameObject selectedObject;
    public TextMeshProUGUI objNameText;
    private BuildingPlacement buildingPlacement;
    public GameObject objUI;
    internal static Terrain activeObject;

    // Start is called before the first frame update
    void Start() {
        buildingPlacement = GameObject.Find("BuildingManager").GetComponent<BuildingPlacement>(); 
    }

    // Update is called once per frame
    void Update() {
       if (Input.GetMouseButtonDown(0)) {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;
           if (Physics.Raycast(ray, out hit, 1000)) {
                if (hit.collider.gameObject.CompareTag("PlaceObject")) {

                    Select(hit.collider.gameObject);
                    if (hit.collider.gameObject.name.Contains("Camp"))
                    {
                        TrainingCamp tr = hit.collider.gameObject.GetComponent<TrainingCamp>();
                        tr.ShopActive();
                    }
                }

                else Deselect();
            }
       }

       if (Input.GetMouseButtonDown(1)) {
            Deselect();
       }
    }

    private void Select(GameObject obj)
    {
        if (obj == buildingPlacement.pendingPrefab) return;
        if (obj == selectedObject) { Deselect(); return; };
        if (selectedObject != null) Deselect();
        //Outline outline = obj.GetComponent<Outline>();
       // if (outline == null) obj.AddComponent<Outline>();
        //else outline.enabled = true;
        objNameText.text = obj.name;
        selectedObject = obj;
        objUI.SetActive(true);
    }

    private void Deselect()
    { 
        if (selectedObject == null) return;
      //  selectedObject.GetComponent<Outline>().enabled = false;
        selectedObject = null;
        objNameText.text = "Select Building";
        objUI.SetActive(false);
    }

    public void Move() {
        GameObject objToDestroy = selectedObject;
        buildingPlacement.pendingPrefab = objToDestroy;

        if (buildingPlacement.ActiveBuildings.Contains(objToDestroy)) {
            buildingPlacement.ActiveBuildings.Remove(objToDestroy);
        }
    }

    public void Delete() {
        GameObject objToDestroy = selectedObject;
        Deselect();

        if (buildingPlacement.ActiveBuildings.Contains(objToDestroy)) {
            buildingPlacement.ActiveBuildings.Remove(objToDestroy);
        }

        Destroy(objToDestroy);
    }
}
