using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{
    [RequireComponent(typeof(WeaponSystem))]
    public class EnemyAI : MonoBehaviour
    {
        const float waitTimeToReturn = 3f;
        //todo remove
        [SerializeField] float chaseRadius = 7;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float waitTimeBetweenAttacks = 3f;

        float currentWeaponRange = 3;
        float distanceToPlayer = 0f;
        enum State { idle, attacking, patrolling, chasing, rangedAttacking }
        State state = State.idle;
        int nextWaypointIndex;
        Vector3 spawnPos;
        Player player;
        CharacterMovement characterMovement;
        WeaponSystem weaponSystem;
        RangedEnemy rangedEnemy; //nullable
        WeaponConfig meleeWeapon;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            characterMovement = GetComponent<CharacterMovement>();
            weaponSystem = GetComponent<WeaponSystem>();
            spawnPos = transform.position;

            rangedEnemy = GetComponent<RangedEnemy>(); // nullable
            weaponSystem = GetComponent<WeaponSystem>();
            meleeWeapon = weaponSystem.GetWeaponConfig();
            currentWeaponRange = meleeWeapon.MaxAttackRange;
        }

        private void Update()
        {

            distanceToPlayer = Vector3.Distance(player.AimTransform.position, transform.position);

            bool inWeaponCircle = distanceToPlayer <= currentWeaponRange;
            bool inChaseCircle = distanceToPlayer > currentWeaponRange && distanceToPlayer <= chaseRadius;
            bool outsideChaseCircle = distanceToPlayer > chaseRadius;

            if (InsideRangedCircle() && state != State.rangedAttacking)
            {
                StopBehaviour();
                //start ranged attack
                state = State.rangedAttacking;
                weaponSystem.StartRangedAttackRepeatedlyCoroutine(rangedEnemy.RangedWeapon, player.gameObject);
            }
            else if (outsideChaseCircle && !InsideRangedCircle())
            {
                if (patrolPath != null && state != State.patrolling)
                {
                    StopBehaviour();
                    //start patrolling
                    StartCoroutine(Patrol());
                }
                else if (patrolPath == null &&state != State.idle)
                {
                    StopBehaviour();
                    //start idling 
                    state = State.idle;
                    StartCoroutine(ReturnToOriginAfterDelay(waitTimeToReturn));
                }
            }
            else if (inChaseCircle && state != State.chasing)
            {
                StopBehaviour();
                //chase player
                StartCoroutine(ChasePlayer());
            }
            else if (inWeaponCircle && state != State.attacking)
            {
                //stop
                StopBehaviour();

                //attack the player
                weaponSystem.StartMeleeAttackRepeatedlyCoroutine(player.gameObject,meleeWeapon);
                state = State.attacking;
            }
        }

     

        private void StopBehaviour()
        {
            StopAllCoroutines();
            weaponSystem.StopAllCoroutines(); //consider moving Coroutine methods calls into this class
            characterMovement.EnableWalk(false);
            characterMovement.SetDestination(transform.position); //stopping in place
        }

        private IEnumerator ReturnToOriginAfterDelay(float waitTimeToReturn)
        {
            yield return new WaitForSecondsRealtime(waitTimeToReturn);

            characterMovement.SetDestination(spawnPos);
        }

        private IEnumerator Patrol()
        {
            state = State.patrolling;
            yield return new WaitForFixedUpdate();
            while (true)
            {
                //work out where to go next
                Vector3 nextWaypointPos = patrolPath.transform.GetChild(nextWaypointIndex).position;
                //tell characterMovement to go there
                characterMovement.SetDestination(nextWaypointPos);
                //cycle waypoint when close
                CycleWayPointWhenClose(nextWaypointPos);
                //wait at waypoint
                yield return new WaitForSeconds(1f);
                characterMovement.EnableWalk(true);
            }
        }

        private void CycleWayPointWhenClose(Vector3 nextWaypointPos)
        {
            if (Vector3.Distance(transform.position, nextWaypointPos) <= waypointTolerance)
            {
                nextWaypointIndex = (nextWaypointIndex + 1) % patrolPath.transform.childCount;
            }
        }

        private IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >= currentWeaponRange)
            {
                characterMovement.SetDestination(player.transform.position);
                yield return new WaitForFixedUpdate();
            }
        }

        private bool InsideRangedCircle()
        {
            if (rangedEnemy)
            {
                bool insideRangedCircle = distanceToPlayer <= rangedEnemy.WeaponRange && distanceToPlayer > chaseRadius;
                return insideRangedCircle;
            }
            return false;
        }



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(transform.position, chaseRadius); //aggroRadius


            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, currentWeaponRange); // attackRadius
            if (rangedEnemy)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, rangedEnemy.WeaponRange); // attackRadius

            }
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}