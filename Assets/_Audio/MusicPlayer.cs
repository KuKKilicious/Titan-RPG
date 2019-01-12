using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MusicPlayer : MonoBehaviour
{
    MusicPlayer musicPlayer;
    void Awake()
    {
        MusicPlayer[] objs = GameObject.FindObjectsOfType<MusicPlayer>();

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
