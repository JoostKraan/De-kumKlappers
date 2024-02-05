using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEconomy : MonoBehaviour
{
    public float Money;
    public float Wood;
    public float Stone;
    public float Iron;

    // Adding functions

    public void AddMoney(float addedMoney)
    {
        Money += addedMoney; // Use += to add the value
    }

    public void AddWood(float addedWood)
    {
        Wood += addedWood; // Use += to add the value
    }

    public void AddStone(float addedStone)
    {
        Stone += addedStone; // Use += to add the value
    }

    public void AddIron(float addedIron)
    {
        Iron += addedIron; // Use += to add the value
    }

    // Removing functions

    public void RemoveMoney(float removedMoney)
    {
        Money -= removedMoney; // Use -= to subtract the value
    }

    public void RemoveWood(float removedWood)
    {
        Wood -= removedWood; // Use -= to subtract the value
    }

    public void RemoveStone(float removedStone)
    {
        Stone -= removedStone; // Use -= to subtract the value
    }

    public void RemoveIron(float removedIron)
    {
        Iron -= removedIron; // Use -= to subtract the value
    }
}
