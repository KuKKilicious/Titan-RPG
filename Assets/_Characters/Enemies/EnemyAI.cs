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

        //todo remove
        [SerializeField] float chaseRadius = 7;
        [SerializeField] WaypointContainer patrolPath;
        [SerializeField] float waypointTolerance = 2f;
        [SerializeField] float waitTimeBetweenAttacks = 3f;

        float currentWeaponRange = 3;
        float distanceToPlayer = 0f;
        enum State { idle, attacking, patrolling, chasing }
        State state = State.idle;
        bool isAttacking = false;
        int nextWaypointIndex;
        Player player;
        CharacterMovement characterMovement;
        WeaponSystem weaponSystem;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            characterMovement = GetComponent<CharacterMovement>();
            weaponSystem = GetComponent<WeaponSystem>();
        }

        private void Update()
        {
            weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetWeaponConfig().MaxAttackRange;

            distanceToPlayer = Vector3.Distance(player.AimTransform.position, transform.position);

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                //stop 
                StopAllCoroutines();
                if (patrolPath != null)
                {
                    //start patrolling
                    StartCoroutine(Patrol());
                }
                else
                {
                    //start idling 
                }
            }
            if (distanceToPlayer <= chaseRadius && state != State.chasing)
            {
                StopAllCoroutines();
                //chase player
                StartCoroutine(ChasePlayer());
            }
            if (distanceToPlayer <= currentWeaponRange && state != State.attacking)
            {
                //stop
                StopAllCoroutines();
                //attack the player
                weaponSystem.StartAttackTargetRepeatedlyCoroutine(player.gameObject);
                state = State.attacking;
            }
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



        //TODO seperate out Character firing logic
        //private IEnumerator SpawnProjectile()
        //{


        //    while (isAttacking)
        //    {
        //        float variation = GetRandomVariation();

        //        var projectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
        //        projectile.transform.rotation =
        //            Quaternion.LookRotation(player.AimTransform.position -
        //                                    projectileSocket.transform.position); //rotate to player
        //        Projectile projectileComponent = projectile.GetComponent<Projectile>();
        //        projectileComponent.Shooter = gameObject;
        //        projectileComponent.setDamage(damagePerShot);


        //        Vector3 unitVectorToPlayer =
        //            Vector3.Normalize(player.AimTransform.position - projectileSocket.transform.position);
        //        float projectileSpeed = projectileComponent.ProjectileSpeed;
        //        projectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
        //        yield return new WaitForSeconds(variation);
        //    }
        //}

        //private float GetRandomVariation()
        //{
        //    float variationMin = secondsBetweenShots + variationInSec;
        //    float variationMax = secondsBetweenShots - variationInSec;
        //    float variation = Random.Range(variationMin, variationMax);
        //    return variation;
        //}



        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(transform.position, chaseRadius); //aggroRadius


            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, currentWeaponRange); // attackRadius
        }

        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}