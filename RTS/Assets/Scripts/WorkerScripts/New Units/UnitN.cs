using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitN : MonoBehaviour
{
    public int timeToSpawn = 30;


    void Start()
    {
        UnitSelections.instance.unitList.Add(this.gameObject);
    }

     void OnDestroy()
    {
        UnitSelections.instance.unitList.Remove(this.gameObject);
    }
}
