using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;

    public int startingWood = 100;
    public int startingStone = 50;
    public int startingIron = 20;

    [HideInInspector]
    public int wood;
    [HideInInspector]
    public int stone;
    [HideInInspector]
    public int iron;

    private void Start()
    {
        wood = startingWood;
        stone = startingStone;
        iron = startingIron;
    }


    //public void SpendResources(int woodAmount, int stoneAmount, int ironAmount)
    //{
    //    if (HasEnoughResources(woodAmount, stoneAmount, ironAmount))
    //    {
    //        wood -= woodAmount;
    //        stone -= stoneAmount;
    //        iron -= ironAmount;
    //    }
    //    // You may want to add additional handling if resources are insufficient
    //}
}
