using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using RPG.CameraUI;
using RPG.Core;

namespace RPG.Characters
{
    public class Player : MonoBehaviour, IDamageable
    {
        //trigger consts
        const string ATTACK_TRIGGER = "Attack";
        const string DEATH_TRIGGER = "Death";
        const string DEFAULT_ATTACK = "DEFAULT_ATTACK";

        [Header("SFX")]
        //SFX
        [SerializeField] private AudioClip[] deathSFX;
        [SerializeField] private AudioClip[] getHitSFX;

        [Header("Stats")]
        //stats
        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float baseDamage = 10f;
        [SerializeField] ParticleSystem criticalParticles = null;
        //weapon
        [SerializeField] Weapon weaponConfigInUse;
        [SerializeField] private AnimatorOverrideController animatorOverrideController;

        //position to aim at
        [SerializeField] Transform aimTransform;

       

        //TODO remove Serialize Field of specialAbillity
        [SerializeField] AbilityConfig[] abilities;
        [Header("Criticals")]
        [Range(.1f, 1f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;

        float currentHealthPoints;
        float lastHitTime = 0f;
        private float timeAtLastHitPlay = 0f;

        private bool isAlive = true;
        private CameraRaycaster cameraRaycaster;
        private Animator animator = null;
        private Energy energy = null;
        private AudioSource audioSource = null;
        private Enemy enemy = null;
        private GameObject weaponObject;
        #region Getter
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
        #endregion

        // Use this for initialization
        void Start()
        {
            GetComponents();
            SetCurrentMaxHealth();
            RegisterForMouseClick();
            PutWeaponInHand(weaponConfigInUse);
            SetAttackAnimation();
            AttachInitialAbbilities();

        }



        #region Initialize Methods

        private void GetComponents()
        {
            energy = GetComponent<Energy>();
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
        }
        private void AttachInitialAbbilities()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i].AttachComponentTo(gameObject);
            }
        }


        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

        private void SetAttackAnimation()
        {
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = weaponConfigInUse.AttackAnimation; 
        }
        private void RegisterForMouseClick()
        {
            cameraRaycaster = FindObjectOfType<CameraRaycaster>();
            cameraRaycaster.notifyMouseOverEnemy += CameraRaycaster_notifyMouseOverEnemy;

        }
        #endregion

        private void Update()
        {
            if (isAlive)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex < abilities.Length; keyIndex++)
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    AttemptSpecialAbility(abilities[keyIndex]);
                }
        }

        private void CameraRaycaster_notifyMouseOverEnemy(Enemy newEnemy)
        {
            enemy = newEnemy;
            if (Input.GetMouseButton(0))
            {
                HandleAttack(enemy);
            }
            else if (Input.GetMouseButtonDown(1)) //TODO inRange criteria
            {
                AttemptSpecialAbility(abilities[0]);
            }
        }

        private void AttemptSpecialAbility(AbilityConfig ability)
        {
            float energyCost = ability.EnergyCost;
            if (energy.isEnergyAvailable(energyCost))
            {
                //check in range
                energy.UpdateEnergy(energyCost);
                //Use ability
                var abilityParams = new AbilityUseParams(enemy, baseDamage);
                ability.Use(abilityParams);
            }
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
                if (Time.time - lastHitTime >= weaponConfigInUse.MinTimeBetweenHits)
                {
                    SetAttackAnimation();
                    PlayAttackAnimation();
                    DealDamageToTarget(enemy);
                }
            }

        }

        private bool TargetIsInRange(Enemy target)
        {
            return (Vector3.Distance(transform.position, target.transform.position) <= weaponConfigInUse.MaxAttackRange);
        }

        private void PlayAttackAnimation()
        {

            animator.SetTrigger(ATTACK_TRIGGER); //make const

        }

        private void DealDamageToTarget(Enemy enemy)
        {
            float damage = CalculateDamage();
            enemy.GetComponent<IDamageable>().SubstractHealth(damage);
            lastHitTime = Time.time;

        }

        private float CalculateDamage()
        {
            float damage = baseDamage +weaponConfigInUse.AdditionalDamage;
            //critical hit
            if(Random.value <= criticalHitChance)
            {
                damage *= criticalHitMultiplier;
                criticalParticles.Play();
            }
            return damage;
        }

        void IDamageable.SubstractHealth(float damage)
        {
            if (isAlive)
            {
                if (currentHealthPoints - damage <= 0) //Player dies
                {
                    ReduceHealth(damage);
                    isAlive = false;
                    //Kill Player
                    StartCoroutine(KillPlayer());
                }
                else
                {
                    ReduceHealth(damage);
                    if (timeAtLastHitPlay < Time.time - Random.Range(0.4f, 0.7f))//TODO switch magic number to var
                    {
                        PlayRandomSFX(getHitSFX);
                        timeAtLastHitPlay = Time.time;
                    }
                }
            }
        }

        private void PlayRandomSFX(AudioClip[] sfx)
        {

            int randomIndex = Random.Range(0, sfx.Length);
            audioSource.clip = sfx[randomIndex];
            audioSource.Play();
        }

        private IEnumerator KillPlayer()
        {
            float timeScaleSloMo = 0.6f;
            int waitTime = 3;
            //play death sound
            PlayRandomSFX(deathSFX);
            //trigger death animation
            animator.SetTrigger(DEATH_TRIGGER);
            //restrict Movement
            RestrictMovement();
            //Slow time
            Time.timeScale = timeScaleSloMo;
            //wait a bit
            yield return new WaitForSecondsRealtime(waitTime);
            //reload scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            Time.timeScale = 1f;
        }

        private void RestrictMovement()
        {
            CharacterMovement movement = GetComponent<CharacterMovement>();
            if (movement)
            {
                movement.StopMovement();
            }
        }

        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }
       
        internal void PutWeaponInHand(Weapon weaponConfig)
        {
            weaponConfigInUse = weaponConfig;
            GameObject dominantHand = RequestDominantHand();
            Destroy(weaponObject); //empty hand
            weaponObject = Instantiate(weaponConfigInUse.WeaponPrefab, transform.position, Quaternion.identity, dominantHand.transform);
            weaponObject.transform.localPosition = weaponConfigInUse.GripTransform.localPosition;
            weaponObject.transform.localRotation = weaponConfigInUse.GripTransform.localRotation;
        }
        public GameObject GetGameObject()
        {
            return gameObject;
        }
    }
}
