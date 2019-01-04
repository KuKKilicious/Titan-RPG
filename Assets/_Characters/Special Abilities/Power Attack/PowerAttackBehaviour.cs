﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{

    public class PowerAttackBehaviour : MonoBehaviour, ISpecialAbility
    {
        PowerAttackConfig config;

        public PowerAttackConfig SetConfig {
            set {
                config = value;
            }
        }

       

        public void Use(AbilityUseParams useParams)
        {
            float damageToDeal = useParams.baseDamage + config.ExtraDamage;
            useParams.target.TakeDamage(damageToDeal);
        }
    }
}
