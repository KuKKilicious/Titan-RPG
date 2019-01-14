using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Characters;

public class TextAppearance : MonoBehaviour
{
    [SerializeField] [TextArea] string textToDisplay;
    [SerializeField] bool playOnce;


    bool hasPlayed;


    UIManager uiManager;
    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (playOnce && hasPlayed)
            {
                return;
            }
            else
            {
                uiManager.DisplayText(textToDisplay);
                hasPlayed = true;
            }

        }
    }
}
