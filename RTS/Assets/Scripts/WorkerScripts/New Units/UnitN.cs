using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitN : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnitSelections.instance.unitList.Add(this.gameObject);
    }

     void OnDestroy()
    {
        UnitSelections.instance.unitList.Remove(this.gameObject);
    }
}
