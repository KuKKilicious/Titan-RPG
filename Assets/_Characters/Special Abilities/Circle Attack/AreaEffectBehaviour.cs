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
            //play Particles
            PlayParticleEffect(useParams.target.GetGameObject());
            Collider[] hits = Physics.OverlapSphere(useParams.target.GetGameObject().transform.position,config.Radius);

            float damageToDeal = useParams.baseDamage + config.DamageToEachTarget;//move into loop, if considering enemy based adjustment
            foreach (Collider hit in hits)
            {
                var damageable = hit.gameObject.GetComponent<IDamageable>();
                if (damageable != null && hit.gameObject != gameObject)
                {
                    damageable.SubstractHealth(damageToDeal);
                   
                }
            }
        }
    }
}
