using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArmRotation : MonoBehaviour
{
    [SerializeField]
    float rotateDegree = 1f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rotateCameraArm();
    }

    private void rotateCameraArm()
    {
        if (Input.GetKey(KeyCode.I))
        {
            transform.Rotate(0, rotateDegree * Time.timeScale, 0);
        }
        else if (Input.GetKey(KeyCode.O))
        {
            transform.Rotate(0, -rotateDegree * Time.timeScale, 0);
        }

    }


}
