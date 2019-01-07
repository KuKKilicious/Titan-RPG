using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    public class PowerAttackBehaviour : AbilityBehaviour
    {

        public override void Use(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + (config as PowerAttackConfig).ExtraDamage;
            useParams.target.SubstractHealth(damageToDeal);
            PlayAbilitySound();
        }
    }
}
