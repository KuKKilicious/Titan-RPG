using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RPG.Characters
{
    [CreateAssetMenu(menuName = "RPG/Special Ability/Power Attack")]
    public class PowerAttackConfig : AbilityConfig
    {
        [Header("Power Attack Specific")]
        [SerializeField]
        private float extraDamage = 0;

        public float ExtraDamage {
            get {
                return extraDamage;
            }
        }

        public override void AttachComponentTo(GameObject gameObjectToAttachTo)
        {
            var behaviourComponent = gameObjectToAttachTo.AddComponent<PowerAttackBehaviour>();

            behaviourComponent.SetConfig = this;
            behaviour = behaviourComponent;
        }
    }
}
