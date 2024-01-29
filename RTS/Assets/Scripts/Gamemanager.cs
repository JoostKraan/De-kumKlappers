using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gamemanager : MonoBehaviour
{
    public static Gamemanager Instance;
    public Transform[] Units;
    public int startingWood = 100;
    public int startingStone = 50;
    public int startingIron = 20;

    public int wood;
    public int stone;
    public int iron;

    private void Start()
    {
        wood = startingWood;
        stone = startingStone;
        iron = startingIron;
    }


    
}
