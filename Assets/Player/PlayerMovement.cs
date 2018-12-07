using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {


    [SerializeField]
    float walkMoveStopRadius = 0.2f;

    ThirdPersonCharacter m_Character;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentClickTarget;
    bool isInDirectMode = false; 
    private void Start() {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        m_Character = GetComponent<ThirdPersonCharacter>();
        currentClickTarget = transform.position;
    }

    // Fixed update is called in sync with physics
    private void FixedUpdate() {
        if (Input.GetKeyDown(KeyCode.G)) { //G for Gamepad todo: allow player to remap later or add to menu
            isInDirectMode = !isInDirectMode; //toggle mode
            currentClickTarget = transform.position;
        }
        if (isInDirectMode) {
            ProcessDirectMovement();
        } else {
            ProcessMouseMovement();
        }
    }

    private void ProcessDirectMovement() {
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");
        Transform m_Cam = Camera.main.transform;
        Vector3 m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 m_Move = v * m_CamForward + h * m_Cam.right;
        m_Character.Move(m_Move,false,false);
    }

    private void ProcessMouseMovement() {
        if (Input.GetMouseButton(0)) {
            print("Cursor raycast layerHit" + cameraRaycaster.layerHit.ToString());
            switch (cameraRaycaster.layerHit) {
                case Layer.Walkable:
                    currentClickTarget = cameraRaycaster.hit.point;
                    break;
                case Layer.Enemy:
                    Debug.Log("not moving to enemy for now");
                    break;
                default:
                    Debug.Log("no processable layer hit");
                    return;
            }
            print("Cursor raycast hit" + cameraRaycaster.hit.collider.gameObject.name.ToString());

        }
        var playerToClickPoint = currentClickTarget - transform.position;
        if (playerToClickPoint.magnitude >= walkMoveStopRadius) {
            m_Character.Move(playerToClickPoint, false, false);
        } else {
            m_Character.Move(Vector3.zero, false, false);
        }
    }
}

