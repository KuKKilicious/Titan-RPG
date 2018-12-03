using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {
    
    Transform playerToFollow;
    private void Start() {
        playerToFollow = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate() {
        transform.position = playerToFollow.position;
    }
}
