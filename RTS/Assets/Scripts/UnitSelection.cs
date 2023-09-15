using UnityEngine;
using System.Collections.Generic;

public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayer; // Layer where your units are placed
    private Vector3 selectionStart;
    private List<Unit> selectedUnits = new List<Unit>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionStart = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            Vector3 selectionEnd = Input.mousePosition;
            SelectUnits(selectionStart, selectionEnd);
        }

        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0) // Right-click to move
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Vector3 targetPosition = hit.point;

                // Move all selected units to the target position
                foreach (var unit in selectedUnits)
                {
                    unit.MoveTo(targetPosition);
                }
            }
        }
    }

    void SelectUnits(Vector3 start, Vector3 end)
    {
        // Perform a box selection here and add selected units to the 'selectedUnits' list.
        // You'll need to use Physics.RaycastAll or other methods to detect units within the selection box.
    }
}
