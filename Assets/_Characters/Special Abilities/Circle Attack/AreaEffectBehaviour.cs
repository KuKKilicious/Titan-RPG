using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{

    public class AreaEffectBehaviour : MonoBehaviour, ISpecialAbility
    {
        AreaEffectConfig config;
        GameObject effectParticleSystem;
        public AreaEffectConfig SetConfig {
            set {
                config = value;
            }
        }
        public void Use(AbilityUseParams useParams)
        {
            DealSphericalDamageAroundTarget(useParams);
            
        }
        private void PlayParticleEffect(GameObject target)
        {
            //Instantiate a particle system prefab attached to player
            effectParticleSystem = Instantiate(config.ParticlePrefab, target.transform.position, Quaternion.identity);
            //Get the particle System
            ParticleSystem particles = effectParticleSystem.GetComponent<ParticleSystem>();
            //Play particle system
            particles.Play();
            //Destory particle system after finished  playing
            float destroyAfter = particles.main.startLifetime.constantMax + particles.main.duration;
            Destroy(effectParticleSystem, destroyAfter);
        }

        private void DealSphericalDamageAroundTarget(AbilityUseParams useParams)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //play Particles
            RaycastHit hitInfo;//TODO change way to generally determine target
            Physics.Raycast(ray, out hitInfo, 100f);//TODO switch magic number
            PlayParticleEffect(hitInfo.collider.gameObject);
            RaycastHit[] hits = Physics.SphereCastAll(ray, config.Radius);

            float damageToDeal = useParams.baseDamage + config.DamageToEachTarget;//move into loop, if considering enemy based adjustment
            foreach (RaycastHit hit in hits)
            {
                var damageable = hit.collider.gameObject.GetComponent<IDamageable>();
                if (damageable != null && hit.collider.gameObject != gameObject)
                {
                    damageable.SubstractHealth(damageToDeal);
                   
                }
            }
        }
    }
}
