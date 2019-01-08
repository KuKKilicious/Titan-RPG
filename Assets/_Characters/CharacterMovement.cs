using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.AI;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class CharacterMovement : MonoBehaviour
    {
        const float FLOAT_ONE = 1f;

        [SerializeField] float stoppingDistance = 1f;
        [SerializeField] float moveSpeedMultiplier = 1f;
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float animatorSpeed = 1f;

        Rigidbody my_Rigidbody;
        NavMeshAgent agent;
        Animator animator;


        private float turnAmount;
        private float forwardAmount;
        private Vector3 groundNormal;
        private Vector3 clickPoint;
        private bool canMove = true;

        private void Start()
        {

            SetupComponents();
        }

        #region Setup
        private void SetupComponents()
        {
            SetupNavMeshAgent();
            SetupAnimator();
            SetupRigidBody();
            SetupRaycaster();
        }

        private void SetupRaycaster()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyMouseOverEnemy += CameraRaycaster_notifyMouseOverEnemy; ;
            cameraRaycaster.notifyMouseOverWalkableObservers += CameraRaycaster_notifyMouseOverWalkableObservers;
        }

        private void SetupAnimator()
        {
            animator = GetComponent<Animator>();
            animator.applyRootMotion = true;
        }

        private void SetupRigidBody()
        {
            my_Rigidbody = GetComponent<Rigidbody>();
            my_Rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        }

        private void SetupNavMeshAgent()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updatePosition = true;
            agent.stoppingDistance = stoppingDistance;
        }
        #endregion

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
            if (agent.remainingDistance > agent.stoppingDistance)
            {
                Move(agent.desiredVelocity);
            }
            else
            {
                Move(Vector3.zero);
            }
        }
        //subscriber method
        private void CameraRaycaster_notifyMouseOverWalkableObservers(Vector3 destination)
        {
            if (!canMove) { return; }
            if (Input.GetMouseButton(0))
            {
                agent.SetDestination(destination);
            }
        }


        public void StopMovement()
        {
            canMove = false;
        }

        private void OnAnimatorMove()
        {
            // we implement this function to override the default root motion.
            // this allows us to modify the positional speed before it's applied.
            if (Time.deltaTime > 0)
            {
                Vector3 v = (animator.deltaPosition * moveSpeedMultiplier) / Time.deltaTime;

                // we preserve the existing y part of the current velocity.
                v.y = my_Rigidbody.velocity.y;
                my_Rigidbody.velocity = v; 
            }
        }


        public void Move(Vector3 movement)
        {
            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            SetsForwardAndTurn(movement);

            ApplyExtraTurnRotation();

            UpdateAnimator();
        }

        private void SetsForwardAndTurn(Vector3 movement)
        {
            if (movement.magnitude > FLOAT_ONE)
            {
                movement.Normalize();
            }
            var localMovement = transform.InverseTransformDirection(movement);
            turnAmount = Mathf.Atan2(localMovement.x, localMovement.z);
            forwardAmount = localMovement.z;
        }

        void UpdateAnimator()
        {
            animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
            animator.speed = animatorSpeed;
        }



        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }


    }

}