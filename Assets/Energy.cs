using System;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Characters
{

    public class Energy : MonoBehaviour
    {
        [SerializeField]
        Image energyBar;
        [SerializeField]
        private float maxEnergy = 100f;
        [SerializeField]
        private float regenPointsPerSecond = 1;
        float currentEnergy;
        // Use this for initialization
        void Start()
        {
            currentEnergy = maxEnergy;
        }

        private void Update()
        {
            RegenEnergy();
        }

        private void RegenEnergy()
        {
                UpdateEnergyAmount(-(regenPointsPerSecond*Time.deltaTime));
                UpdateEnergyBar();
        }

        public bool isEnergyAvailable(float amount)
        {
            return (currentEnergy >= amount);
        }
        public void UpdateEnergy(float amount)
        {
            UpdateEnergyAmount(amount);
            UpdateEnergyBar();
        }

        private void UpdateEnergyAmount(float amount)
        {
            currentEnergy = Mathf.Clamp(currentEnergy - amount, 0f, maxEnergy);
        }
        private void UpdateEnergyBar()
        {
            energyBar.fillAmount = currentEnergy / maxEnergy;
        }

    }
}
