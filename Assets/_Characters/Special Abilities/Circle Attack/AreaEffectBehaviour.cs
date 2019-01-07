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

        public override void Use(AbilityUseParams useParams)
        {
            DealSphericalDamageAroundTarget(useParams);
            PlayAbilitySound();
        }

        private void DealSphericalDamageAroundTarget(AbilityUseParams useParams)
        {
            //play Particles
            PlayParticleEffect(useParams.target.GetGameObject());
            Collider[] hits = Physics.OverlapSphere(useParams.target.GetGameObject().transform.position, (config as AreaEffectConfig).Radius);

            float damageToDeal = useParams.baseDamage + (config as AreaEffectConfig).DamageToEachTarget;//move into loop, if considering enemy based adjustment
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
