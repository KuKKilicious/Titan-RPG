using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject deathText;

    public void PlayDeathScreen()
    {

        deathText.gameObject.SetActive(true);
    }

}

