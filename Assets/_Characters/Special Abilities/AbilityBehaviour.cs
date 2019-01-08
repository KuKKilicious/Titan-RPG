using System.Collections;
using UnityEngine;
namespace RPG.Characters
{
    public abstract class AbilityBehaviour : MonoBehaviour
    {
        GameObject particleObject;
        protected AbilityConfig config;
        const float PARTICLE_CLEAN_UP_DELAY = 20f;
        AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        public AbilityConfig SetConfig {
            set {
                config = value;
            }
        }
        public abstract void Use(GameObject target);

        protected void PlayParticleEffect(GameObject target)
        {
            //Instantiate a particle system prefab attached to gameObject
            particleObject =
                Instantiate(config.ParticlePrefab, target.transform.position, config.ParticlePrefab.transform.rotation);
            particleObject.transform.parent = transform;
            //Get the particle System
            ParticleSystem particles = particleObject.GetComponent<ParticleSystem>();
            //Play particle system
            particles.Play();
            //Destory particle system after finished  playing
            StartCoroutine(DestroyParticleWhenFinished(particleObject));
        }
        IEnumerator DestroyParticleWhenFinished(GameObject particleObject)
        {
            while (particleObject.GetComponent<ParticleSystem>())
            {
                yield return new WaitForSeconds(PARTICLE_CLEAN_UP_DELAY);
            }
            Destroy(particleObject);
            yield return new WaitForEndOfFrame();
        }
        protected void PlayAbilitySound()
        {
            AudioClip abilitySound = config.GetRandomSfx();
            if (abilitySound)
            {
                audioSource.PlayOneShot(abilitySound);
            }
        }
    }
}
