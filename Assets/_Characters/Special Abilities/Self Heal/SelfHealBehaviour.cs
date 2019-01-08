﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Characters
{
    public class SelfHealBehaviour :AbilityBehaviour
    {
        
        public override void Use(AbilityUseParams abilityParams)
        {
            HealthSystem damageable = GetComponent<HealthSystem>();
            if (damageable!= null)
            {
                damageable.HealCharacter((config as SelfHealConfig).HealAmount);

                //Play VFX
                PlayParticleEffect(gameObject);
                //play random SFX
                PlayAbilitySound();
                //audioPlayer.PlaySound(config.SFX);
            }
        }
 
        
    }
}
