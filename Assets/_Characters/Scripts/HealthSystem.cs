﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using Random = UnityEngine.Random;

namespace RPG.Characters
{
    [RequireComponent(typeof(AudioSource))]
    public class HealthSystem : MonoBehaviour
    {
        const string DEATH_TRIGGER = "Death";

        [SerializeField] float maxHealthPoints = 100f;
        [SerializeField] float deathVanishSeconds = 2f;
        [SerializeField] Image healthBar;
        [Header("SFX")]
        //SFX
        [SerializeField] private AudioClip[] deathSFX;
        [SerializeField] private AudioClip[] getHitSFX;

        private AudioSource audioSource = null;
        private Animator animator = null;
        CharacterMovement characterMovement;
        private bool isAlive = true;
        private float timeAtLastHitPlay = 0f;

        float currentHealthPoints;
        private void Start()
        {
            animator = GetComponent<Animator>();
            audioSource = GetComponent<AudioSource>();
            characterMovement = GetComponent<CharacterMovement>();
            SetCurrentMaxHealth();

        }
        public bool IsAlive {
            get {
                return isAlive;
            }
        }
        private void Update()
        {
            UpdateHealthBar();
        }

        private void UpdateHealthBar()
        {
            if (healthBar)
            {
                healthBar.fillAmount = healthAsPercentage;
            }
        }

        public float healthAsPercentage {
            get {
                return currentHealthPoints / maxHealthPoints;
            }
        }

        public void SubstractHealth(float damage)
        {
            if (isAlive)
            {
                bool characterDies = currentHealthPoints - damage <= 0;
                if (characterDies) //Player dies
                {
                    ReduceHealth(damage);
                    isAlive = false;
                    //Kill Player
                    StartCoroutine(KillCharacter());
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

        private IEnumerator KillCharacter()
        {
            float waitTime = 3f;
            //restrict Movement
            RestrictMovement();
            //trigger death animation
            animator.SetTrigger(DEATH_TRIGGER);
            PlayRandomSFX(deathSFX);
            var playerComponent = GetComponent<Player>();
            if (playerComponent && playerComponent.isActiveAndEnabled)
            {
                float timeScaleSloMo = 0.6f;
                //play death sound //TODO consider giving specific clips to player && Enemy
                //Slow time
                Time.timeScale = timeScaleSloMo;
                PlayDeathScreen(waitTime);
                //wait a bit
                yield return new WaitForSeconds(waitTime);
                //reload scene
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                Time.timeScale = 1f;

            }
            else //assumes Enemy Death
            {
                Destroy(gameObject, deathVanishSeconds);
            }
        }

        private void PlayDeathScreen(float waitTime)
        {
            UIManager uiManager = GameObject.FindObjectOfType<UIManager>();
            uiManager.PlayDeathScreen(); //consider including  waittime
        }

        private void RestrictMovement()
        {
            characterMovement.DisableMovement();
        }
        public void HealCharacter(float amount)
        {
            ReduceHealth(-amount);
        }

        private void PlayRandomSFX(AudioClip[] sfx)
        {
            if (sfx.Length > 0)
            {
                int randomIndex = Random.Range(0, sfx.Length);
                audioSource.PlayOneShot(sfx[randomIndex]);
            }
        }
        private void ReduceHealth(float damage)
        {
            currentHealthPoints = Mathf.Clamp(currentHealthPoints - damage, 0f, maxHealthPoints);
        }

        private void SetCurrentMaxHealth()
        {
            currentHealthPoints = maxHealthPoints;
        }

    }
}
