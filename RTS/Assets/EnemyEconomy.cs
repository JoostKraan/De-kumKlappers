using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEconomy : MonoBehaviour
{
    public int Money;
    public int Wood;
    public int Stone;
    public int Iron;

    public void AddMoney(int addedMoney)
    {
        Money += addedMoney;
    }
    public void RemoveMoney(int removedMoney)
    {
        Money -= removedMoney;
    }
    public void AddWood(int addedWood)
    {
        Wood += addedWood;
    }
    public void RemoveWood(int removedWood)
    {
        Wood -= removedWood;
    }
    public void AddStone(int addedStone)
    {
        Stone += addedStone;
    }
    public void RemoveStone(int removedStone)
    {
        Stone -= removedStone;
    }
    public void AddIron(int addedIron)
    {
        Iron += addedIron;
    }
    public void RemoveIron(int removedIron)
    {
        Iron -= removedIron;
    }
}
