using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitSelection : MonoBehaviour
{
    public Camera mainCamera;
    public List<GameObject> unitList = new List<GameObject>();
    public Image boxSelectionImage; // Reference to the UI Image for box selection visuals

    private Vector3 boxStartWorldPosition;
    private Vector3 boxEndWorldPosition;
    private bool isBoxSelecting = false;

    void Start()
    {
        mainCamera = Camera.main; // Assuming you're using the main camera
        // Disable the UI Image at the start
        boxSelectionImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {

                if (hit.transform.gameObject.CompareTag("TrainingCamp"))
                {
                    Debug.Log("Cool");
                    hit.transform.gameObject.GetComponent<TrainingCamp>().Shop.GetComponentInChildren<Canvas>().enabled = true;
                }

                if (hit.transform.gameObject.CompareTag("Unit"))
                {
                    GameObject selectedUnit = hit.transform.gameObject;

                    if (unitList.Contains(selectedUnit))
                    {
                        DeselectUnit(selectedUnit);
                    }
                    else
                    {
                        SelectUnit(selectedUnit);
                    }
                }
                else
                {
                    DeselectAllUnits();
                }
               
            }
        }

        if (Input.GetMouseButtonDown(0)) // Check if the left mouse button is clicked
        {
            // Record the start world position for box selection
            boxStartWorldPosition = GetMouseWorldPosition();
            isBoxSelecting = true;
            DeselectAllUnits();
        }

        if (Input.GetMouseButtonUp(0)) // Left mouse button release
        {
            // Disable the UI Image for box selection visuals
            boxSelectionImage.gameObject.SetActive(false);
            isBoxSelecting = false;
        }

        // Enable the UI Image for box selection visuals when holding down the mouse button and dragging
        if (isBoxSelecting)
        {
            // Record the end world position for box selection
            boxEndWorldPosition = GetMouseWorldPosition();

            // Set the position and size of the UI Image
            Vector3 screenStart = mainCamera.WorldToScreenPoint(boxStartWorldPosition);
            Vector3 screenEnd = mainCamera.WorldToScreenPoint(boxEndWorldPosition);

            Vector3 screenCenter = (screenStart + screenEnd) / 2;
            Vector3 screenSize = new Vector3(Mathf.Abs(screenEnd.x - screenStart.x), Mathf.Abs(screenEnd.y - screenStart.y), 100); // The third parameter (100) represents the depth of the box - adjust it as needed.

            boxSelectionImage.rectTransform.position = screenCenter;
            boxSelectionImage.rectTransform.sizeDelta = new Vector2(screenSize.x, screenSize.y);
            boxSelectionImage.gameObject.SetActive(true);

            // Check for all objects inside the selection box
            CheckObjectsInsideSelectionBox();
        }
    }

    // Get the world position of the mouse pointer
    Vector3 GetMouseWorldPosition()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    // Implement the logic to check for all objects inside the selection box
    void CheckObjectsInsideSelectionBox()
    {
        Collider[] colliders = Physics.OverlapBox((boxStartWorldPosition + boxEndWorldPosition) / 2,
            new Vector3(Mathf.Abs(boxEndWorldPosition.x - boxStartWorldPosition.x) / 2,
                        Mathf.Abs(boxEndWorldPosition.y - boxStartWorldPosition.y) / 2, 100)); // The third parameter (100) represents the depth of the box - adjust it as needed.

        foreach (Collider collider in colliders)
        {
            GameObject obj = collider.gameObject;

            if (obj.CompareTag("Unit"))
            {
                SelectUnit(obj);
            }
        }
    }

    private void SelectUnit(GameObject unit)
    {
        if (!unitList.Contains(unit))
        {
            unitList.Add(unit);
            unit.GetComponent<Unit>().isSelected = true;
        }
    }

    private void DeselectUnit(GameObject unit)
    {
        if (unitList.Contains(unit))
        {
            unitList.Remove(unit);
            unit.GetComponent<Unit>().isSelected = false;
        }
    }

    private void DeselectAllUnits()
    {
        foreach (GameObject unit in unitList)
        {
            unit.GetComponent<Unit>().isSelected = false;
        }
        unitList.Clear(); // Clear the unitList to remove deselected units
    }
}
