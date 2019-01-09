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


        float currentWeaponRange = 3;
        float distanceToPlayer = 0f;
        enum State { idle, attacking, patrolling, chasing }
        State state = State.idle;
        bool isAttacking = false;
        Player player;
        CharacterMovement characterMovement;

        private void Start()
        {
            player = FindObjectOfType<Player>();
            characterMovement = GetComponent<CharacterMovement>();
        }

        private void Update()
        {
            WeaponSystem weaponSystem = GetComponent<WeaponSystem>();
            currentWeaponRange = weaponSystem.GetWeaponConfig().MaxAttackRange;

             distanceToPlayer = Vector3.Distance(player.AimTransform.position, transform.position);

            if (distanceToPlayer > chaseRadius && state != State.patrolling)
            {
                //stop 
                StopAllCoroutines();
                //start patrolling
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
            }
        }

        private IEnumerator ChasePlayer()
        {
            state = State.chasing;
            while (distanceToPlayer >=currentWeaponRange)
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