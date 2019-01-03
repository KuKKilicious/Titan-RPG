using UnityEngine;
using UnityEngine.UI;
using RPG.CameraUI;
namespace RPG.Characters
{

    public class Energy : MonoBehaviour
    {
        [SerializeField]
        RawImage energyBar;
        [SerializeField]
        float maxEnergy = 100f;
        [SerializeField]
        float pointsPerHit = 10f;
        float currentEnergy;
        CameraRaycaster cameraRaycaster = null;

        // Use this for initialization
        void Start()
        {
            currentEnergy = maxEnergy;
            Debug.Log(currentEnergy);
            cameraRaycaster = Camera.main.GetComponent<CameraRaycaster>();
            cameraRaycaster.notifyMouseOverEnemy += CameraRaycaster_notifyMouseOverEnemy;
        }

        private void CameraRaycaster_notifyMouseOverEnemy(Enemy enemy)
        {
            if (Input.GetMouseButtonDown(1)) //TODO inRange criteria
            {
                UpdateEnergy();
                UpdateEnergyBar();
            }
        }

        private void UpdateEnergyBar()
        {
            float xValue = -(currentEnergy / maxEnergy / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        private void UpdateEnergy()
        {
            currentEnergy = Mathf.Clamp(currentEnergy - pointsPerHit, 0f, maxEnergy);
        }
    }
}
