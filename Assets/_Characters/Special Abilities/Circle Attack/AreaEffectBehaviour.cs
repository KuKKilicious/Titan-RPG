﻿using System.Collections;
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
            StartCoroutine(DealSphericalDamageAroundTarget(target,(config as AreaEffectConfig).AnimationDelay));
            StartCoroutine(PlayAbilitySoundWithAnimation((config as AreaEffectConfig).AnimationDelay));
            PlayAbilityAnimation();
        }

        private IEnumerator PlayAbilitySoundWithAnimation(float animationDelay)
        {
            yield return new WaitForSecondsRealtime(animationDelay);
            PlayAbilitySound();
        }

        private IEnumerator DealSphericalDamageAroundTarget(GameObject target,float delay)
        {
            transform.LookAt(target.transform);
            //StopMoving
            CharacterMovement characterMovement = GetComponent<CharacterMovement>();
            if (characterMovement)
            {
                characterMovement.DisableMovement();
            }
            //play Particles
            yield return new WaitForSeconds(delay);
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
            if (characterMovement)
            {
                characterMovement.EnableMovement();
            }
        }
    }
}
