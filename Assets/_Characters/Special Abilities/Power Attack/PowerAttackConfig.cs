using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Special Ability/Power Attack")]
    public class PowerAttackConfig : SpecialAbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField]
        float extraDamage = 0;

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();

            behaviourComponent.SetConfig = this;
            behaviour = behaviourComponent;
        }
    }
}
