using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(AICharacterControl))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class PlayerMovement : MonoBehaviour
    {

        ThirdPersonCharacter thirdPersonCharacter; // A reference to the ThirdPersonCharacter on the object
        CameraRaycaster cameraRaycaster = null;
        Vector3 currentDestination, clickPoint;
        bool isInDirectMode = false; //whether WASD/Gamepad is used
        AICharacterControl aiCharControl = null;
        GameObject walkTarget = null;
        private bool canMove = true;
        private void Start()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyMouseOverEnemy += CameraRaycaster_notifyMouseOverEnemy; ;
            cameraRaycaster.notifyMouseOverWalkableObservers += CameraRaycaster_notifyMouseOverWalkableObservers;
            thirdPersonCharacter = GetComponent<ThirdPersonCharacter>();
            aiCharControl = GetComponent<AICharacterControl>();
            currentDestination = transform.position;

            walkTarget = new GameObject("Player walk Target");
        }

        private void CameraRaycaster_notifyMouseOverEnemy(Enemy enemy)
        {
            if (!canMove) { return; }
            if (Input.GetMouseButton(0))
            {
                aiCharControl.target = enemy.transform;
            }
            else if (Input.GetMouseButtonDown(1))
            {

            }
        }

        //subscriber method
        private void CameraRaycaster_notifyMouseOverWalkableObservers(Vector3 destination)
        {
            if (!canMove) { return; }
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                aiCharControl.target = walkTarget.transform;
            }
        }


        // Fixed update is called in sync with physics
        private void FixedUpdate()
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                //G for Gamepad todo: allow player to remap later or add to menu
                isInDirectMode = !isInDirectMode; //toggle mode
                currentDestination = transform.position;
            }

            if (isInDirectMode)
            {
                ProcessDirectMovement();
            }
            else
            {
                // ProcessMouseMovement();
            }
        }

        //TODO make this get called again
        private void ProcessDirectMovement()
        {
            float h = CrossPlatformInputManager.GetAxis("Horizontal");
            float v = CrossPlatformInputManager.GetAxis("Vertical");
            Transform m_Cam = Camera.main.transform;
            Vector3 camForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward + h * m_Cam.right;
            thirdPersonCharacter.Move(move, false, false);
        }

       
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.black;
            Gizmos.DrawLine(transform.position, currentDestination);
            Gizmos.DrawSphere(currentDestination, 0.1f);
            Gizmos.DrawSphere(clickPoint, 0.1f);

            Gizmos.color = Color.cyan;

            Gizmos.DrawWireSphere(transform.position, 5f); // ranged
            Debug.Log("Gizzzzzmo");
        }

        public void StopMovement()
        {
            canMove = false;
        }

    }

}