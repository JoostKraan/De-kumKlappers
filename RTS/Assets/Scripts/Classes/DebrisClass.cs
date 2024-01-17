using UnityEngine;

[System.Serializable]
public class DebrisClass
{
    public GameObject DebrisObject;
    public int Weight;

    public DebrisClass(GameObject gameObject, int ID)
    {
        this.DebrisObject = gameObject;
        this.Weight = ID;
    }
}