using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCursor : MonoBehaviour {
    CameraRaycaster raycaster;
	// Use this for initialization
	void Start () {
        raycaster = GetComponent<CameraRaycaster>();
	}
	
	// Update is called once per frame
	void Update () {
        Debug.Log(raycaster.layerHit);
	}
}
