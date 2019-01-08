using System;
using UnityEngine;
using UnityEngine.UI;
namespace RPG.Characters
{

    public class SpecialAbilities : MonoBehaviour
    {
        [SerializeField] AbilityConfig[] abilities;
        [SerializeField] Image energyBar;
        [SerializeField] private float maxEnergy = 100f;
        [SerializeField] private float regenPointsPerSecond = 1;
        [SerializeField] private AudioClip outOfEnergySound;
        //ToDO EnergySounds
        float currentEnergy;
        AudioSource audioSource;
        // Use this for initialization
        void Start()
        {
            currentEnergy = maxEnergy;
            audioSource = GetComponent<AudioSource>();
            AttachInitialAbbilities();
            UpdateEnergyBar();
        }
        bool isEnergyAvailable(float amount)
        {
            return (currentEnergy >= amount);
        }
        public void UpdateEnergy(float amount)
        {
            UpdateEnergyAmount(amount);
            UpdateEnergyBar();
        }

        private void Update()
        {
            RegenEnergy();
        }

        private void RegenEnergy()
        {
            UpdateEnergyAmount(-(regenPointsPerSecond * Time.deltaTime));
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

        private void AttachInitialAbbilities()
        {
            for (int i = 0; i < abilities.Length; i++)
            {
                abilities[i].AttachComponentTo(gameObject);
            }
        }

        public int GetNumberOfAbilities()
        {
            return abilities.Length;
        }

        internal  void AttemptSpecialAbility(int index, GameObject target = null)
        {
            if (index >= abilities.Length) { return; }

            AbilityConfig ability = abilities[index];
            float energyCost = ability.EnergyCost;
            if (isEnergyAvailable(energyCost))
            {
                //check in range
                UpdateEnergy(energyCost);
                //Use ability
                ability.Use(target);
            }
            else
            {
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(outOfEnergySound);
                }
            }
            //else {maybe play outofenergysound}
        }
    }
}
