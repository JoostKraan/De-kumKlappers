using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.Burst.CompilerServices;

public class TrainingCamp : MonoBehaviour
{
    public GameObject[] prefabsToSpawn; // Array of prefabs for different unit types
    public Transform spawnPoint;
    public GameObject Shop;
    public TMP_Text countdownText; // Reference to the TextMeshPro Text component
    private float countdownTimer = 30f;
    private bool isCountingDown = false;
    private int prefabIndex = 0; // Index to select the prefab to spawn

    // Start is called before the first frame update
    void Start()
    {
        // Make sure you've assigned the spawnPoint and countdownText in the Inspector.
        UpdateCountdownText();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.anyKeyDown && !(Input.GetMouseButtonDown(0)
             || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2)))
        {
            Shop.GetComponentInChildren<Canvas>().enabled = false;
        }
        if (isCountingDown)
        {
            countdownTimer -= Time.deltaTime;
            UpdateCountdownText();

            if (countdownTimer == 0)
            {
                isCountingDown = false; // Stop the countdown
                countdownTimer = 30f; // Reset the timer to 30 seconds
                UpdateCountdownText();
                SpawnPrefab(prefabIndex); // Use the selected index to spawn the corresponding prefab
            }
        }
    }

    public void ToggleCountdown()
    {
        isCountingDown = !isCountingDown; // Toggle countdown on and off

        if (isCountingDown)
        {
            UpdateCountdownText();
        }
    }

    public void SetPrefabToSpawn(int index)
    {
        prefabIndex = index; // Set the index of the prefab to spawn
        countdownTimer = prefabsToSpawn[index].GetComponent<Unit>().timeToSpawn;
        ToggleCountdown();
    }

    public void SpawnPrefab(int index)
    {
        if (index >= 0 && index < prefabsToSpawn.Length)
        {
            if (prefabsToSpawn[index] != null && spawnPoint != null)
            {
                Instantiate(prefabsToSpawn[index], spawnPoint.position, spawnPoint.rotation);
            }
        }
    }

    private void UpdateCountdownText()
    {
        if (isCountingDown)
        {
            countdownTimer = Mathf.Max(0, countdownTimer);
            TimeSpan timeSpan = TimeSpan.FromSeconds(countdownTimer);
            countdownText.text = timeSpan.ToString(@"mm\:ss");
        }
        else
        {
            countdownText.text = "00:30";
        }
    }
}