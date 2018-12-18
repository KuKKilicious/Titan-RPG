using System;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(ThirdPersonCharacter))]
public class PlayerMovement : MonoBehaviour {


    [SerializeField]
    float walkMoveStopRadius = 0.3f;
    [SerializeField]
    float meleeMoveStopRadius = 0.4f;
    [SerializeField]
    float rangedMoveStopRadius = 5f;

    [SerializeField]
    float dashDistance = 7f;

    ThirdPersonCharacter thirdPersonCharacter;   // A reference to the ThirdPersonCharacter on the object
    CameraRaycaster cameraRaycaster;
    Vector3 currentDestination, clickPoint;
    bool isInDirectMode = false;


    private void Start() {
        cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
        thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
        currentDestination = transform.position;
    }
    /*
        // Fixed update is called in sync with physics
        private void FixedUpdate() {
            if (Input.GetKeyDown(KeyCode.G)) { //G for Gamepad todo: allow player to remap later or add to menu
                isInDirectMode = !isInDirectMode; //toggle mode
                currentDestination = transform.position;
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
            Vector3 camForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward + h * m_Cam.right;
            thirdPersonCharacter.Move(move, false, false);
        }

        private void ProcessMouseMovement() {
            if (Input.GetMouseButton(0)) { // left click
                setCurrentDestination(meleeMoveStopRadius);

            }
            if (Input.GetMouseButton(1)) { //right click
                setCurrentDestination(rangedMoveStopRadius);

           }
            walkToDestination();
        }

        private void setCurrentDestination(float enemyStopRadius) {
            clickPoint = cameraRaycaster.hit.point;
            switch (cameraRaycaster.currentLayerHit) {
                case Layer.Walkable:
                    if (Vector3.Distance(transform.position, clickPoint) < walkMoveStopRadius) {
                        break;
                    }
                    currentDestination = ShortDestination(clickPoint, walkMoveStopRadius);
                    break;
                case Layer.Enemy:
                    if (Vector3.Distance(transform.position, clickPoint) < enemyStopRadius) {
                        break;
                    }
                    currentDestination = ShortDestination(clickPoint, enemyStopRadius);
                    break;
                default:
                    Debug.Log("no processable layer hit");
                    return;
            }
        }

        private void walkToDestination() {
            var playerToClickPoint = currentDestination - transform.position;
            if (playerToClickPoint.magnitude >= 0) {
                thirdPersonCharacter.Move(playerToClickPoint, false, false);
            } else {
                thirdPersonCharacter.Move(Vector3.zero, false, false);
            }
        }

        private Vector3 ShortDestination(Vector3 destination, float shortening) {

            Vector3 reductionVector = (destination - transform.position).normalized * shortening;
            return destination - reductionVector;
        }

        private void OnDrawGizmos() {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, currentDestination);
            Gizmos.DrawSphere(currentDestination, 0.1f);
            Gizmos.DrawSphere(clickPoint, 0.1f);

            Gizmos.color = Color.cyan;

            Gizmos.DrawWireSphere(transform.position, 5f); // ranged
            Debug.Log("Gizzzzzmo");
        }
        */
}

