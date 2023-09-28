using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    BuildingPlacement buildingPlacement;

    void Start()
    {
        buildingPlacement = GameObject.Find("BuildingManager").GetComponent<BuildingPlacement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlaceObject"))
        {
            buildingPlacement.canPlace = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlaceObject"))
        {
            buildingPlacement.canPlace = true;
        }
    }
}
