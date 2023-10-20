using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingClass : MonoBehaviour
{
    [SerializeField]
    private int Workers;
    [SerializeField]
    private int Capacity;
    [SerializeField]
    private int BuildingStage;
    [SerializeField]
    private string BuildingType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Handle Workers
    public void SetWorkers(int WorkersInt) { Workers = WorkersInt; }
    public int GetWorkers() { return Workers; }

    // Handle Capacity
    public void UpdateCapacity(int CapacityInt) { Capacity = CapacityInt; }
    public int GetCapacity() { return Capacity;}

    // Handle BuildState
    public void HandleBuildingStage(int CapacityInt)
    {

    }
    public int GetBuildState() { return BuildingStage; }
}
