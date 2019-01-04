using System;
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

        //stats
        [SerializeField]
        float maxHealthPoints = 100f;
        [SerializeField]
        float baseDamage = 10f;

        //weapon
        [SerializeField]
        Weapon weaponInUse;

        //animator
        private Animator animator;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideController;
        //position to aim at
        [SerializeField]
        Transform aimTransform;

        //TODO remove Serialize Field of specialAbillity
        [SerializeField]
        SpecialAbility[] abilities;
        CameraRaycaster cameraRaycaster;

        float currentHealthPoints;
        float lastHitTime = 0f;
        Energy energy = null;
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
            energy = GetComponent<Energy>();
            animator = GetComponent<Animator>();
            SetCurrentMaxHealth();
            RegisterForMouseClick();
            PlaceWeaponInHand();
            OverrideAnimatorController();
            abilities[0].AttachComponentTo(gameObject);
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
            cameraRaycaster.notifyMouseOverEnemy += CameraRaycaster_notifyMouseOverEnemy;

        }

        private void CameraRaycaster_notifyMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButton(0))
            {
                HandleAttack(enemy);
            }
            else if (Input.GetMouseButtonDown(1)) //TODO inRange criteria
            {
                AttemptSpecialAbility(0, enemy);
            }
        }

        private void AttemptSpecialAbility(int abilityIndex, Enemy enemy)
        {
            float energyCost = abilities[abilityIndex].EnergyCost;
            if (energy.isEnergyAvailable(energyCost))
            {
                energy.UpdateEnergy(energyCost);
                //Use ability
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                abilities[0].Use(abilityParams);
            }
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


        private void HandleAttack(Enemy enemy)
        {
            if (TargetIsInRange(enemy))
            {
                if (Time.time - lastHitTime >= weaponInUse.MinTimeBetweenHits)
                {
                    PlayAttackAnimation();
                    DealDamageToTarget(enemy);
                }
            }

        }

        private bool TargetIsInRange(Enemy target)
        {
            return (Vector3.Distance(transform.position, target.transform.position) <= weaponInUse.MaxAttackRange);
        }

        private void PlayAttackAnimation()
        {

            animator.SetTrigger("Attack"); //make const

        }

        private void DealDamageToTarget(Enemy enemy)
        {

            enemy.GetComponent<IDamageable>().TakeDamage(baseDamage);
            lastHitTime = Time.time;

        }

        void IDamageable.TakeDamage(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
    }
}
