﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using RPG.Core;
using RPG.Weapons;
namespace RPG.Characters
{


    public class Player : MonoBehaviour, IDamageable
    {
        [SerializeField]
        int enemyLayer = 10;
        [SerializeField]
        float maxHealthPoints = 100f;
        [SerializeField]
        float damageToDeal = 10f;
       
        [SerializeField]
        Weapon weaponInUse;

        private Animator animator;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideController;
        float currentHealthPoints;
        GameObject currentTarget;
        float lastHitTime = 0f;

        CameraRaycaster cameraRaycaster;


        [SerializeField]
        Transform aimTransform;
        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        public Transform AimTransform {
            get {
                return aimTransform;
            }
        }

        // Use this for initialization
        void Start()
        {
            animator = GetComponent<Animator>();
            SetCurrentMaxHealth();
            RegisterForMouseClick();
            PlaceWeaponInHand();
            OverrideAnimatorController();
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void OverrideAnimatorController()
        {
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController["DEFAULT_ATTACK"] = weaponInUse.AttackAnimation; //todo: remove string reference
        }

        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseClickObservers += CameraRaycaster_notifyMouseClickObservers;
        }

        private void PlaceWeaponInHand()
        {
            GameObject dominantHand = RequestDominantHand();
            var weapon = Instantiate(weaponInUse.WeaponPrefab, transform.position, Quaternion.identity, dominantHand.transform);
            weapon.transform.localPosition = weaponInUse.GripTransform.localPosition;
            weapon.transform.localRotation = weaponInUse.GripTransform.localRotation;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            Assert.IsFalse(dominantHands.Length < 1, "No DominantHand specified in player");
            Assert.IsFalse(dominantHands.Length > 1, "Multiple DominantHand specified in player");
            return dominantHands[0].gameObject;
        }

        private void CameraRaycaster_notifyMouseClickObservers(RaycastHit raycastHit, int layerHit)
        {
            if (layerHit == enemyLayer)
            {

                var enemy = raycastHit.collider.gameObject;
                currentTarget = enemy;
                HandleAttack();
            }
        }

        private void HandleAttack()
        {
            if (TargetIsInRange(currentTarget))
            {
                if (Time.time - lastHitTime >= weaponInUse.MinTimeBetweenHits)
                {
                    PlayAttackAnimation();
                    DealDamageToTarget();
                }
            }

            
            
        }

        private bool TargetIsInRange(GameObject target)
        {
            return (Vector3.Distance(transform.position, target.transform.position) <= weaponInUse.MaxAttackRange);
        }

        private void PlayAttackAnimation()
        { 
           
            animator.SetTrigger("Attack"); //make const

        }

        private void DealDamageToTarget()
        {

            currentTarget.GetComponent<IDamageable>().TakeDamage(damageToDeal);
            lastHitTime = Time.time;

        }



        // Update is called once per frame
        void Update()
        {

        }

        void IDamageable.TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
    }
}