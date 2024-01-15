using System.Collections;
using UnityEngine;
using TMPro;

public class Typewriter : MonoBehaviour
{
    public float typingSpeed = 0.05f; // Adjust the typing speed as needed
    public TMP_Text textToType;

    private string originalText;
    private string currentText;

    void Start()
    {
        originalText = textToType.text;
        textToType.text = "";
        StartCoroutine(TypeText());
    }

    IEnumerator TypeText()
    {
        for (int i = 0; i < originalText.Length; i++)
        {
            currentText = originalText.Substring(0, i + 1);
            textToType.text = currentText;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
}
