using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public Gamemanager gamemanager;

    [Header("variable")]
    private float cooldownTimer = 3f;
    public int collectValue = 5;
    [Header("bool")]
    public bool woodCollector = false;
    public bool stoneCollector = false;
    public bool ironCollector = false;
    private void Start()
    {
        gamemanager= GameObject.FindWithTag("Gamemanager").GetComponent<Gamemanager>();
    }
    private void Update()
    {
        cooldownTimer -= Time.deltaTime;
        if (cooldownTimer <= 0)
        {
            if (woodCollector)
            {
                gamemanager.wood += collectValue;
            }
            if (stoneCollector)
            {
                gamemanager.stone += collectValue;
            }
            if (ironCollector)
            {
                gamemanager.iron += collectValue;
            }
        }
    }
}

