using UnityEngine;

namespace RPG.Characters
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(Animator))]
    public class ThirdPersonCharacter : MonoBehaviour
    {
        [SerializeField] float movingTurnSpeed = 360;
        [SerializeField] float stationaryTurnSpeed = 180;
        [SerializeField] float runCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others

        Rigidbody my_Rigidbody;
        Animator my_Animator;
        float turnAmount;
        float forwardAmount;
        Vector3 groundNormal;

        void Start()
        {
            my_Animator = GetComponent<Animator>();
            my_Rigidbody = GetComponent<Rigidbody>();
            my_Animator.applyRootMotion = true;
            my_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        }


        public void Move(Vector3 move)
        {

            // convert the world relative moveInput vector into a local-relative
            // turn amount and forward amount required to head in the desired
            // direction.
            if (move.magnitude > 1f) move.Normalize();
            move = transform.InverseTransformDirection(move);
            move = Vector3.ProjectOnPlane(move, groundNormal);
            turnAmount = Mathf.Atan2(move.x, move.z);
            forwardAmount = move.z;

            ApplyExtraTurnRotation();


            // send input and other state parameters to the animator
            UpdateAnimator(move);
        }


        void UpdateAnimator(Vector3 move)
        {
            // update the animator parameters
            my_Animator.SetFloat("Forward", forwardAmount, 0.1f, Time.deltaTime);
            my_Animator.SetFloat("Turn", turnAmount, 0.1f, Time.deltaTime);
      
            // calculate which leg is behind, so as to leave that leg trailing in the jump animation
            // (This code is reliant on the specific run cycle offset in our animations,
            // and assumes one leg passes the other at the normalized clip times of 0.0 and 0.5)
            float runCycle =
                Mathf.Repeat(
                    my_Animator.GetCurrentAnimatorStateInfo(0).normalizedTime + runCycleLegOffset, 1);
        }



        void ApplyExtraTurnRotation()
        {
            // help the character turn faster (this is in addition to root rotation in the animation)
            float turnSpeed = Mathf.Lerp(stationaryTurnSpeed, movingTurnSpeed, forwardAmount);
            transform.Rotate(0, turnAmount * turnSpeed * Time.deltaTime, 0);
        }



    }
}
