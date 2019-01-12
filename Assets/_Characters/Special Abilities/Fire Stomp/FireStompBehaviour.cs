using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
using System;

namespace RPG.Characters
{

    public class FireStompBehaviour : AbilityBehaviour
    {
        
        GameObject effectParticleSystem;

        public override void Use(GameObject target)
        {
            StartCoroutine(DealSphericalDamageAroundTarget(target,(config as FireStompConfig).AnimationDelay));
            StartCoroutine(PlayAbilitySoundWithAnimation((config as FireStompConfig).AnimationDelay));
            PlayAbilityAnimation();
        }

        private IEnumerator PlayAbilitySoundWithAnimation(float animationDelay)
        {
            yield return new WaitForSecondsRealtime(animationDelay);
            PlayAbilitySound();
        }

        private IEnumerator DealSphericalDamageAroundTarget(GameObject target,float delay)
        {
            //StopMoving
            CharacterMovement characterMovement = GetComponent<CharacterMovement>();
            if (characterMovement)
            {
                characterMovement.DisableMovement();
            }
            //play Particles
            yield return new WaitForSeconds(delay);
            PlayParticleEffect(gameObject);
            Collider[] hits = Physics.OverlapSphere(transform.position, (config as FireStompConfig).Radius);

            float damageToDeal = (config as FireStompConfig).DamageToEachTarget;//move into loop, if considering enemy based adjustment
            foreach (Collider hit in hits)
            {
                var damageable = hit.gameObject.GetComponent<HealthSystem>();
                if (damageable != null && hit.gameObject != gameObject)
                {
                    damageable.SubstractHealth(damageToDeal);
                   
                }
            }
            if (characterMovement)
            {
                characterMovement.EnableMovement();
            }
        }
    }
}
