using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] GameObject deathText;
    [SerializeField] Text generalText;
    [SerializeField] float textSpeedPerChar = 0.1f;

    private void Start()
    {
        generalText.text = "";
    }

    public void PlayDeathScreen()
    {

        deathText.gameObject.SetActive(true);
    }


    public void DisplayText(string text)
    {
        StartCoroutine(DisplayTextString(text));
    }

    private IEnumerator DisplayTextString(string text)
    {
        generalText.text = "";
        for (int i = 0; i < text.Length; i++)
        {
            generalText.text += text[i];
            yield return new WaitForSeconds(textSpeedPerChar);

        }
        StartCoroutine(ClearText(2f)); // todo implement non-conflicting way of handling text with multiple Triggers, (Queue?)
    }

    private IEnumerator ClearText(float v)
    {
        yield return new WaitForSeconds(v);
        generalText.text = "";
    }
}


