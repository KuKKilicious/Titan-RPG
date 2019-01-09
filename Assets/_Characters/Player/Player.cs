using UnityEngine;
using UnityEngine.Assertions;
using RPG.CameraUI;

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

        private CameraRaycaster cameraRaycaster;
        private GameObject enemyObject = null;
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

        private void CameraRaycaster_notifyMouseOverEnemy(EnemyAI newEnemy)
        {

            if (!healthSystem.IsAlive) { return; }

            enemyObject = newEnemy.gameObject;
            if (Input.GetMouseButton(0))
            {
                characterMovement.SetDestination(enemyObject.transform.position);
                weaponSystem.HandleAttack(newEnemy.gameObject);
            }
            else if (Input.GetMouseButtonDown(1)) //TODO inRange criteria
            {
                specialAbilities.AttemptSpecialAbility(0,enemyObject);
            }
        }
    
    }
}
