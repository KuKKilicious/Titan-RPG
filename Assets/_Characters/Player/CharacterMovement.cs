using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(ThirdPersonCharacter))]
    public class CharacterMovement : MonoBehaviour
    {
        [SerializeField] float stoppingDistance = 1f;
        ThirdPersonCharacter character; // A reference to the ThirdPersonCharacter on the object
        Vector3 clickPoint;
        bool isInDirectMode = false; //whether WASD/Gamepad is used
        GameObject walkTarget = null;
        private bool canMove = true;
        NavMeshAgent agent;
        private void Start()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            character = GetComponent<ThirdPersonCharacter>();
            walkTarget = new GameObject("Player walk Target");
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;
            cameraRaycaster.notifyMouseOverEnemy += CameraRaycaster_notifyMouseOverEnemy; ;
            cameraRaycaster.notifyMouseOverWalkableObservers += CameraRaycaster_notifyMouseOverWalkableObservers;
        }

        private void CameraRaycaster_notifyMouseOverEnemy(Enemy enemy)
        {
            if (!canMove) { return; }
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(enemy.transform.position);
            }
          
        }
        private void Update()
        {
            if(agent.remainingDistance > agent.stoppingDistance)
            {
                character.Move(agent.desiredVelocity);
            }
            else
            {
                character.Move(Vector3.zero);
            }
        }
        //subscriber method
        private void CameraRaycaster_notifyMouseOverWalkableObservers(Vector3 destination)
        {
            if (!canMove) { return; }
            if (Input.GetMouseButton(0))
            {
                walkTarget.transform.position = destination;
                agent.SetDestination(destination);
            }
        }


        public void StopMovement()
        {
            canMove = false;
        }

    }

}