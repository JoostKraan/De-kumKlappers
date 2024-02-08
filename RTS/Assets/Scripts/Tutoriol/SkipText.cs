using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SkipText : MonoBehaviour
{
    [SerializeField] private GameObject nextLine;
    [SerializeField] private GameObject dimmed;
    [SerializeField] private GameObject shop;

    public bool woodb = false;
    public bool trainingB = false;


    private TrainingCamp camp;
    private WoodWorkerSpawner wood;
    public Selection userInterface;
    void Start()
    {
        //placement = GameObject.FindObjectOfType<BuildingPlacement>();
    }
    void Update()
    {
        wood = GameObject.FindAnyObjectByType<WoodWorkerSpawner>();
        camp = GameObject.FindAnyObjectByType<TrainingCamp>();
        if (woodb)
        {
            if (Input.GetMouseButtonDown(0) && wood != null)
            {
                if (userInterface) userInterface.CloseUI();
                nextLine.SetActive(true);
                Destroy(gameObject);
            }
        }
        if (trainingB)
        {
            if (Input.GetMouseButtonDown(0) && camp != null)
            {
                //if (userInterface) userInterface.CloseUI();
                nextLine.SetActive(true);
                Destroy(gameObject);
            }
        }
        //if (shop.active == true)
        //{
        //    print("open");
        //    NextText();
        //}
    }
    public void WaitForPlacement()
    {
        dimmed.SetActive(false);
    }
    public void NextText()
    {
        nextLine.SetActive(true);
        Destroy(gameObject);
    }
    public void RemoveText()
    {
        Destroy(gameObject);
    }
    public void LoadGame()
    {
        SceneManager.LoadScene("Main Scene");
    }
}
