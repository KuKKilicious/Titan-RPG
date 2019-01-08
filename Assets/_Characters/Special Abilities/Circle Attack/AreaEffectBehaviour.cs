using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{

    public class AreaEffectBehaviour : AbilityBehaviour
    {
        GameObject effectParticleSystem;

        public override void Use(GameObject target)
        {
            DealSphericalDamageAroundTarget(target);
            PlayAbilitySound();
        }

        private void DealSphericalDamageAroundTarget(GameObject target)
        {
            //play Particles
            PlayParticleEffect(target);
            Collider[] hits = Physics.OverlapSphere(target.transform.position, (config as AreaEffectConfig).Radius);

            float damageToDeal = (config as AreaEffectConfig).DamageToEachTarget;//move into loop, if considering enemy based adjustment
            foreach (Collider hit in hits)
            {
                var damageable = hit.gameObject.GetComponent<HealthSystem>();
                if (damageable != null && hit.gameObject != gameObject)
                {
                    damageable.SubstractHealth(damageToDeal);
                   
                }
            }
        }
    }
}
