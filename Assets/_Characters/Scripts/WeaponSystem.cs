using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
namespace RPG.Characters
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(HealthSystem))]
    public class WeaponSystem : MonoBehaviour
    {
        //consts
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT_ATTACK";


        [Header("Criticals")]
        [Range(0f, 1f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        [SerializeField] ParticleSystem criticalParticles = null;


        [Header("Properties")]
        [SerializeField] float baseDamage = 10f;
        [SerializeField] WeaponConfig weaponConfigInUse;

        float lastHitTime = 0f;
        private GameObject weaponObject;
        private GameObject target;
        private Animator animator = null;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            PutWeaponInHand(weaponConfigInUse);
            SetAttackAnimation(weaponConfigInUse);
        }
        public WeaponConfig GetWeaponConfig()
        {
            return weaponConfigInUse;
        }
        public void HandleAttack(GameObject target)
        {
            this.target = target;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;

            if (TargetIsInRange(target,weaponConfigInUse) && targetStillAlive)
            {

                if (timeToHit(weaponConfigInUse))
                {
                    AttackTargetOnce(weaponConfigInUse);
                }
            }

        }



        public void StartMeleeAttackRepeatedlyCoroutine(GameObject targetToAttack,WeaponConfig weapon)
        {
            PutWeaponInHand(weapon);
            target = targetToAttack;
            StartCoroutine(AttackTargetRepeatedly(weaponConfigInUse));
        }
        private IEnumerator AttackTargetRepeatedly(WeaponConfig weapon)
        {

            //determine if alive (attacker and defender)
            bool attackerStillAlive = GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            bool targetStillAlive = target.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon;
            //while still alive
            while (attackerStillAlive && targetStillAlive)
            {

                float weaponHitPeriod = weapon.AttackAnimation.length + weapon.MinTimeBetweenHits;
                float timetoWait = weaponHitPeriod * animator.speed;
                //if time to hit again
                if (timeToHit(weapon) && TargetIsInRange(target,weapon))
                {
                    //hit target
                    AttackTargetOnce(weapon);
                    lastHitTime = Time.time;
                }
                yield return new WaitForSeconds(timetoWait);
            }

        }

        internal void StartRangedAttackRepeatedlyCoroutine(WeaponConfig rangedWeapon,GameObject target)
        {
            PutWeaponInHand(rangedWeapon);
            this.target = target;
            StartCoroutine(AttackTargetRepeatedly(rangedWeapon));
        }

        bool timeToHit(WeaponConfig weapon)
        {
            float weaponHitPeriod = weapon.AttackAnimation.length + weapon.MinTimeBetweenHits;
            float timetoWait = weaponHitPeriod * animator.speed;
            bool isTimeToHitAgain = Time.time - lastHitTime > timetoWait;
            return isTimeToHitAgain;
        }


        private void AttackTargetOnce(WeaponConfig weapon)
        {
            transform.LookAt(target.transform);

            SetAttackAnimation(weapon);
            PlayAttackAnimation();
            //todo consider checking for range
            StartCoroutine(DealDamageAfterDelay(weapon.DamageDelay)); // todo read from weaponconfiginUse
        }

        private IEnumerator DealDamageAfterDelay(float v)
        {
            lastHitTime = Time.time;
            yield return new WaitForSeconds(v);
            float damage = CalculateDamage();
            target.GetComponent<HealthSystem>().SubstractHealth(damage);
        }

        private void SetAttackAnimation(WeaponConfig weapon)
        {
            var animatorOverrideController = GetComponent<CharacterMovement>().AnimatorOverrideController;
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = weapon.AttackAnimation;
        }
        internal void PutWeaponInHand(WeaponConfig weaponConfig)
        {
            weaponConfigInUse = weaponConfig;
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); //empty hand
            if (weaponConfigInUse.WeaponPrefab)
            {
                weaponObject = Instantiate(weaponConfigInUse.WeaponPrefab, transform.position, Quaternion.identity, dominantHand.transform);
                weaponObject.transform.localPosition = weaponConfigInUse.GripTransform.localPosition;
                weaponObject.transform.localRotation = weaponConfigInUse.GripTransform.localRotation;
            }
        }
        private float CalculateDamage()
        {
            float damage = baseDamage + weaponConfigInUse.AdditionalDamage;
            //critical hit
            if (Random.value <= criticalHitChance)
            {
                damage *= criticalHitMultiplier;
                if (criticalParticles != null)
                {
                    criticalParticles.Play();
                }
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
            animator.SetTrigger(ATTACK_TRIGGER);
        }
        private void DealDamageToTarget(GameObject target)
        {
            float damage = CalculateDamage();
            target.GetComponent<HealthSystem>().SubstractHealth(damage);
            lastHitTime = Time.time;
        }

        private bool TargetIsInRange(GameObject target, WeaponConfig weapon)
        {
            return (Vector3.Distance(transform.position, target.transform.position) <= weapon.MaxAttackRange);
        }


    }
}
