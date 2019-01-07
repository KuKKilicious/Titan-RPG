using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Core;
namespace RPG.Characters
{
    public class SelfHealBehaviour :AbilityBehaviour
    {
        
        public override void Use(AbilityUseParams abilityParams)
        {
            IDamageable damageable = GetComponent<IDamageable>();
            if (damageable!= null)
            {
                damageable.SubstractHealth(-(config as SelfHealConfig).HealAmount);

                //Play VFX
                PlayParticleEffect(gameObject);
                //play random SFX
                PlayAbilitySound();
                //audioPlayer.PlaySound(config.SFX);
            }
        }
 
        
    }
}
