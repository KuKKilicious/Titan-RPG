using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Characters
{
    public class SelfHealBehaviour : MonoBehaviour, ISpecialAbility
    {
        SelfHealConfig config;
        GameObject effectParticleSystem;
        AudioSource audioSource;
        public SelfHealConfig SetConfig {
            set {
                config = value;
            }
        }
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public void Use(AbilityUseParams abilityParams)
        {
            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable!= null)
            {
                damageable.SubstractHealth(-config.HealAmount);

                //Play VFX
                PlayParticleEffect(gameObject);
                //play random SFX
                PlayRandomSFX(config.Sfx);
                //audioPlayer.PlaySound(config.SFX);
            }
        }
        private void PlayRandomSFX(AudioClip[] sfx)
        {

            int randomIndex = Random.Range(0, sfx.Length);
            audioSource.clip = sfx[randomIndex];
            audioSource.Play();
        }
        private void PlayParticleEffect(GameObject target)
        {
            //Instantiate a particle system prefab attached to player
            effectParticleSystem = Instantiate(config.ParticlePrefab, target.transform.position, Quaternion.identity);
            effectParticleSystem.transform.parent = transform;
            //Get the particle System
            ParticleSystem particles = effectParticleSystem.GetComponent<ParticleSystem>();
            //Play particle system
            particles.Play();
            //Destory particle system after finished  playing
            float destroyAfter = particles.main.startLifetime.constantMax + particles.main.duration;
            Destroy(effectParticleSystem, destroyAfter);
        }
    }
}
