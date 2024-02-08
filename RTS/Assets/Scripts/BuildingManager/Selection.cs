using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Selection : MonoBehaviour
{
    public GameObject selectedObject;
    public TextMeshProUGUI objNameText;
    private BuildingPlacement buildingPlacement;
    public GameObject objUI;
    public EventSystem EventSystem;
    public Buttons UIHandler;
    public TrainingCamp tr;
    internal static Terrain activeObject;

    // Start is called before the first frame update
    void Start() {
        buildingPlacement = GameObject.Find("BuildingManager").GetComponent<BuildingPlacement>(); 
    }

    // Update is called once per frame
    void Update() {
       if (Input.GetMouseButtonDown(0))
       {
           Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
           RaycastHit hit;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (Physics.Raycast(ray, out hit, 1000)) {
                if (hit.collider.gameObject.CompareTag("PlaceObject")) {
                    if (hit.collider.gameObject.name.Contains("Camp")) {
                        tr = null;
                        tr = hit.collider.gameObject.GetComponent<TrainingCamp>();
                    }

                    Select(hit.collider.gameObject);
                }
                else {
                    Deselect();
                }
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
        objNameText.text = obj.name;
        selectedObject = obj;
        objUI.SetActive(true);

        if (tr != null) {
            UIHandler.trainingCampUI.SetActive(true);
            tr.countdownText = UIHandler.timerText.GetComponent<TMP_Text>();
            tr.ActiveUI = UIHandler;

            UIHandler.MoveButton.GetComponent<Button>().interactable = !tr.isCountingDown;
            UIHandler.DestroyButton.GetComponent<Button>().interactable = !tr.isCountingDown;

            for (int i = 0; i < UIHandler.trainingButtons.transform.childCount; i++) {
                GameObject uiButton = UIHandler.trainingButtons.transform.GetChild(i).gameObject;
                uiButton.GetComponent<Button>().interactable = !tr.isCountingDown;
            }
        }
    }

    private void Deselect() { 
        if (selectedObject == null) return;
        selectedObject = null;

        if (tr != null) {
            UIHandler.trainingCampUI.SetActive(false);
            tr.countdownText = null;
            tr.ActiveUI = null;
            UIHandler.timerText.GetComponent<TMP_Text>().text = "Select which to train.";
        }

        tr = null;
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

    public void CloseUI() {
        Deselect();
    }

    public void TrainSoldier(int index) {
        if (tr != null) {
            tr.SetPrefabToSpawn(index);

            UIHandler.MoveButton.GetComponent<Button>().interactable = false;
            UIHandler.DestroyButton.GetComponent<Button>().interactable = false;

            for (int i = 0; i < UIHandler.trainingButtons.transform.childCount; i++) {
                GameObject uiButton = UIHandler.trainingButtons.transform.GetChild(i).gameObject;
                uiButton.GetComponent<Button>().interactable = false;
            }
        }
    }
}
