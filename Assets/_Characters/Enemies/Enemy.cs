using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using RPG.Weapons;

namespace RPG.Characters
{
    public class Enemy : MonoBehaviour, IDamageable
    {
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float aggroRadius = 7;
        [SerializeField] float attackRadius = 3;
        [SerializeField] float damagePerShot = 7;
        [SerializeField] GameObject projectileToUse;
        [SerializeField] GameObject projectileSocket;


        [SerializeField] float secondsBetweenShots = 2f;
        bool isAttacking = false;
        AICharacterControl aiCharControl;
        Player player;
        float currentHealthPoints;

        private void Start()
        {
            currentHealthPoints = maxHealthPoints;
            aiCharControl = GetComponent<AICharacterControl>();
            player = FindObjectOfType<Player>();
        }

        private void Update()
        {
            CheckForPlayerInRange();
        }

        private void CheckForPlayerInRange()
        {
            float distanceToPlayer = Vector3.Distance(player.AimTransform.position, transform.position);
            if ((distanceToPlayer < attackRadius) && !isAttacking)
            {
                isAttacking = true;
                aiCharControl.target = transform; //stop moving
                StartCoroutine(SpawnProjectile(secondsBetweenShots));
            }

            if ((distanceToPlayer >= attackRadius))
            {
                isAttacking = false;
            }

            if (distanceToPlayer < aggroRadius && !isAttacking)
            {
                aiCharControl.target = player.transform;
            }
            else
            {
                aiCharControl.target = transform;
            }
        }

        //TODO seperate out Character firing logic
        private IEnumerator SpawnProjectile(float waitTime)
        {


            while (isAttacking)
            {


                var projectile = Instantiate(projectileToUse, projectileSocket.transform.position, Quaternion.identity);
                projectile.transform.rotation =
                    Quaternion.LookRotation(player.AimTransform.position -
                                            projectileSocket.transform.position); //rotate to player
                Projectile projectileComponent = projectile.GetComponent<Projectile>();
                projectileComponent.Shooter = gameObject;
                projectileComponent.setDamage(damagePerShot);


                Vector3 unitVectorToPlayer =
                    Vector3.Normalize(player.AimTransform.position - projectileSocket.transform.position);
                float projectileSpeed = projectileComponent.ProjectileSpeed;
                projectile.GetComponent<Rigidbody>().velocity = unitVectorToPlayer * projectileSpeed;
                yield return new WaitForSeconds(waitTime);
            }
        }

        void IDamageable.TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
            if (currentHealthPoints <= 0)
            {
                Destroy(gameObject);
            }

        }

        public float healthAsPercentage
        {
            get { return currentHealthPoints / maxHealthPoints; }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;

            Gizmos.DrawWireSphere(transform.position, aggroRadius); //aggroRadius


            Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(transform.position, attackRadius); // attackRadius
        }


    }
}