using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;
using System.Collections;

namespace RPG.Characters
{
    [RequireComponent(typeof(HealthSystem))]
    [RequireComponent(typeof(CharacterMovement))]
    [RequireComponent(typeof(WeaponSystem))]
    public class Player : MonoBehaviour
    {
        //trigger consts

        [Header("Properties")]

        //position to aim at
        [SerializeField] Transform aimTransform;

        private GameObject enemyObject = null; //todo needed?
        private HealthSystem healthSystem;
        private SpecialAbilities specialAbilities;
        private CharacterMovement characterMovement;
        private WeaponSystem weaponSystem;
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


        }


        #region Initialize Methods

        private void GetComponents()
        {
            healthSystem = GetComponent<HealthSystem>();
            specialAbilities = GetComponent<SpecialAbilities>();
            characterMovement = GetComponent<CharacterMovement>();
            weaponSystem = GetComponent<WeaponSystem>();
        }



        private void RegisterForMouseClick()
        {
            CameraRaycaster cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
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
                    StartCoroutine(MoveCloserToTargetAndCastSpell(keyIndex));
                }
        }

        //subscriber method
        private void CameraRaycaster_notifyMouseOverWalkableObservers(Vector3 destination)
        {
            if (!healthSystem.IsAlive) { return; }
            if (Input.GetMouseButton(0))
            {
                StopAllCoroutines();
                characterMovement.SetDestination(destination);
            }
        }

        private IEnumerator MoveCloserToTargetAndAttack()
        {
            bool targetStillAliveAndNotInRange = !TargetIsInRange(enemyObject) && (enemyObject.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon);

            while (targetStillAliveAndNotInRange)
            {

                characterMovement.SetDestination(enemyObject.transform.position);
                yield return new WaitForSecondsRealtime(0.1f);
                targetStillAliveAndNotInRange = !TargetIsInRange(enemyObject) && (enemyObject.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon);
            }
            characterMovement.SetDestination(transform.position);

            weaponSystem.HandleAttack(enemyObject);

        }

        private IEnumerator MoveCloserToTargetAndCastSpell(int keyIndex)
        {
            bool targetStillAliveAndNotInRange = !specialAbilities.TargetIsInSpellRange(keyIndex,enemyObject) && (enemyObject.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon);

            while (targetStillAliveAndNotInRange)
            {

                characterMovement.SetDestination(enemyObject.transform.position);
                yield return new WaitForSecondsRealtime(0.1f);
                targetStillAliveAndNotInRange = !TargetIsInRange(enemyObject) && (enemyObject.GetComponent<HealthSystem>().healthAsPercentage >= Mathf.Epsilon);
            }
            characterMovement.SetDestination(transform.position);

            specialAbilities.AttemptSpecialAbility(keyIndex, enemyObject);

        }
        private bool TargetIsInRange(GameObject target)
        {
            return (Vector3.Distance(transform.position, target.transform.position) <= weaponSystem.GetWeaponConfig().MaxAttackRange);
        }
        private void CameraRaycaster_notifyMouseOverEnemy(EnemyAI newEnemy)
        {

            if (!healthSystem.IsAlive) { return; }

            enemyObject = newEnemy.gameObject;
            if (Input.GetMouseButton(0))
            {
                StartCoroutine(MoveCloserToTargetAndAttack());
            }
            else if (Input.GetMouseButtonDown(1))
            {
                StartCoroutine(MoveCloserToTargetAndCastSpell(0));
            }
        }

    }
}
