using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSelections : MonoBehaviour
{
    public List<GameObject> unitList = new List<GameObject>();
    public List<GameObject> unitsSelected = new List<GameObject>();

    private static UnitSelections _instance;
    public static UnitSelections instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public void ClickSelect(GameObject unitToAdd)
    {
        if (IsPlayerUnit(unitToAdd))
        {
            DeselectAll();
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    public void ShiftClickSelect(GameObject unitToAdd)
    {
        if (IsPlayerUnit(unitToAdd))
        {
            if (!unitsSelected.Contains(unitToAdd))
            {
                unitsSelected.Add(unitToAdd);
                unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
                unitToAdd.GetComponent<UnitMovement>().enabled = true;
            }
            else
            {
                unitToAdd.GetComponent<UnitMovement>().enabled = false;
                unitToAdd.transform.GetChild(0).gameObject.SetActive(false);
                unitsSelected.Remove(unitToAdd);
            }
        }
    }

    public void DragSelect(GameObject unitToAdd)
    {
        if (IsPlayerUnit(unitToAdd) && !unitsSelected.Contains(unitToAdd))
        {
            unitsSelected.Add(unitToAdd);
            unitToAdd.transform.GetChild(0).gameObject.SetActive(true);
            unitToAdd.GetComponent<UnitMovement>().enabled = true;
        }
    }

    public void DeselectAll()
    {
        foreach (var unit in unitsSelected)
        {
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.transform.GetChild(0).gameObject.SetActive(false);
        }
        unitsSelected.Clear();
    }

    public void Deselect(GameObject unitToDeselect)
    {
        unitsSelected.Remove(unitToDeselect);
        unitToDeselect.GetComponent<UnitMovement>().enabled = false;
        unitToDeselect.transform.GetChild(0).gameObject.SetActive(false);
    }

    private bool IsPlayerUnit(GameObject unit)
    {
        // Add a condition based on your tag or layer
        return unit.CompareTag("PlayerUnit");
    }
}
