using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    [Header("Handlers")]
    public Gamemanager gamemanager;
    public AudioManager AudioManager;
    [SerializeField] private GameObject pauseScreen;
    [SerializeField] private GameObject settingsScreen;

    [Header("boloean")]
    bool pauseScreenIsActive = false;
    bool settingScreenIsActive = false;
    [Header("Text")]
    public GameObject wood;
    public GameObject stone;
    public GameObject iron;
    [Header("TrainingCamp")]
    public TrainingCamp TrainingCamp;
    public GameObject timerText;
    public GameObject trainingCampUI;
    public GameObject trainingButtons;
    [Header("DestroyMove")]
    public GameObject DestroyButton;
    public GameObject MoveButton;
    public GameObject LoadingButtons;
    public bool IsLoadingScreenUI = false;  

    private void Start() {
        gamemanager = GameObject.FindObjectOfType<Gamemanager>();
        if (IsLoadingScreenUI) LoadingButtons.SetActive(true);
    }

    private void Update() {
        wood.GetComponent<TMP_Text>().SetText(gamemanager.wood.ToString("N0"));
        stone.GetComponent<TMP_Text>().SetText(gamemanager.stone.ToString("N0"));
        iron.GetComponent<TMP_Text>().SetText(gamemanager.iron.ToString("N0"));

        if(Input.GetKeyDown(KeyCode.Escape)) {
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
        if (IsLoadingScreenUI) LoadingButtons.SetActive(true);
        Time.timeScale = 1.0f;
    }
    public void OpenOptionsMenu()
    {
        pauseScreen.SetActive(false);
        settingsScreen.SetActive(true);
        if (IsLoadingScreenUI) LoadingButtons.SetActive(false);
        settingScreenIsActive = true;

    }
    public void ExitGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ChangeMusicVolume(Slider a) {
        AudioManager.m_AudioSources[0].volume = a.value;
    }
}
