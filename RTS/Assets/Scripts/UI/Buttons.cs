using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    private Gamemanager gamemanager;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;

    [Header("boloean")]
    bool pauseScreenIsActive = false;
    bool settingScreenIsActive = false;
    [Header("Text")]
    public Text wood;
    public Text stone;
    public Text iron;
    private void Start()
    {
        gamemanager = GameObject.FindObjectOfType<Gamemanager>();
    }
    private void Update()
    {
        wood.text = gamemanager.wood.ToString();
        stone.text = gamemanager.stone.ToString();
        iron.text = gamemanager.iron.ToString();

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(!pauseScreenIsActive)
            {
                pauseScreen.SetActive(true);
                pauseScreenIsActive = true;
            }
            else if(pauseScreenIsActive)
            {
                if (settingScreenIsActive)
                {
                    settingsScreen.SetActive(false);
                    pauseScreen.SetActive(true);
                    settingScreenIsActive = false;
                }
                else
                {
                    pauseScreen.SetActive(false);
                    pauseScreenIsActive = false;
                }
            }
        }
        if(pauseScreenIsActive)
        {
            Time.timeScale = 0f;
        }
        if(pauseScreenIsActive == false)
        {
            Time.timeScale = 1.0f;
        }
    }
    public void PauseGame()
    {
        pauseScreenIsActive = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }
    public void ResumeGame()
    {
        pauseScreenIsActive = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1.0f;
    }
    public void OpenOptionsMenu()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
        settingScreenIsActive = true;
    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }
}
