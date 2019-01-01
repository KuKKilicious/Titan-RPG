using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraArmRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        rotateCameraArm();	
	}

    private void rotateCameraArm()
    {
        if (Input.GetKey(KeyCode.I))
        {
            Debug.Log("rotateCameraArm");
        }
    }
}
