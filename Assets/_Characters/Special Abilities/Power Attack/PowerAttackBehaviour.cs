using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    public class PowerAttackBehaviour : AbilityBehaviour
    {

        public override void Use(GameObject target)
        {
            float damageToDeal = (config as PowerAttackConfig).ExtraDamage;
            HealthSystem healthSystem = target.GetComponent<HealthSystem>();
            if (healthSystem)
            {
                healthSystem.SubstractHealth(damageToDeal);
                PlayAbilitySound();
            }
        }
    }
}
