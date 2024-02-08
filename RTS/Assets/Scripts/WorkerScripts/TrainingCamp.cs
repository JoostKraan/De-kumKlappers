using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;
using UnityEngine.UI;

public class TrainingCamp : MonoBehaviour
{
    public GameObject[] prefabsToSpawn;
    public Transform spawnPoint;
    public GameObject Shop;
    public TMP_Text countdownText;
    public Buttons ActiveUI;
    public float countdownTimer = 30f;
    public bool isCountingDown = false;
    private int prefabIndex = 0;
    public float timeToSpawn;


    void Start() {
        UpdateCountdownText();
    }


    void Update()
    {
       if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
          || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
           Shop.GetComponentInChildren<Canvas>().enabled = false;
        }
        if (isCountingDown) {
            countdownTimer -= Time.deltaTime;
            UpdateCountdownText();

            if (countdownTimer == 0) {
                isCountingDown = false;
                countdownTimer = 30f;
                UpdateCountdownText();
                SpawnPrefab(prefabIndex); 
            }
        }
    }
    public void ShopActive() {
        Shop.GetComponentInChildren<Canvas>().enabled = true;
    }
    public void ToggleCountdown() {
        isCountingDown = !isCountingDown;

        if (isCountingDown) {
            UpdateCountdownText();
        }
    }

    public void SetPrefabToSpawn(int index)
    {
        prefabIndex = index; 
        countdownTimer = prefabsToSpawn[index].GetComponent<UnitN>().timeToSpawn;
        ToggleCountdown();
    }

    public void SpawnPrefab(int index) {
        if (index >= 0 && index < prefabsToSpawn.Length) {
            if (prefabsToSpawn[index] != null && spawnPoint != null) {
                Instantiate(prefabsToSpawn[index], spawnPoint.position, spawnPoint.rotation);
            }
        }
    }

    private void UpdateCountdownText() {
        if (!countdownText) return;

        if (isCountingDown) {
            countdownTimer = Mathf.Max(0, countdownTimer);
            TimeSpan timeSpan = TimeSpan.FromSeconds(countdownTimer);
            countdownText.text = $"Training | {timeSpan.ToString(@"mm\:ss")}";
        } else {
            countdownText.text = "Select which to train.";

            if (ActiveUI) {
                for (int i = 0; i < ActiveUI.trainingButtons.transform.childCount; i++) {
                    GameObject uiButton = ActiveUI.trainingButtons.transform.GetChild(i).gameObject;
                    uiButton.GetComponent<Button>().interactable = true;
                }
            }
         }
    }
}