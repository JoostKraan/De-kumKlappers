using UnityEngine;
using TMPro;

public class TimeControlTMP : MonoBehaviour
{
    public TMP_Text timeScaleText; // Text to display current time scale
    private float originalTimeScale = 1f;
    private float maxTimeScale = 3f; // Adjust the maximum time scale as needed

    void Start()
    {
        originalTimeScale = Time.timeScale;
        UpdateTimeScaleText();
    }

    void Update()
    {
        // You can add other logic or functionality here
    }

    public void SpeedUpTime()
    {
        if (Time.timeScale < maxTimeScale)
        {
            Time.timeScale += 0.5f; // You can adjust the increment as needed

            // Clamp the time scale to the maximum value
            Time.timeScale = Mathf.Clamp(Time.timeScale, originalTimeScale, maxTimeScale);
        }
        else
        {
            // Reset time scale to its original value when it reaches the maximum
            Time.timeScale = originalTimeScale;
        }

        UpdateTimeScaleText();
    }

    void UpdateTimeScaleText()
    {
        timeScaleText.text = "" + Time.timeScale.ToString("F1");
    }
}
