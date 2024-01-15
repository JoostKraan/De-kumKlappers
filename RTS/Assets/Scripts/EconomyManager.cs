using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    private Gamemanager gamemanager;

    public bool canAffordforester = false;
    public bool canAffordMiner = false;
    public bool canAffordTrainer = false;

    [Header("ForesterCost")]
    public int foresterWoodCost;
    public int foresterStoneCost;
    public int foresterIronCost;
    [Header("MinerCost")]
    public int minerWoodCost;
    public int minerStoneCost;
    public int minerIronCost;
    [Header("TrainerCost")]
    public int trainerWoodCost;
    public int trainerStoneCost;
    public int trainerIronCost;

    private void Start()
    {
        gamemanager = GameObject.FindObjectOfType<Gamemanager>();
    }
    private void Update()
    {
        if(gamemanager.wood >= foresterWoodCost && gamemanager.stone >= foresterStoneCost && gamemanager.iron >= foresterIronCost)
        {
            canAffordforester = true;
        }
        else
        {
            canAffordforester = false;
        }

        if (gamemanager.wood >= minerWoodCost && gamemanager.stone >= minerStoneCost && gamemanager.iron >= minerIronCost)
        {
            canAffordMiner = true;
        }
        else
        {
            canAffordMiner = false;
        }

        if (gamemanager.wood >= trainerWoodCost && gamemanager.stone >= trainerStoneCost && gamemanager.iron >= trainerIronCost)
        {
            canAffordTrainer = true;
        }
        else
        {
            canAffordTrainer = false;
        }
    }
}
