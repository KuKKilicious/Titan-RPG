using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
namespace RPG.Characters
{
    [RequireComponent(typeof(Animator))]
    public class WeaponSystem : MonoBehaviour
    {
        //consts
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT_ATTACK";


        [Header("Criticals")]
        [Range(.1f, 1f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalParticles = null;


        [Header("Properties")]
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig weaponConfigInUse;

        float lastHitTime = 0f;
        private GameObject weaponObject;
        private Animator animator = null;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            PutWeaponInHand(weaponConfigInUse);
            SetAttackAnimation();
        }

        public void HandleAttack(GameObject target)
        {
            if (TargetIsInRange(target))
            {
                if (Time.time - lastHitTime >= weaponConfigInUse.MinTimeBetweenHits)
                {
                    SetAttackAnimation();
                    PlayAttackAnimation();
                    DealDamageToTarget(target);
                }
            }

        }


        // Update is called once per frame
        void Update()
        {

        }
        private void SetAttackAnimation()
        {
            var animatorOverrideController = GetComponent<CharacterMovement>().AnimatorOverrideController;
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = weaponConfigInUse.AttackAnimation;
        }
        internal void PutWeaponInHand(WeaponConfig weaponConfig)
        {
            weaponConfigInUse = weaponConfig;
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); //empty hand
            weaponObject = Instantiate(weaponConfigInUse.WeaponPrefab, transform.position, Quaternion.identity, dominantHand.transform);
            weaponObject.transform.localPosition = weaponConfigInUse.GripTransform.localPosition;
            weaponObject.transform.localRotation = weaponConfigInUse.GripTransform.localRotation;
        }
        private float CalculateDamage()
        {
            float damage = baseDamage + weaponConfigInUse.AdditionalDamage;
            //critical hit
            if (Random.value <= criticalHitChance)
            {
                damage *= criticalHitMultiplier;
                criticalParticles.Play();
            }
            return damage;
        }

        private GameObject RequestDominantHand()
        {
            var dominantHands = GetComponentsInChildren<DominantHand>();
            Assert.IsFalse(dominantHands.Length < 1, "No DominantHand specified in player");
            Assert.IsFalse(dominantHands.Length > 1, "Multiple DominantHand specified in player");
            return dominantHands[0].gameObject;
        }
        private void PlayAttackAnimation()
        {
            animator.SetTrigger(ATTACK_TRIGGER); //make const
        }
        private void DealDamageToTarget(GameObject target)
        {
            float damage = CalculateDamage();
            target.GetComponent<HealthSystem>().SubstractHealth(damage);
            lastHitTime = Time.time;

        }

        private bool TargetIsInRange(GameObject target)
        {
            return (Vector3.Distance(transform.position, target.transform.position) <= weaponConfigInUse.MaxAttackRange);
        }


    }
}
