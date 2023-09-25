using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelection : MonoBehaviour
{
    public LayerMask unitLayer;
    public List<Unit> selectedUnits = new List<Unit>();

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, unitLayer))
            {
                Unit unit = hit.collider.gameObject.GetComponent<Unit>();

                if (unit != null)
                {
                    // Toggle the selection state of the unit
                    if (selectedUnits.Contains(unit))
                    {
                        selectedUnits.Remove(unit);
                        unit.Selected = false;
                    }
                    else
                    {
                        selectedUnits.Add(unit);
                        unit.Selected = true;
                    }
                }
            }
            else
            {
                // Deselect all units if no unit was clicked
                DeselectAllUnits();
            }
        }
    }

    // Helper method to deselect all units
    private void DeselectAllUnits()
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.Selected = false;
        }
        selectedUnits.Clear();
    }
}
