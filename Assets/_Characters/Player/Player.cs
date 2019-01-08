using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(CharacterMovement))]
    public class Player : MonoBehaviour
    {
        //trigger consts
        const string ATTACK_TRIGGER = "Attack";
        const string DEFAULT_ATTACK = "DEFAULT_ATTACK";

  

        [Header("Stats")]
        //stats
        [SerializeField] float baseDamage = 10f;
        [SerializeField] ParticleSystem criticalParticles = null;
        //weapon
        [SerializeField] Weapon weaponConfigInUse;
        [SerializeField] private AnimatorOverrideController animatorOverrideController;

        //position to aim at
        [SerializeField] Transform aimTransform;

       

        //TODO remove Serialize Field of specialAbillity
        [Header("Criticals")]
        [Range(.1f, 1f)] [SerializeField] float criticalHitChance = 0.1f;
        [SerializeField] float criticalHitMultiplier = 1.25f;
        float lastHitTime = 0f;

        private CameraRaycaster cameraRaycaster;
        private Animator animator = null;
        private GameObject enemyObject = null;
        private GameObject weaponObject;
        private HealthSystem healthSystem;
        private SpecialAbilities specialAbilities;
        private CharacterMovement characterMovement;

        #region Getter


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
            RegisterForMouseClick();
            PutWeaponInHand(weaponConfigInUse);
            SetAttackAnimation();

        }
        private void SetupRaycaster()
        {
           

        }
   

        #region Initialize Methods

        private void GetComponents()
        {
            animator = GetComponent<Animator>();
            healthSystem = GetComponent<HealthSystem>();
            specialAbilities = GetComponent<SpecialAbilities>();
            characterMovement = GetComponent<CharacterMovement>();
        }
  

        private void SetAttackAnimation()
        {
            animator.runtimeAnimatorController = animatorOverrideController;
            animatorOverrideController[DEFAULT_ATTACK] = weaponConfigInUse.AttackAnimation; 
        }
        private void RegisterForMouseClick()
        {
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyMouseOverEnemy += CameraRaycaster_notifyMouseOverEnemy;
            cameraRaycaster.notifyMouseOverWalkableObservers += CameraRaycaster_notifyMouseOverWalkableObservers;

        }
        #endregion

        private void Update()
        {
            if (healthSystem.IsAlive)
            {
                ScanForAbilityKeyDown();
            }
        }

        private void ScanForAbilityKeyDown()
        {
            for (int keyIndex = 1; keyIndex <= specialAbilities.GetNumberOfAbilities(); keyIndex++)
                if (Input.GetKeyDown(keyIndex.ToString()))
                {
                    specialAbilities.AttemptSpecialAbility(keyIndex,enemyObject);
                }
        }

        //subscriber method
        private void CameraRaycaster_notifyMouseOverWalkableObservers(Vector3 destination)
        {
            if (!healthSystem.IsAlive) { return; }
            if (Input.GetMouseButton(0))
            {
                characterMovement.SetDestination(destination);
            }
        }

        private void CameraRaycaster_notifyMouseOverEnemy(Enemy newEnemy)
        {

            if (!healthSystem.IsAlive) { return; }

            enemyObject = newEnemy.gameObject;
            if (Input.GetMouseButton(0))
            {
                characterMovement.SetDestination(enemyObject.transform.position);
                HandleAttack(newEnemy);
            }
            else if (Input.GetMouseButtonDown(1)) //TODO inRange criteria
            {
                specialAbilities.AttemptSpecialAbility(0,enemyObject);
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
            enemy.GetComponent<HealthSystem>().SubstractHealth(damage);
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
