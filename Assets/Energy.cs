using System.Collections;
using System.Collections.Generic;
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
            cameraRaycaster.notifyRightMouseClickObservers += CameraRaycaster_notifyRightMouseClickObservers;
        }

        private void CameraRaycaster_notifyRightMouseClickObservers(RaycastHit raycastHit, int layerHit)
        {
            Debug.Log(currentEnergy +"b");
            currentEnergy = Mathf.Clamp(currentEnergy - pointsPerHit, 0f, maxEnergy);
            Debug.Log(currentEnergy);
            float xValue = -(currentEnergy/maxEnergy / 2f) - 0.5f;
            energyBar.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
