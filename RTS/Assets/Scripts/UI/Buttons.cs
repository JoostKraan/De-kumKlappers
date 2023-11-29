using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{


    [SerializeField] GameObject shop;

    [Header("boloean")]
    bool shopIsOpen = false;
    public void OpenShop()
    {
        if (shopIsOpen)
        {
            shop.SetActive(false);
            shopIsOpen = false;
        }
        else if (shopIsOpen == false)
        {
            shop.SetActive(true);
            shopIsOpen = true;
        }

    }
    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Escape))
        {
            if (shopIsOpen)
            {
                shop.SetActive(false);
            }
        }
    }
}
