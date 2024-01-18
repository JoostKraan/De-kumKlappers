using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacement : MonoBehaviour
{
    BuildingPlacement buildingPlacement;
    public List<GameObject> FoliageGameObjects = new List<GameObject>();

    void Start()
    {
        buildingPlacement = GameObject.Find("BuildingManager").GetComponent<BuildingPlacement>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PlaceObject")) {
            buildingPlacement.canPlace = false;
        }

        if (other.gameObject.CompareTag("Foliage")) {
            FoliageGameObjects.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("PlaceObject")) {
            buildingPlacement.canPlace = true;
        }

        if (other.gameObject.CompareTag("Foliage")) {
            FoliageGameObjects.Remove(other.gameObject);
        }
    }

    public void PlaceBuilding() {
        for (int i = 0; i < FoliageGameObjects.Count; i++) {
            GameObject FoliageItem = FoliageGameObjects[i];
            Destroy(FoliageItem);
        }
    }
}
